using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public abstract class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; set; } = default!;        
    }
}