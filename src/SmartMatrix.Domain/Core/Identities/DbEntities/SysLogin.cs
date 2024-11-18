using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysLogin : AuditableEntity<int>
    {        
        public int SysUserId { get; set; }
        public string LoginProvider { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Description { get; set; }
        public SysUser User { get; set; }

        public static SysLogin Copy(SysLogin l)
        {
            return new SysLogin
            {
                Id = l.Id,
                Status = l.Status,
                IsDeleted = l.IsDeleted,
                CreatedAt = l.CreatedAt,
                CreatedBy = l.CreatedBy,
                ModifiedAt = l.ModifiedAt,
                ModifiedBy = l.ModifiedBy,
                DeletedAt = l.DeletedAt,
                DeletedBy = l.DeletedBy,
                SysUserId = l.SysUserId,
                LoginProvider = l.LoginProvider,
                LoginName = l.LoginName,
                PasswordHash = l.PasswordHash,
                Description = l.Description
            };
        }
    }
}