using System;
using System.Collections.Generic;

namespace SmartMatrix.Domain.Core.Identities
{
    public class JwtContent
    {
        public JwtSecret Secret { get; set; } = new JwtSecret();        
        public string LoginProviderName { get; set; }
        public string LoginNameIdentifier { get; set; }
        public string Sid { get; set; }
        public string UserNameIdentifier { get; set; }        
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }        
    }
}