using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    // It works as Data Transfer Object (DTO) between the Domain and Application layers
    public abstract class BasePayload<TId> : IPayload<TId>
    {
        public TId Id { get; set; } = default!;
        public Guid Guid { get; set; }  // Globally unique identifier
    }
}