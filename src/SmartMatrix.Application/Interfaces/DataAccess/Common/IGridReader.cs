namespace SmartMatrix.Application.Interfaces.DataAccess.Common
{
    public interface IGridReader : IDisposable
    {
        Task<IEnumerable<T>> ReadAsync<T>();
        Task<bool> IsConsumedAsync();
    }
}