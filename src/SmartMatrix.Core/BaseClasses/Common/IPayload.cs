namespace SmartMatrix.Core.BaseClasses.Common
{
    public interface IPayload
    {
    }

    public interface IPayload<TId> : IPayload
    {
        TId Id { get; set; }
    }
}