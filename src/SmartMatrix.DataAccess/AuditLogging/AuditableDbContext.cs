using Microsoft.EntityFrameworkCore;

namespace SmartMatrix.DataAccess.AuditLogging
{
    public abstract class AuditableDbContext : DbContext
    {
        public AuditableDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Audit> AuditLogs { get; set; }

        public virtual async Task<int> SaveChangesAsync(bool skipAudit, DateTime actionTime, string userName)
        {
            if (!skipAudit)
            {
                OnBeforeSaveChanges(actionTime, userName);
            }

            var result = await base.SaveChangesAsync();
            return result;
        }

        private void OnBeforeSaveChanges(DateTime actionTime, string userName)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;

                var auditEntry = new AuditEntry(entry)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserName = userName,
                    ActionTime = actionTime
                };

                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue ?? "";
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.AuditType = AuditType.Create;
                            auditEntry.NewValues[propertyName] = property.CurrentValue ?? "";
                            break;

                        case EntityState.Deleted:
                            auditEntry.AuditType = AuditType.Delete;
                            auditEntry.OldValues[propertyName] = property.OriginalValue ?? "";
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.AffectedColumns.Add(propertyName);
                                auditEntry.AuditType = AuditType.Update;
                                auditEntry.OldValues[propertyName] = property.OriginalValue ?? "";
                                auditEntry.NewValues[propertyName] = property.CurrentValue ?? "";
                            }
                            break;
                    }
                }
            }

            foreach (var auditEntry in auditEntries)
            {
                AuditLogs.Add(auditEntry.ToAudit());
            }
        }

        protected string Get_Guid_DefaultValue_Sql()
        {
            // Return the appropriate default value SQL based on the database provider
            if (Database.IsSqlServer())
            {
                return "NEWSEQUENTIALID()";
            }            
            else
            {
                throw new NotSupportedException("Database provider not supported for GUID generation.");
            }

            /*

            if (Database.IsNpgsql())
            {
                return "uuid_generate_v4()"; // For PostgreSQL with the uuid-ossp extension
            }
            if (Database.IsSqlite())
            {
                return "lower(hex(randomblob(4))) || '-' || lower(hex(randomblob(2))) || '-4' || substr(lower(hex(randomblob(2))), 2) || '-' || substr('89ab', abs(random()) % 4 + 1, 1) || substr(lower(hex(randomblob(2))), 2) || '-' || lower(hex(randomblob(6)))"; // For SQLite
            }
            
            */
        }
    }
}