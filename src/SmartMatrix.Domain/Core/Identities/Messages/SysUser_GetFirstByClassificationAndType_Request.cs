namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByClassificationAndType_Request
    {
        public string PartitionKey { get; set; }
        public string Classification { get; set; }        
        public string Type { get; set; }
    }
}