using SmartMatrix.Domain.Demos.SimpleNotes.Entities;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Repositories.Demos.SimpleNotes
{
    public interface ISimpleNoteRepo
    {
        IQueryable<SimpleNote> SimpleNotes { get; }        
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
    }
}