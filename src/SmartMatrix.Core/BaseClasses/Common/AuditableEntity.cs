using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public abstract class AuditableEntity<TId> : IAuditableEntity<TId>
    {
        public TId Id { get; set; } = default!;
        public bool IsDeleted { get; set; }
        public string? Status { get; set; }        
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }        
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }        
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public string? InternalRemark { get; set; }
        public bool SkipAudit { get; } = true;
    }
}