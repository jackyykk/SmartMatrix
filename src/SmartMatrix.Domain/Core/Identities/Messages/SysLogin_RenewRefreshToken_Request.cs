namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysLogin_RenewRefreshToken_Request
    {
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }
}