using System;

namespace SmartMatrix.Domain.Core.Identities
{
    public class TokenContent
    {
        public string AuthToken { get; set; }
        public double AuthToken_LifeInMinutes { get; set; }
        public DateTime? AuthToken_Expires { get; set; }        
        public string RefreshToken { get; set; }
        public double RefreshToken_LifeInMinutes { get; set; }
        public DateTime? RefreshToken_Expires { get; set; }        
    }
}