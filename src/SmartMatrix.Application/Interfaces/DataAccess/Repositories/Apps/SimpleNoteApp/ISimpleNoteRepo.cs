using SmartMatrix.Domain.Apps.SimpleNoteApp.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Apps.SimpleNoteApp
{
    public interface ISimpleNoteRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SimpleNote> SimpleNotes { get; }
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
        Task<SimpleNote> InsertAsync(SimpleNote entity);
    }
}