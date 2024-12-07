using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo
{
    public interface ISimpleNoteCacheRepo
    {
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}