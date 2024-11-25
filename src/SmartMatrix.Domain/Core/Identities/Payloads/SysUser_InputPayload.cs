using System.Collections.Generic;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.Payloads
{
    public class SysUser_InputPayload : AuditablePayload<int>
    {

        #region Properties

        public string PartitionKey { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }

        public new string Status { get; set; }

        public ICollection<SysLogin_InputPayload> Logins { get; set; } = new List<SysLogin_InputPayload>();
        public ICollection<SysRole_InputPayload> Roles { get; set; } = new List<SysRole_InputPayload>();

        #endregion

        #region Options

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

        #region Methods

        

        #endregion

        #endregion        
    }
}