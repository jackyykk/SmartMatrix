namespace SmartMatrix.Domain.Constants
{
    public static class CommonConstants
    {
        public static class PartitionKeys
        {
            public const string SmartMatrix = "smartmatrix";
        }

        public static class Identities
        {
            public static class SysPolicies
            {
                public const string Admin = "Admin";
                public const string User = "User";
            }

            public static class SysClaimTypes
            {
                public const string LoginProviderName = "login_provider_name";
                public const string LoginNameIdentifier = "login_name_identifier";
                public const string ApiPermission = "api_permission";
            }
        }
        
        public static class DbEntityStatus
        {
            public const string Active = "active";  // This is the default status
            public const string Disabled = "disabled";  // This is the status when the entity is disabled by the user
            public const string Deleted = "deleted";  // This is the status when the entity is deleted by the user
            public const string Pending = "pending";  // In case the process is pending, the entity status will be pending
            public const string Processing = "processing";  // In case the process is processing, the entity status will be processing
            public const string Inactive = "inactive";  // In case the process has failed, the entity status will become inactive
        }

        public static class DbProcessStatus
        {
            public const string Pending = "pending";  // This is the default status of the process
            public const string Processing = "processing";  // This is the status when the process is processing
            public const string Succeeded = "succeeded";  // This is the status when the process has succeeded
            public const string Failed = "failed";  // This is the status when the process has failed
        }
    }
}