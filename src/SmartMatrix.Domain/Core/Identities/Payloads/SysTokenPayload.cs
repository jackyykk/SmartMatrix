using System;

namespace SmartMatrix.Domain.Core.Identities.Payloads
{
    public class SysTokenPayload
    {        
        public string AccessToken { get; set; }
        public double AccessToken_LifeInMinutes { get; set; }
        public DateTime? AccessToken_Expires { get; set; }        
        public string RefreshToken { get; set; }
        public double RefreshToken_LifeInMinutes { get; set; }
        public DateTime? RefreshToken_Expires { get; set; }
    }
}