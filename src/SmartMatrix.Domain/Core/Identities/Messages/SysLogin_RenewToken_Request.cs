namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_RenewToken_Request
    {
        public string PartitionKey { get; set; }
        public string RefreshToken { get; set; }
    }
}