using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysRole : AuditableEntity<int>
    {
        public string PartitionKey { get; set; }
        public string Type { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}