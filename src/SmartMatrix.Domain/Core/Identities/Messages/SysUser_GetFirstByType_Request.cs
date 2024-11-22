namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByType_Request
    {
        public string PartitionKey { get; set; }
        public string Type { get; set; }        
    }
}