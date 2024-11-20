using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_PerformLogin_Response
    {
        public SysUser User { get; set; }
        public TokenContent Token { get; set; }
    }
}