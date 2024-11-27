using System;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_UpdateTokens_Request
    {
        public int LoginId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }        
        public string OneTimeToken { get; set; }
        public DateTime? OneTimeTokenExpires { get; set; }
    }
}