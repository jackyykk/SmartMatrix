namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByUserName_Request
    {
        public string PartitionKey { get; set; }
        public string UserName { get; set; }
    }
}