namespace SmartMatrix.Application.Interfaces.DataAccess.Common
{
    public interface IGridReader : IDisposable
    {
        IEnumerable<T> Read<T>();        
    }
}