using System;

namespace SmartMatrix.Core.Abstractions.Domain
{
    public interface IAuditableEntity<TId> : IAuditableEntity, IEntity<TId>
    {
    }

    public interface IAuditableEntity : IEntity
    {        
        string CreatedBy { get; set; }
        DateTime? CreatedAt { get; set; }
        string ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
        bool IsDeleted { get; set; }
        string DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
        bool SkipAudit { get; }
    }
}