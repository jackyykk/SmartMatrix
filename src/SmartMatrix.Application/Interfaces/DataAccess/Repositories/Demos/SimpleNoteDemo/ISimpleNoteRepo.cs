using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo
{
    public interface ISimpleNoteRepo : IDbConnectionChangeable, ITransactionableRepo
    {
        IQueryable<SimpleNote> SimpleNotes { get; }
        Task<SimpleNote?> GetByIdAsync(int id);
        Task<List<SimpleNote>> GetListAsync(string owner);
        Task<SimpleNote> InsertAsync(SimpleNote entity);
    }
}