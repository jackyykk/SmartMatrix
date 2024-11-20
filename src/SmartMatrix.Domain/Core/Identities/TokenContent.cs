using System;

namespace SmartMatrix.Domain.Core.Identities
{
    public class TokenContent
    {
        public string AuthToken { get; set; }
        public DateTime? AuthExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }
}