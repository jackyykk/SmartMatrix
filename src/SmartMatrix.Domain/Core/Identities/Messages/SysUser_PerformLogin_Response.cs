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
            public const int Configuration_Error = MessageConstants.StatusCodes.Configuration_Error;

            // Other errors
            public const int Invalid_Request = MessageConstants.StatusCodes.Invalid_Request;
            public const int User_NotFound = MessageConstants.StatusCodes.SysUser_Codes.User_NotFound;
            public const int User_Insert_Failed = MessageConstants.StatusCodes.SysUser_Codes.User_Insert_Failed;
            
            public const int UserProfile_NotFound = MessageConstants.StatusCodes.SysUser_Codes.UserProfile_NotFound;
            public const int UserProfile_Disabled = MessageConstants.StatusCodes.SysUser_Codes.UserProfile_Disabled;
            public const int UserProfile_Deleted = MessageConstants.StatusCodes.SysUser_Codes.UserProfile_Deleted;

            public const int Login_NotFound = MessageConstants.StatusCodes.SysLogin_Codes.Login_NotFound;
            public const int Login_Disabled = MessageConstants.StatusCodes.SysLogin_Codes.Login_Disabled;
            public const int Login_Deleted = MessageConstants.StatusCodes.SysLogin_Codes.Login_Deleted;
            public const int Password_NotMatch = MessageConstants.StatusCodes.SysLogin_Codes.Password_NotMatch;
            public const int RefreshToken_Update_Failed = MessageConstants.StatusCodes.SysLogin_Codes.RefreshToken_Update_Failed;
            public const int Token_Generation_Failed = MessageConstants.StatusCodes.SysToken_Codes.Token_Generation_Failed;
        }

        public static class StatusTexts
        {
            public const string Invalid_Request = MessageConstants.StatusTexts.Invalid_Request;
            public const string Unknown_Error = MessageConstants.StatusTexts.Unknown_Error;
            public const string Configuration_Error = MessageConstants.StatusTexts.Configuration_Error;
            public const string Login_Failed = "Login failed";
        }

        public string LoginName { get; set; }
        public string UserName { get; set; }
        public SysUser_OutputPayload User { get; set; }
        public SysToken_OutputPayload Token { get; set; }
    }
}