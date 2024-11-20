using System;

namespace SmartMatrix.Domain.Core.Identities
{
    public class JwtSecret
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double LifeInMinutes { get; set; }
        public DateTime? Expires { get; set; }        

        public JwtSecret Copy()
        {
            return new JwtSecret
            {
                Key = Key,
                Issuer = Issuer,
                Audience = Audience,
                LifeInMinutes = LifeInMinutes,
                Expires = Expires
            };
        }
    }
}