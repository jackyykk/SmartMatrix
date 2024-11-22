using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.Payloads;

namespace SmartMatrix.Domain.Demos.SimpleNoteDemo.Messages
{
    public class SimpleNote_GetById_Response
    {
        public static class StatusCodes
        {
            public const int Success = MessageConstants.StatusCodes.Success;
            public const int Unknown_Error = MessageConstants.StatusCodes.Unknown_Error;
            public const int Invalid_Request = MessageConstants.StatusCodes.Invalid_Request;                        
        }

        public static class StatusTexts
        {
            public const string Invalid_Request = MessageConstants.StatusTexts.Invalid_Request;                     
        }
        
        public SimpleNotePayload SimpleNote { get; set; }
    }
}