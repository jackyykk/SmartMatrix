using Microsoft.EntityFrameworkCore;
using SmartMatrix.DataAccess.AuditLogging;

namespace SmartMatrix.DataAccess.DbContexts
{
    public abstract class AppBaseDbContext : AuditableDbContext
    {
        public AppBaseDbContext(DbContextOptions options) : base(options) { }

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
                b.Property(p => p.Guid).HasColumnName("guid").HasDefaultValueSql(Get_Guid_DefaultValue_Sql());
                b.Property(p => p.UserName).HasColumnName("username");                                
                b.Property(p => p.Type).HasColumnName("type");                
                b.Property(p => p.ActionTime).HasColumnName("action_time");
                b.Property(p => p.TableName).HasColumnName("table_name");
                b.Property(p => p.OldValues).HasColumnName("old_values");
                b.Property(p => p.NewValues).HasColumnName("new_values");
                b.Property(p => p.AffectedColumns).HasColumnName("affected_columns");
                b.Property(p => p.PrimaryKey).HasColumnName("primary_key");
            });            
        }
    }
}