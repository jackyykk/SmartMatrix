using SmartMatrix.Domain.Apps.SimpleNoteApp.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Apps.SimpleNoteApp
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}