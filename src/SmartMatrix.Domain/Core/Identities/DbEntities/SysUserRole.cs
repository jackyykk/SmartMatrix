using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUserRole : BaseEntity<long>
    {
        #region Properties

        public int SysUserId { get; set; }
        public int SysRoleId { get; set; }
        public SysUser User { get; set; }
        public SysRole Role { get; set; }

        #endregion

        #region Methods

        public static SysUserRole Copy(SysUserRole x)
        {
            return new SysUserRole
            {
                Id = x.Id,
                SysUserId = x.SysUserId,                
                SysRoleId = x.SysRoleId,                
            };
        }

        public static SysUserRole CopyAsNew(SysUserRole x)
        {
            return new SysUserRole
            {                
                SysUserId = x.SysUserId,                
                SysRoleId = x.SysRoleId,                
            };
        }

        #endregion
    
    }
}