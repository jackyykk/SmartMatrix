using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysLogin : AuditableEntity<int>
    {
        public string PartitionKey { get; set; }
        public int SysUserId { get; set; }
        public string LoginProvider { get; set; }
        public string LoginType { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Description { get; set; }

        public new string Status { get; set; } = StatusOption.Active;

        public class StatusOption
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Disabled = CommonConstants.DbEntityStatus.Disabled;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

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
                PartitionKey = l.PartitionKey,
                SysUserId = l.SysUserId,
                LoginProvider = l.LoginProvider,
                LoginType = l.LoginType,
                LoginName = l.LoginName,
                PasswordHash = l.PasswordHash,
                Description = l.Description
            };
        }
    }
}