using System.Data;
using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.DataAccess.DbContexts
{
    public class DemoWriteDbContext : DemoBaseDbContext, IDemoWriteDbContext
    {
        private readonly IDateTimeService _dateTimeSvc;
        private readonly IAuthenticatedUserService _userSvc;
        public DbContext DbContext => this;        
        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();

        // DBSet
        public DbSet<SimpleNote> SimpleNotes { get; set; }        

        public DemoWriteDbContext(DbContextOptions<DemoWriteDbContext> options, IDateTimeService dateTimeSvc, IAuthenticatedUserService userSvc) : base(options)
        {
            _dateTimeSvc = dateTimeSvc;
            _userSvc = userSvc;
        }                

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
                        entry.Entity.CreatedBy = string.IsNullOrEmpty(entry.Entity.CreatedBy) ? _userSvc.UserNameIdentifier : entry.Entity.CreatedBy;
                        break;

                    case EntityState.Modified:
                        entry.Entity.ModifiedAt = _dateTimeSvc.UtcNow;
                        // Set ModifiedBy to be the current user if it's empty
                        entry.Entity.ModifiedBy = string.IsNullOrEmpty(entry.Entity.ModifiedBy) ? _userSvc.UserNameIdentifier : entry.Entity.ModifiedBy;
                        break;

                    case EntityState.Deleted:
                        entry.Entity.DeletedAt = _dateTimeSvc.UtcNow;
                        // Set DeletedBy to be the current user if it's empty
                        entry.Entity.DeletedBy = string.IsNullOrEmpty(entry.Entity.DeletedBy) ? _userSvc.UserNameIdentifier : entry.Entity.DeletedBy;
                        break;
                }
            }

            if (string.IsNullOrEmpty(_userSvc.UserNameIdentifier))
            {
                // Skip audit logs if user is not authenticated
                return await base.SaveChangesAsync(cancellationToken);

                // Test
                //bool skipAudit = skipAudits.Any(b => b == true);
                //return await base.SaveChangesAsync(false, _dateTimeSvc.UtcNow, "system");
            }
            else
            {
                // skip audit logging if any entity has SkipAudit flag set to true
                bool skipAudit = skipAudits.Any(b => b == true);
                return await base.SaveChangesAsync(skipAudit, _dateTimeSvc.UtcNow, _userSvc.UserNameIdentifier);
            }
        }        
    }
}