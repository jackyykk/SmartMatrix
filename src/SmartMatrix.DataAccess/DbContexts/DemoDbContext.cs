using System.Data;
using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.DataAccess.AuditLogging;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;

namespace SmartMatrix.DataAccess.DbContexts
{
    public class DemoDbContext : AuditableDbContext, IDemoDbContext
    {
        private readonly IDateTimeService _dateTimeSvc;
        private readonly IAuthenticatedUserService _userSvc;
        public DbContext DbContext => this;

        public DemoDbContext(DbContextOptions<DemoDbContext> options, IDateTimeService dateTimeSvc, IAuthenticatedUserService userSvc) : base(options)
        {
            _dateTimeSvc = dateTimeSvc;
            _userSvc = userSvc;
        }

        public DbSet<SimpleNote> SimpleNotes { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            List<bool> skipAudits = new List<bool>();

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                skipAudits.Add(entry.Entity.SkipAudit);
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTimeSvc.UtcNow;
                        // Set CreatedBy to be the current user if it's empty
                        entry.Entity.CreatedBy = string.IsNullOrEmpty(entry.Entity.CreatedBy) ? _userSvc.UserAccountName : entry.Entity.CreatedBy;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = _dateTimeSvc.UtcNow;
                        // Set ModifiedBy to be the current user if it's empty
                        entry.Entity.ModifiedBy = string.IsNullOrEmpty(entry.Entity.ModifiedBy) ? _userSvc.UserAccountName : entry.Entity.ModifiedBy;
                        break;
                }
            }

            if (string.IsNullOrEmpty(_userSvc.UserAccountName))
            {
                // Skip audit logs if user is not authenticated
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // skip audit logs if any entity has SkipAudit flag set to true
                bool skipAudit = skipAudits.Any(b => b == true);
                return await base.SaveChangesAsync(skipAudit, _dateTimeSvc.UtcNow, _userSvc.UserAccountName);
            }
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
                b.ToTable("AuditLogs");
            });

            builder.Entity<SimpleNote>(b =>
            {
                b.HasKey("Id");
                b.ToTable("SimpleNotes");
                b.Ignore(c => c.SkipAudit);
            });
        }
    }
}