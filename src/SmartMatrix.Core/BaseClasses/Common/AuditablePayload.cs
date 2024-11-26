using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public abstract class AuditablePayload<TId> : IAuditablePayload<TId>
    {
        public TId Id { get; set; } = default!;
        public Guid Guid { get; set; }  // Globally unique identifier
        public bool IsDeleted { get; set; }
        public string? Status { get; set; }        
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }        
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }        
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string? InternalRemark { get; set; }
        
        [System.Text.Json.Serialization.JsonIgnore]
        public bool SkipAudit { get; } = true;
    }
}