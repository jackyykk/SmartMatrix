namespace SmartMatrix.Domain.Core.Identities.Messages
{
    public class SysUser_GetById_Request
    {
        public string PartitionKey { get; set; }
        public int Id { get; set; }
    }
}