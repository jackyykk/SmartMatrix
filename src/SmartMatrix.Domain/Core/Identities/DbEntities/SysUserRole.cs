using SmartMatrix.Core.BaseClasses.Common;
using SmartMatrix.Domain.Constants;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUserRole : AuditableEntity<long>
    {
        #region Properties

        public int SysUserId { get; set; }
        public int SysRoleId { get; set; }
        public SysUser User { get; set; }
        public SysRole Role { get; set; }

        public new string Status { get; set; } = StatusOptions.Active;

        #endregion

        #region Options

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

        public static SysUserRole Copy(SysUserRole x)
        {
            return new SysUserRole
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
                SysUserId = x.SysUserId,                
                SysRoleId = x.SysRoleId,                
            };
        }

        public static SysUserRole CopyAsNew(SysUserRole x)
        {
            return new SysUserRole
            {
                CreatedBy = OwnerOptions.System,
                SysUserId = x.SysUserId,                
                SysRoleId = x.SysRoleId,                
            };
        }

        #endregion
    
    }
}