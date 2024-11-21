namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_GetFirstByRefreshToken_Request
    {
        public string PartitionKey { get; set; }
        public string RefreshToken { get; set; }
    }
}