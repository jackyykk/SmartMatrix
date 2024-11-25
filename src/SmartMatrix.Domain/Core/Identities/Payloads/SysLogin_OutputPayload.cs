using System;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Domain.Core.Identities.Payloads
{
    public class SysLogin_OutputPayload : AuditablePayload<int>
    {

        #region Properties

        public string PartitionKey { get; set; }
        public int SysUserId { get; set; }
        public string LoginProvider { get; set; }
        public string LoginType { get; set; }
        public string LoginName { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string RefreshToken { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime? RefreshTokenExpires { get; set; }
        public string Description { get; set; }

        public new string Status { get; set; }        

        #endregion

        #region Options

        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.Sys_SmartMatrix;
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

        public class OwnerOptions
        {
            public const string System = CommonConstants.DbEntityOwner.System;
        }

        #endregion

        #region Methods
        
                

        #endregion
    
    }
}