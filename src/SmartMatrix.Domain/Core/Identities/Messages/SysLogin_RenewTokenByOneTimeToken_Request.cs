namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_RenewTokenByOneTimeToken_Request
    {
        public string PartitionKey { get; set; }
        public string OneTimeToken { get; set; }
    }
}