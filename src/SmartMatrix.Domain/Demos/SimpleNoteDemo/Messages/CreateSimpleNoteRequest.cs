using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages
{
    public class CreateSimpleNoteRequest
    {
        public SimpleNote SimpleNote { get; set; }
    }
}