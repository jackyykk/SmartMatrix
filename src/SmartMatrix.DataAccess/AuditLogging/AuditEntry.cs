using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SmartMatrix.DataAccess.AuditLogging
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public string? UserName { get; set; }
        public string? TableName { get; set; }
        public DateTime ActionTime { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> AffectedColumns { get; } = new List<string>();
        public Audit ToAudit()
        {
            var audit = new Audit()
            {
                UserName = UserName,
                Type = AuditType.ToString(),
                TableName = TableName,
                ActionTime = ActionTime,
                PrimaryKey = JsonSerializer.Serialize(KeyValues),
                OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
                NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
                AffectedColumns = AffectedColumns.Count == 0 ? null : JsonSerializer.Serialize(AffectedColumns)
            };
            return audit;
        }
    }
}