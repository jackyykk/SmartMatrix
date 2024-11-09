using SmartMatrix.Core.BaseClasses.Common;

namespace SmartMatrix.DataAccess.AuditLogging
{
    public class Audit : IEntity<long>
    {
        public long Id { get; set; }
        public string? UserName { get; set; }
        public string? Type { get; set; }
        public string? TableName { get; set; }
        public DateTime ActionTime { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? AffectedColumns { get; set; }
        public string? PrimaryKey { get; set; }
    }
}