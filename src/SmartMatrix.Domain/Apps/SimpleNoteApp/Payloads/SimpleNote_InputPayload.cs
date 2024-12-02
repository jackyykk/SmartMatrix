using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.Domain.Apps.SimpleNoteApp.Payloads
{
    public class SimpleNote_InputPayload : AuditablePayload<int>
    {
        public string Category { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Owner { get; set; }
    }
}