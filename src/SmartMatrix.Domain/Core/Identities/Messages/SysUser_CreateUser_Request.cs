using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_CreateUser_Request
    {
        public SysUser_InputPayload User { get; set; }        
    }
}