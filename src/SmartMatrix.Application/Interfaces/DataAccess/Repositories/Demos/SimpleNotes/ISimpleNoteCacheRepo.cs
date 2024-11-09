using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}