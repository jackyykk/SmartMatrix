using SmartMatrix.Core.Abstractions.Domain;

namespace SmartMatrix.Domain.Entities.Identities
{
    public class AppRole : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}