using System;

namespace SmartMatrix.Domain.Core.Identities
{
    public class SysSecret
    {        
        public string Jwt_Key { get; set; }
        public string Jwt_Issuer { get; set; }
        public string Jwt_Audience { get; set; }
        public string AccessToken_Format { get; set; }
        public double AccessToken_LifeInMinutes { get; set; }
        public DateTime? AccessToken_Expires { get; set; }
        public string RefreshToken_Format { get; set; }
        public double RefreshToken_LifeInMinutes { get; set; }
        public DateTime? RefreshToken_Expires { get; set; }
        public string OneTimeToken_Format { get; set; }
        public double OneTimeToken_LifeInMinutes { get; set; }
        public DateTime? OneTimeToken_Expires { get; set; }

        public SysSecret Copy()
        {
            return new SysSecret
            {                
                Jwt_Key = Jwt_Key,
                Jwt_Issuer = Jwt_Issuer,
                Jwt_Audience = Jwt_Audience,
                AccessToken_Format = AccessToken_Format,
                AccessToken_LifeInMinutes = AccessToken_LifeInMinutes,
                AccessToken_Expires = AccessToken_Expires,
                RefreshToken_Format = RefreshToken_Format,
                RefreshToken_LifeInMinutes = RefreshToken_LifeInMinutes,
                RefreshToken_Expires = RefreshToken_Expires,
                OneTimeToken_Format = OneTimeToken_Format,
                OneTimeToken_LifeInMinutes = OneTimeToken_LifeInMinutes,
                OneTimeToken_Expires = OneTimeToken_Expires
            };
        }
    }
}