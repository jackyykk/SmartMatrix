using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByType_Response
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

        public SysUser_OutputPayload User { get; set; }
    }
}