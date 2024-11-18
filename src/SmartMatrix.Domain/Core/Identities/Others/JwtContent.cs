using System;

namespace SmartMatrix.Domain.Core.Identities.Others
{
    public class JwtContent
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime? Expires { get; set; }
        public string LoginProviderName { get; set; }
        public string Sid { get; set; }
        public string NameIdentifier { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
    }
}