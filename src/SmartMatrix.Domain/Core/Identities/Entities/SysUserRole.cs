namespace SmartMatrix.Domain.Core.Identities.Entities
{
    public class SysUserRole
    {
        public int AppUserId { get; set; }
        public int AppRoleId { get; set; }
        public SysUser User { get; set; }
        public SysRole Role { get; set; }
    }
}