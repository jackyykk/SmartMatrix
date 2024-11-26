using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.DataAccess.AuditLogging
{
    public class Audit : BaseEntity<long>
    {        
        public string? UserName { get; set; }
        public string? Type { get; set; }
        public DateTime ActionTime { get; set; }
        public string? TableName { get; set; }
        public string? PrimaryKey { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }        
    }
}