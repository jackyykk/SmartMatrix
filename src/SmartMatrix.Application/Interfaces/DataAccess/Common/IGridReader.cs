namespace SmartMatrix.Domain.Interfaces.DataAccess.Common
{
    public interface IGridReader : IDisposable
    {
        IEnumerable<T> Read<T>();        
    }
}