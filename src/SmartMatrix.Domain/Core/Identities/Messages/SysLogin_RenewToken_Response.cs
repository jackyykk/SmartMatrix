namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_RenewToken_Response : SysToken
    {
        public static class StatusCodes
        {
            public const int Success = 0;            
            public const int UnknownError = -999;

            // Other errors            
            public const int InvalidRequest = -101;
            public const int LoginNotFound = -201;
            public const int EmptyLoginName = -202;
            public const int RefreshTokenExpired = -203;
            public const int LoginDisabled = -204;
            public const int LoginDeleted = -205;
        }
    }
}