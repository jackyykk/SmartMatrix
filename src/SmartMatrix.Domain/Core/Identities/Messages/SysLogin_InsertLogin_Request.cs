using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_InsertLogin_Request
    {
        public SysLoginPayload Login { get; set; }        
    }
}