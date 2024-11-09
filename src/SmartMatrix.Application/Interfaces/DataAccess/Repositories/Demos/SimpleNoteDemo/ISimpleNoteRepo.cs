using SmartMatrix.Domain.Demos.SimpleNoteDemo.Entities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo
{
    public interface ISimpleNoteRepo
    {
        IQueryable<SimpleNote> SimpleNotes { get; }        
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
        Task<int> InsertAsync(SimpleNote entity);
    }
}