using System;

namespace SmartMatrix.Domain.Core.Identities
{
    public class SysSecret
    {
        public string Format { get; set; }
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double LifeInMinutes { get; set; }
        public DateTime? Expires { get; set; }        

        public SysSecret Copy()
        {
            return new SysSecret
            {
                Format = Format,
                Key = Key,
                Issuer = Issuer,
                Audience = Audience,
                LifeInMinutes = LifeInMinutes,
                Expires = Expires
            };
        }
    }
}