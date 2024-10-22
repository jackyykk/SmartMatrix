namespace SmartMatrix.Core.Abstractions.Domain
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
    }
}