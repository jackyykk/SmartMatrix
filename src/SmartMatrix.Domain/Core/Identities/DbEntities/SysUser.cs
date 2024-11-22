using System.Collections.Generic;
using System.Runtime.CompilerServices;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUser : AuditableEntity<int>
    {
        // User Information
        public string PartitionKey { get; set; } = PartitionKeyOptions.SmartMatrix;
        public string Type { get; set; } = TypeOptions.Normal_User;
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        
        public new string Status { get; set; } = StatusOptions.Active;

        public List<SysLogin> Logins { get; set; } = new List<SysLogin>();
        public List<SysRole> Roles { get; set; } = new List<SysRole>();

        public void ClearSecrets()
        {
            foreach (var login in Logins)
            {
                login.ClearSecrets();
            }
        }

        // Options
        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.Sys_SmartMatrix;
        }

        public class TypeOptions
        {
            public const string BuiltIn_Normal_User_Profile = "built_in_normal_user_profile";
            public const string BuiltIn_User = "built_in_user";
            public const string Normal_User = "normal_user";
        }

        public class StatusOptions
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Disabled = CommonConstants.DbEntityStatus.Disabled;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public class OwnerOptions
        {
            public const string System = CommonConstants.DbEntityOwner.System;            
        }

        public static SysUser Copy(SysUser x)
        {
            var user = new SysUser
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
                Type = x.Type,
                UserName = x.UserName,
                DisplayName = x.DisplayName,
                GivenName = x.GivenName,
                Surname = x.Surname,
                Email = x.Email
            };
            
            foreach (var login in x.Logins)
            {
                user.Logins.Add(SysLogin.Copy(login));
            }

            foreach (var role in x.Roles)
            {
                user.Roles.Add(SysRole.Copy(role));
            }

            return user;
        }

        public static SysUser Copy(SysUser x, List<SysLogin> logins, List<SysRole> roles)
        {
            var user = new SysUser  
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
                Type = x.Type,
                UserName = x.UserName,
                DisplayName = x.DisplayName,
                GivenName = x.GivenName,
                Surname = x.Surname,
                Email = x.Email
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

        public static SysUser CopyAsNew(SysUser x)
        {
            var user = new SysUser
            {
                CreatedBy = OwnerOptions.System,
                PartitionKey = x.PartitionKey,
                Type = x.Type,
                UserName = x.UserName,
                DisplayName = x.DisplayName,
                GivenName = x.GivenName,
                Surname = x.Surname,
                Email = x.Email
            };
            
            foreach (var login in x.Logins)
            {
                user.Logins.Add(SysLogin.CopyAsNew(login));
            }

            foreach (var role in x.Roles)
            {
                user.Roles.Add(SysRole.CopyAsNew(role));
            }

            return user;
        }
    }
}