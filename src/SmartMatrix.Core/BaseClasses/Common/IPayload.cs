using System;

namespace SmartMatrix.Core.BaseClasses.Common
{
    public interface IPayload
    {
    }

    public interface IPayload<TId> : IPayload
    {
        TId Id { get; set; }
        Guid? Guid { get; set; }  // Globally unique identifier
    }
}