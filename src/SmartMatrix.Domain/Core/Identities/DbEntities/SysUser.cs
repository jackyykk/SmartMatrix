using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUser : AuditableEntity<int>
    {
        // User Information
        public string Type { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        
        public new string Status { get; set; } = StatusOption.Active;

        public class StatusOption
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Inactive = CommonConstants.DbEntityStatus.Inactive;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public List<SysLogin> Logins { get; set; } = new List<SysLogin>();

        public static SysUser Copy(SysUser u)
        {
            var user = new SysUser
            {
                Id = u.Id,
                Status = u.Status,
                IsDeleted = u.IsDeleted,
                CreatedAt = u.CreatedAt,
                CreatedBy = u.CreatedBy,
                ModifiedAt = u.ModifiedAt,
                ModifiedBy = u.ModifiedBy,
                DeletedAt = u.DeletedAt,
                DeletedBy = u.DeletedBy,
                Type = u.Type,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                GivenName = u.GivenName,
                Surname = u.Surname
            };
            
            foreach (var login in u.Logins)
            {
                user.Logins.Add(SysLogin.Copy(login));
            }

            return user;
        }
    }
}