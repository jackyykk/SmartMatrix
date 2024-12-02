using SmartMatrix.Domain.Apps.SimpleNoteApp.Payloads;

namespace SmartMatrix.Domain.Apps.SimpleNoteApp.Messages
{
    public class SimpleNote_Create_Request
    {
        public SimpleNote_InputPayload SimpleNote { get; set; }
    }
}