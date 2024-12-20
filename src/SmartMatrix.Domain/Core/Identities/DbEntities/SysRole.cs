using System;
using System.Collections.Generic;
using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysRole : AuditableEntity<int>
    {

        #region Properties

        public string PartitionKey { get; set; } = PartitionKeyOptions.SmartMatrix;
        public string Classification { get; set; } = ClassificationOptions.Normal;
        public string Type { get; set; } = TypeOptions.Standard;        
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }

        public new string Status { get; set; } = StatusOptions.Active;

        public ICollection<SysUserRole> UserRoles { get; set; } = new List<SysUserRole>();

        #endregion

        #region Options

        public class PartitionKeyOptions
        {
            public const string SmartMatrix = CommonConstants.PartitionKeys.Sys_SmartMatrix;
        }

        public class ClassificationOptions
        {            
            public const string BuiltIn = "built_in";
            public const string Normal = "normal";            
        }

        public class TypeOptions
        {
            public const string Standard = "standard";
            public const string API = "api";            
        }

        public class RoleCodeOptions
        {
            public const string sysAdmin = "sys-admin:role";
            public const string sysPowerUser = "sys-power_user:role";
            public const string sysSupport = "sys-support:role";
            public const string sysUser = "sys-user:role";
            public const string sysGuest = "sys-guest:role";
            public const string sysStandardApi = "sys-standard_api:role";
            public const string sysAdminApi = "sys-admin_api:role";
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

        public static SysRole Copy(SysRole x)
        {
            return new SysRole
            {
                Id = x.Id,
                Guid = x.Guid,
                Status = x.Status,
                IsDeleted = x.IsDeleted,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                ModifiedAt = x.ModifiedAt,
                ModifiedBy = x.ModifiedBy,
                DeletedAt = x.DeletedAt,
                DeletedBy = x.DeletedBy,
                InternalRemark = x.InternalRemark,
                PartitionKey = x.PartitionKey,
                Classification = x.Classification,
                Type = x.Type,                
                RoleCode = x.RoleCode,
                RoleName = x.RoleName,                
                Description = x.Description
            };
        }

        public static SysRole CopyAsNew(SysRole x)
        {
            return new SysRole
            {
                CreatedBy = OwnerOptions.System,
                PartitionKey = x.PartitionKey,
                Classification = x.Classification,
                Type = x.Type,                
                RoleCode = x.RoleCode,
                RoleName = x.RoleName,                
                Description = x.Description
            };
        }

        public SysRole UpdateStatus_From(SysUserRole x)
        {
            Status = x.Status;
            IsDeleted = x.IsDeleted;
            CreatedAt = x.CreatedAt;
            CreatedBy = x.CreatedBy;
            ModifiedAt = x.ModifiedAt;
            ModifiedBy = x.ModifiedBy;
            DeletedAt = x.DeletedAt;
            DeletedBy = x.DeletedBy;
            InternalRemark = x.InternalRemark;
            
            return this;
        }

        #endregion
    
    }
}