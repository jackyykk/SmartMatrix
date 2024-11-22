using Microsoft.EntityFrameworkCore;
using SmartMatrix.DataAccess.AuditLogging;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.DataAccess.DbContexts
{
    public abstract class DemoBaseDbContext : AuditableDbContext
    {
        public DemoBaseDbContext(DbContextOptions options) : base(options) { }

        public void SetConnection(string connectionString)
        {            
            this.Database.SetConnectionString(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,4)");
            }

            base.OnModelCreating(builder);

            builder.Entity<Audit>(b =>
            {
                b.HasKey("Id");
                b.ToTable("sm_core_audit_logs");
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.UserName).HasColumnName("username");                                
                b.Property(p => p.Type).HasColumnName("type");                
                b.Property(p => p.ActionTime).HasColumnName("action_time");
                b.Property(p => p.TableName).HasColumnName("table_name");
                b.Property(p => p.OldValues).HasColumnName("old_values");
                b.Property(p => p.NewValues).HasColumnName("new_values");
                b.Property(p => p.AffectedColumns).HasColumnName("affected_columns");
                b.Property(p => p.PrimaryKey).HasColumnName("primary_key");
            });

            builder.Entity<SimpleNote>(b =>
            {
                b.HasKey("Id");
                b.ToTable("sm_demo_simple_notes");
                b.Property(p => p.Id).HasColumnName("id");                
                b.Property(p => p.Status).HasColumnName("status");
                b.Property(p => p.IsDeleted).HasColumnName("is_deleted");
                b.Property(p => p.CreatedAt).HasColumnName("created_at");
                b.Property(p => p.CreatedBy).HasColumnName("created_by");                                
                b.Property(p => p.ModifiedAt).HasColumnName("modified_at");
                b.Property(p => p.ModifiedBy).HasColumnName("modified_by");                
                b.Property(p => p.DeletedAt).HasColumnName("deleted_at");
                b.Property(p => p.DeletedBy).HasColumnName("deleted_by");
                b.Property(p => p.Category).HasColumnName("category");
                b.Property(p => p.Title).HasColumnName("title");
                b.Property(p => p.Body).HasColumnName("body");
                b.Property(p => p.Owner).HasColumnName("owner");
                b.Ignore(p => p.SkipAudit);
            });
        }
    }
}