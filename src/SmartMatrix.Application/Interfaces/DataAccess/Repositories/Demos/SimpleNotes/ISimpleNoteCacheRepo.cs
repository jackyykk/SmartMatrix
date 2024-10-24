using SmartMatrix.Domain.Entities.Demos;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}