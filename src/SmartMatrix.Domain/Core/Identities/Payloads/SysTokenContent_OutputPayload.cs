using System;
using System.Collections.Generic;

namespace SmartMatrix.Domain.Core.Identities.Payloads
{
    public class SysTokenContent_OutputPayload
    {        

        #region Properties

        public string LoginProviderName { get; set; }       // e.g. Google, Standard, etc.
        public string LoginNameIdentifier { get; set; }     // Login Name, e.g. Google Email, LoginName of SysLogin in Standard Login, etc.        
        public string UserNameIdentifier { get; set; }      // User Name, e.g. Google Email, UserName of SysLogin in Standard Login, etc.
        public string Email { get; set; }
        public string Name { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        #endregion

        #region Options

        #endregion

        #region Methods

        #endregion
    }
}