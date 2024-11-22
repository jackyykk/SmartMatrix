using SmartMatrix.Domain.Constants;
using SmartMatrix.Domain.Core.Identities.DbEntities;
using SmartMatrix.Domain.Core.Identities.Payloads;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_PerformLogin_Response
    {        
        public static class StatusCodes
        {
            public const int Success = MessageConstants.StatusCodes.Success;            
            public const int Unknown_Error = MessageConstants.StatusCodes.Unknown_Error;
            
            // Other errors
            public const int Invalid_Request = MessageConstants.StatusCodes.Invalid_Request;
            public const int User_NotFound = MessageConstants.StatusCodes.SysUser_Codes.User_NotFound;
            public const int Login_NotFound = MessageConstants.StatusCodes.SysLogin_Codes.Login_NotFound;
            public const int Login_Disabled = MessageConstants.StatusCodes.SysLogin_Codes.Login_Disabled;
            public const int Login_Deleted = MessageConstants.StatusCodes.SysLogin_Codes.Login_Deleted;
            public const int Password_NotMatch = MessageConstants.StatusCodes.SysLogin_Codes.Password_NotMatch;
            public const int RefreshToken_Update_Failed = MessageConstants.StatusCodes.SysLogin_Codes.RefreshToken_Update_Failed;
        }

        public static class StatusTexts
        {
            public const string Invalid_Request = MessageConstants.StatusTexts.Invalid_Request;
            public const string Unknown_Error = MessageConstants.StatusTexts.Unknown_Error;
            public const string Login_Failed = "Login failed";
        }

        public SysUserPayload User { get; set; }
        public SysTokenPayload Token { get; set; }
    }
}