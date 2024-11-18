using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Core.Identities.DbEntities
{
    public class SysRole : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}