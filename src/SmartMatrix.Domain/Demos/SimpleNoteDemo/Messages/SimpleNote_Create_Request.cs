using SmartMatrix.Domain.Demos.SimpleNoteDemo.Payloads;

namespace SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages
{
    public class SimpleNote_Create_Request
    {
        public SimpleNote_InputPayload SimpleNote { get; set; }
    }
}