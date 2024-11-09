using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Repositories.Demos.SimpleNotes
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}