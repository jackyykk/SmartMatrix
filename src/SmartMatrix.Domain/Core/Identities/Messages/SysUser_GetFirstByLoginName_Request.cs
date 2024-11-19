namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByLoginName_Request
    {
        public string PartitionKey { get; set; }
        public string LoginName { get; set; }
    }
}