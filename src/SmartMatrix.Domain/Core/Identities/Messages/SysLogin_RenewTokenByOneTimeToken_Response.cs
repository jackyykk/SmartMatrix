using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_RenewTokenByOneTimeToken_Response
    {
        public static class StatusCodes
        {
            public const int Success = MessageConstants.StatusCodes.Success;
            public const int Unknown_Error = MessageConstants.StatusCodes.Unknown_Error;
            public const int Invalid_Request = MessageConstants.StatusCodes.Invalid_Request;
                        
            // Other errors
            public const int User_NotFound = MessageConstants.StatusCodes.SysUser_Codes.User_NotFound;
            public const int User_Disabled = MessageConstants.StatusCodes.SysUser_Codes.User_Disabled;
            public const int User_Deleted = MessageConstants.StatusCodes.SysUser_Codes.User_Deleted;

            public const int Login_NotFound = MessageConstants.StatusCodes.SysLogin_Codes.Login_NotFound;            
            public const int Login_Disabled = MessageConstants.StatusCodes.SysLogin_Codes.Login_Disabled;
            public const int Login_Deleted = MessageConstants.StatusCodes.SysLogin_Codes.Login_Deleted;
            public const int LoginName_Empty = MessageConstants.StatusCodes.SysLogin_Codes.LoginName_Empty;
                                    
            public const int OneTimeToken_Expired = MessageConstants.StatusCodes.SysLogin_Codes.OneTimeToken_Expired;
            public const int Tokens_Update_Failed = MessageConstants.StatusCodes.SysLogin_Codes.Tokens_Update_Failed;
        }

        public static class StatusTexts
        {
            public const string Invalid_Request = MessageConstants.StatusTexts.Invalid_Request;
            public const string Invalid_Token = "The token is invalid.";            
        }

        public SysToken_OutputPayload Token { get; set; }
    }
}