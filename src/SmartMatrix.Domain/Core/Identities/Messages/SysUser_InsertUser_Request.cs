using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_InsertUser_Request
    {
        public SysUserPayload User { get; set; }        
    }
}