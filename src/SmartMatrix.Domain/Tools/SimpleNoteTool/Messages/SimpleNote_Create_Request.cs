using SmartMatrix.Domain.Tools.SimpleNoteTool.Payloads;

namespace SmartMatrix.Domain.Tools.SimpleNoteTool.Messages
{
    public class SimpleNote_Create_Request
    {
        public SimpleNote_InputPayload SimpleNote { get; set; }
    }
}