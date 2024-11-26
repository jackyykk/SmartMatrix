using Microsoft.EntityFrameworkCore;
using SmartMatrix.DataAccess.AuditLogging;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.DataAccess.DbContexts
{
    public abstract class CoreBaseDbContext : AuditableDbContext
    {
        public CoreBaseDbContext(DbContextOptions options) : base(options) { }

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
                b.ToTable("sm_core_audit_logs");
                b.HasKey(p => p.Id);
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

            builder.Entity<SysUser>(b =>
            {
                b.ToTable("sm_core_sysusers");                
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");                
                b.Property(p => p.Status).HasColumnName("status");
                b.Property(p => p.IsDeleted).HasColumnName("is_deleted");
                b.Property(p => p.CreatedAt).HasColumnName("created_at");
                b.Property(p => p.CreatedBy).HasColumnName("created_by");                                
                b.Property(p => p.ModifiedAt).HasColumnName("modified_at");
                b.Property(p => p.ModifiedBy).HasColumnName("modified_by");                
                b.Property(p => p.DeletedAt).HasColumnName("deleted_at");
                b.Property(p => p.DeletedBy).HasColumnName("deleted_by");
                b.Property(p => p.InternalRemark).HasColumnName("internal_remark");
                b.Property(p => p.PartitionKey).HasColumnName("partition_key");
                b.Property(p => p.Type).HasColumnName("type");
                b.Property(p => p.UserName).HasColumnName("username");
                b.Property(p => p.DisplayName).HasColumnName("display_name");
                b.Property(p => p.GivenName).HasColumnName("given_name");
                b.Property(p => p.Surname).HasColumnName("surname");
                b.Property(p => p.Email).HasColumnName("email");                
                b.Ignore(p => p.SkipAudit);

                b.HasMany(u => u.Logins)
                    .WithOne(l => l.User)
                    .HasForeignKey(l => l.SysUserId);
                
                b.HasMany(u => u.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.SysUserId);                
            });

            builder.Entity<SysUserRole>(b =>
            {                
                b.ToTable("sm_core_sysuser_roles");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");                
                b.Property(p => p.Status).HasColumnName("status");
                b.Property(p => p.IsDeleted).HasColumnName("is_deleted");
                b.Property(p => p.CreatedAt).HasColumnName("created_at");
                b.Property(p => p.CreatedBy).HasColumnName("created_by");                                
                b.Property(p => p.ModifiedAt).HasColumnName("modified_at");
                b.Property(p => p.ModifiedBy).HasColumnName("modified_by");                
                b.Property(p => p.DeletedAt).HasColumnName("deleted_at");
                b.Property(p => p.DeletedBy).HasColumnName("deleted_by");
                b.Property(p => p.InternalRemark).HasColumnName("internal_remark");
                b.Property(p => p.SysUserId).HasColumnName("sysuser_id");
                b.Property(p => p.SysRoleId).HasColumnName("sysrole_id");
            });

            builder.Entity<SysLogin>(b =>
            {                
                b.ToTable("sm_core_syslogins");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.Status).HasColumnName("status");
                b.Property(p => p.IsDeleted).HasColumnName("is_deleted");
                b.Property(p => p.CreatedAt).HasColumnName("created_at");
                b.Property(p => p.CreatedBy).HasColumnName("created_by");                                
                b.Property(p => p.ModifiedAt).HasColumnName("modified_at");
                b.Property(p => p.ModifiedBy).HasColumnName("modified_by");                
                b.Property(p => p.DeletedAt).HasColumnName("deleted_at");
                b.Property(p => p.DeletedBy).HasColumnName("deleted_by");
                b.Property(p => p.InternalRemark).HasColumnName("internal_remark");
                b.Property(p => p.PartitionKey).HasColumnName("partition_key");                
                b.Property(p => p.SysUserId).HasColumnName("sysuser_id");
                b.Property(p => p.LoginProvider).HasColumnName("login_provider");
                b.Property(p => p.LoginType).HasColumnName("login_type");
                b.Property(p => p.LoginName).HasColumnName("login_name");
                b.Property(p => p.Password).HasColumnName("password");
                b.Property(p => p.PasswordHash).HasColumnName("password_hash");
                b.Property(p => p.PasswordSalt).HasColumnName("password_salt");
                b.Property(p => p.RefreshToken).HasColumnName("refresh_token");
                b.Property(p => p.RefreshTokenExpires).HasColumnName("refresh_token_expires");
                b.Property(p => p.Description).HasColumnName("description");                
                b.Ignore(p => p.SkipAudit);
            });

            builder.Entity<SysRole>(b =>
            {                
                b.ToTable("sm_core_sysroles");
                b.HasKey(p => p.Id);
                b.Property(p => p.Id).HasColumnName("id");
                b.Property(p => p.Status).HasColumnName("status");
                b.Property(p => p.IsDeleted).HasColumnName("is_deleted");
                b.Property(p => p.CreatedAt).HasColumnName("created_at");
                b.Property(p => p.CreatedBy).HasColumnName("created_by");                                
                b.Property(p => p.ModifiedAt).HasColumnName("modified_at");
                b.Property(p => p.ModifiedBy).HasColumnName("modified_by");                
                b.Property(p => p.DeletedAt).HasColumnName("deleted_at");
                b.Property(p => p.DeletedBy).HasColumnName("deleted_by");
                b.Property(p => p.InternalRemark).HasColumnName("internal_remark");
                b.Property(p => p.PartitionKey).HasColumnName("partition_key");                
                b.Property(p => p.Type).HasColumnName("type");
                b.Property(p => p.Category).HasColumnName("category");
                b.Property(p => p.RoleCode).HasColumnName("role_code");
                b.Property(p => p.RoleName).HasColumnName("role_name");
                b.Property(p => p.Description).HasColumnName("description");
                b.Ignore(p => p.SkipAudit);
            });            
        }
    }
}