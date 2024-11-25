using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Demos.SimpleNoteDemo.Payloads
{
    public class SimpleNote_OutputPayload : AuditablePayload<int>
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }
    }
}