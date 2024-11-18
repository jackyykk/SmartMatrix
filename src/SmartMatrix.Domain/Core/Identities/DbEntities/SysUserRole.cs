namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysUserRole
    {
        public int SysUserId { get; set; }
        public int SysRoleId { get; set; }
        public SysUser User { get; set; }
        public SysRole Role { get; set; }
    }
}