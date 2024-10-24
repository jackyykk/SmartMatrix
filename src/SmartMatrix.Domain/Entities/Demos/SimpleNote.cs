using SmartMatrix.Core.Abstractions.Domain;

namespace SmartMatrix.Domain.Entities.Demos
{
    public class SimpleNote : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Owner { get; set; }
    }
}