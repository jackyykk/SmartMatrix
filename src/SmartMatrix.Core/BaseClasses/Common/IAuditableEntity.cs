using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {
        public string? Status { get; set; }
        DateTime? CreatedAt { get; set; }
        string? CreatedBy { get; set; }        
        DateTime? ModifiedAt { get; set; }
        string? ModifiedBy { get; set; }        
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
        string? DeletedBy { get; set; }        
        bool SkipAudit { get; }
    }
}