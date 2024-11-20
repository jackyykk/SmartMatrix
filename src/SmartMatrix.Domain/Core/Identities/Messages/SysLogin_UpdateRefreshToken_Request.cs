using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_UpdateRefreshToken_Request
    {
        public SysLogin Login { get; set; }        
    }
}