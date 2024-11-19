namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_Login_Request
    {
        public string PartitionKey { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
    }
}