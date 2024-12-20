namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetFirstByClassification_Request
    {
        public string PartitionKey { get; set; }
        public string Classification { get; set; }        
    }
}