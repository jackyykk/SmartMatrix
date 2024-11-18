namespace SmartMatrix.Domain.Constants
{
    public static class CommonConstants
    {
        public static class DbEntityStatus
        {
            public const string Active = "Active";  // This is the default status
            public const string Disabled = "Disabled";  // This is the status when the entity is disabled by the user
            public const string Deleted = "Deleted";  // This is the status when the entity is deleted by the user
            public const string Pending = "Pending";  // In case the process is pending, the entity status will be pending
            public const string Processing = "Processing";  // In case the process is processing, the entity status will be processing
            public const string Inactive = "Inactive";  // In case the process has failed, the entity status will become inactive
        }

        public static class DbProcessStatus
        {
            public const string Pending = "Pending";  // This is the default status of the process
            public const string Processing = "Processing";  // This is the status when the process is processing
            public const string Succeeded = "Succeeded";  // This is the status when the process has succeeded
            public const string Failed = "Failed";  // This is the status when the process has failed
        }
    }
}