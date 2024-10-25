using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Entities.Identities
{
    public class AppRole : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}