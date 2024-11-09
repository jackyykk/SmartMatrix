using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {
        public string? Status { get; set; }
        string? CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
        string? ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
        bool IsDeleted { get; set; }
        string? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
        bool SkipAudit { get; }
    }
}