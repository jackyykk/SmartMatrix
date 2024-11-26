using System.Collections.Generic;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Domain.Core.Identities.Payloads
{
    public class SysRole_OutputPayload : AuditablePayload<int>
    {

        #region Properties

        public string PartitionKey { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }        

        #endregion

        #region Options

        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.Sys_SmartMatrix;
        }

        public class TypeOptions
        {
            public const string BuiltInRole = "built-in-role";
            public const string NormalRole = "normal-role";
        }

        public class CategoryOptions
        {
            public const string Normal = "normal";
            public const string API = "api";
        }

        public class RoleCodeOptions
        {
            public const string sysAdmin = "sys-admin::role";
            public const string sysPowerUser = "sys-power_user::role";
            public const string sysSupport = "sys-support::role";
            public const string sysUser = "sys-user::role";
            public const string sysGuest = "sys-guest::role";
            public const string sysApi = "sys-api::role";
            public const string sysAdminApi = "sys-admin_api::role";
        }

        public class StatusOptions
        {
            public const string Active = CommonConstants.DbEntityStatus.Active;
            public const string Deleted = CommonConstants.DbEntityStatus.Deleted;
        }

        public class OwnerOptions
        {
            public const string System = CommonConstants.DbEntityOwner.System;
        }

        #endregion

        #region Methods        

        public SysRole ToSysRole()
        {
            return new SysRole
            {
                Id = Id,
                Status = Status,
                IsDeleted = IsDeleted,
                CreatedAt = CreatedAt,
                CreatedBy = CreatedBy,
                ModifiedAt = ModifiedAt,
                ModifiedBy = ModifiedBy,
                DeletedAt = DeletedAt,
                DeletedBy = DeletedBy,
                InternalRemark = InternalRemark,
                PartitionKey = PartitionKey,
                Type = Type,
                Category = Category,
                RoleCode = RoleCode,
                RoleName = RoleName,
                Description = Description,
            };
        }

        #endregion
                
    }
}