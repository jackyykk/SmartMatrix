namespace SmartMatrix.Domain.Constants
{

    public static class MessageConstants
    {        
        public static class StatusCodes
        {
            public const int Success = 0;

            // All errors are negative numbers
            public const int Unknown_Error = -99999;
            public const int Invalid_Request = -1;
            public const int Configuration_Error = -2;
            
            public static class SysUser_Codes
            {
                // Status
                public const int User_NotFound = -101;                
                public const int User_Disabled = -102;
                public const int User_Deleted = -103;

                public const int UserProfile_NotFound = -106;
                public const int UserProfile_Disabled = -107;
                public const int UserProfile_Deleted = -108;

                // Field Errors
                public const int UserName_Empty = -111;
                public const int UserName_Already_Existed = -112;

                // Action Errors                
                public const int User_Insert_Failed = -151;                
            }

            public static class SysLogin_Codes
            {
                // Status
                public const int Login_NotFound = -201;
                public const int Login_Disabled = -202;
                public const int Login_Deleted = -203;

                // Field Errors
                public const int LoginName_Empty = -211;
                public const int LoginName_Already_Existed = -212;
                
                // Action Errors            
                public const int Password_NotMatch = -251;
                public const int RefreshToken_Expired = -252;
                public const int RefreshToken_Update_Failed = -253;
                public const int OneTimeToken_Expired = -254;
                public const int OneTimeToken_Update_Failed = -255;
                public const int Tokens_Update_Failed = -256;
            }

            public static class SysToken_Codes
            {
                // Status
                public const int Token_Generation_Failed = -301;                
            }
        }

        public static class StatusTexts
        {            
            public const string Invalid_Request = "The request is invalid.";
            public const string Unknown_Error = "An unknown error has occurred.";
            public const string Configuration_Error = "Configuration error";
        }        
    }
}