using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Tools.SimpleNoteTool;
using SmartMatrix.Domain.Tools.SimpleNoteTool.DbEntities;

namespace SmartMatrix.DataAccess.Repositories.Tools.SimpleNoteTool
{
    public class SimpleNoteRepo : ISimpleNoteRepo
    {
        private IDbTransaction? _transaction;
        private readonly IDistributedCache _cache;
        private readonly IToolReadDbConnection _readDbConnection;
        private readonly IToolWriteDbConnection _writeDbConnection;
        private readonly IToolReadRepo<SimpleNote, int> _readRepo;
        private readonly IToolWriteRepo<SimpleNote, int> _writeRepo;
        
        public void SetConnection(string connectionString)
        {
            _readRepo.SetConnection(connectionString);
            _writeRepo.SetConnection(connectionString);
            _readDbConnection.SetConnection(connectionString);
            _writeDbConnection.SetConnection(connectionString);
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public SimpleNoteRepo(IDistributedCache cache, IToolReadDbConnection readDbConnection, IToolWriteDbConnection writeDbConnection, IToolReadRepo<SimpleNote, int> readRepo, IToolWriteRepo<SimpleNote, int> writeRepo)
        {
            _cache = cache;            
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public IQueryable<SimpleNote> SimpleNotes => _readRepo.Entities;

        public async Task<SimpleNote?> GetByIdAsync(int id)
        {            
            return await _readRepo.Entities.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SimpleNote>> GetListAsync(string owner)
        {
            return await _readRepo.Entities.Where(x => x.Owner == owner).ToListAsync();
        }

        public async Task<SimpleNote> InsertAsync(SimpleNote entity)
        {
            return await _writeRepo.InsertAsync(entity);            
        }
    }
}