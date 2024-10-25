namespace SmartMatrix.Core.BaseClasses.Common
{
    public interface IEntity
    {
    }

    public interface IEntity<TId> : IEntity
    {
        TId Id { get; set; }
    }
}