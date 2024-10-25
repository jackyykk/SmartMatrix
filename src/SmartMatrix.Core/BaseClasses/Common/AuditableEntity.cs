using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
        public TId Id { get; set; } = default!;
        public string Status { get; set; } = default!;
        public string CreatedBy { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public string ModifiedBy { get; set; } = default!;
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; } = default!;
        public DateTime? DeletedAt { get; set; }
        public bool SkipAudit { get; }
    }
}