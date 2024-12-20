using System.Collections.Generic;
using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;
using SmartMatrix.Domain.Tools.SimpleNoteTool.Payloads;

namespace SmartMatrix.Domain.Tools.SimpleNoteTool.Messages
{
    public class SimpleNote_GetListByOwner_Response
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
        
        public List<SimpleNote_OutputPayload> SimpleNotes { get; set; } = new List<SimpleNote_OutputPayload>();
    }
}