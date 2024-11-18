using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities
{
    public class SimpleNote : AuditableEntity<int>
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }        
    }
}