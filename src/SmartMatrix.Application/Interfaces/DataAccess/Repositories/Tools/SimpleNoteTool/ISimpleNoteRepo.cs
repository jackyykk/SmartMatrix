using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Tools.SimpleNoteTool
{
    public interface ISimpleNoteRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SimpleNote> SimpleNotes { get; }
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
        Task<SimpleNote> InsertAsync(SimpleNote entity);
    }
}