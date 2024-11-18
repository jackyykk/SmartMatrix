using System.Data;
using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.Services.Essential;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.DataAccess.DbContexts
{
    public class DemoReadDbContext : DemoBaseDbContext, IDemoReadDbContext
    {
        private readonly IDateTimeService _dateTimeSvc;
        private readonly IAuthenticatedUserService _userSvc;
        public DbContext DbContext => this;        
        public IDbConnection Connection => Database.GetDbConnection();
        public bool HasChanges => ChangeTracker.HasChanges();

        // DBSet
        public DbSet<SimpleNote> SimpleNotes { get; set; }
        
        public DemoReadDbContext(DbContextOptions<DemoReadDbContext> options, IDateTimeService dateTimeSvc, IAuthenticatedUserService userSvc) : base(options)
        {
            _dateTimeSvc = dateTimeSvc;
            _userSvc = userSvc;
        }
                
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await Task.FromException<int>(new InvalidOperationException("Read-only context cannot save changes."));
        }        
    }
}