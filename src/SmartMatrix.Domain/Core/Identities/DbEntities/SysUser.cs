using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUser : AuditableEntity<int>
    {
        // User Information
        public string PartitionKey { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        
        public new string Status { get; set; } = StatusOption.Active;

        public class StatusOption
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Disabled = CommonConstants.DbEntityStatus.Disabled;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public List<SysLogin> Logins { get; set; } = new List<SysLogin>();
        public List<SysRole> Roles { get; set; } = new List<SysRole>();

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
                PartitionKey = u.PartitionKey,
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

            foreach (var role in u.Roles)
            {
                user.Roles.Add(SysRole.Copy(role));
            }

            return user;
        }

        public static SysUser Copy(SysUser u, List<SysLogin> logins, List<SysRole> roles)
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
                PartitionKey = u.PartitionKey,
                Type = u.Type,
                UserName = u.UserName,
                DisplayName = u.DisplayName,
                GivenName = u.GivenName,
                Surname = u.Surname
            };

            foreach (var login in logins)
            {
                user.Logins.Add(SysLogin.Copy(login));
            }
            
            foreach (var role in roles)
            {
                user.Roles.Add(SysRole.Copy(role));
            }

            return user;
        }
    }
}