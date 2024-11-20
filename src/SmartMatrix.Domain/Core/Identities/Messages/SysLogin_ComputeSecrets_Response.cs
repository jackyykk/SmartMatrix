using System.Collections.Generic;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_ComputeSecrets_Response
    {
        public List<int> UpdatedIds { get; set; } = new List<int>();
    }
}