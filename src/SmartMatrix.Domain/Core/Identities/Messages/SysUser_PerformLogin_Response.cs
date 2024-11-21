using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_PerformLogin_Response
    {
        public static class StatusCodes
        {
            public const int Success = 0;            
            public const int UnknownError = -999;
            
            // Other errors
            public const int InvalidRequest = -101;
            public const int UserNotFound = -201;
            public const int LoginNotFound = -301;
            public const int LoginDisabled = -302;
            public const int LoginDeleted = -303;
            public const int InvalidPassword = -401;
            public const int UpdateTokenFailed = -501;
        }

        public SysUser User { get; set; }
        public SysToken Token { get; set; }
    }
}