using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_CreateLogin_Request
    {
        public SysLogin_InputPayload Login { get; set; }        
    }
}