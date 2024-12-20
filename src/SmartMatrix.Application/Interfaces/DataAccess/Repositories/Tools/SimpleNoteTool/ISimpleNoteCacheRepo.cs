using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Tools.SimpleNoteTool
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}