using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysLogin : AuditableEntity<int>
    {
        public string PartitionKey { get; set; } = PartitionKeyOptions.SmartMatrix;
        public int SysUserId { get; set; }
        public string LoginProvider { get; set; } = LoginProviderOptions.Standard;
        public string LoginType { get; set; }
        public string LoginName { get; set; }
        public string PasswordHash { get; set; }
        public string Description { get; set; }

        public new string Status { get; set; } = StatusOptions.Active;

        public SysUser User { get; set; }

        // Options
        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.SmartMatrix;
        }

        public class LoginProviderOptions
        {
            public const string Standard = "standard";
            public const string Google = "google";
        }

        public class LoginTypeOptions
        {
            public const string Web = "web";
            public const string API = "api";            
        }

        public class StatusOptions
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Disabled = CommonConstants.DbEntityStatus.Disabled;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public static SysLogin Copy(SysLogin x)
        {
            return new SysLogin
            {
                Id = x.Id,
                Status = x.Status,
                IsDeleted = x.IsDeleted,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt,
                ModifiedBy = x.ModifiedBy,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy,
                PartitionKey = x.PartitionKey,
                SysUserId = x.SysUserId,
                LoginProvider = x.LoginProvider,
                LoginType = x.LoginType,
                LoginName = x.LoginName,
                PasswordHash = x.PasswordHash,
                Description = x.Description
            };
        }
    }
}