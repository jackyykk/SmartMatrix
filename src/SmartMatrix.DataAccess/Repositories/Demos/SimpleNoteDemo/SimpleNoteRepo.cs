using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNoteDemo;
using SmartMatrix.Domain.Demos.SimpleNoteDemo.DbEntities;

namespace SmartMatrix.DataAccess.Repositories.Demos.SimpleNoteDemo
{
    public class SimpleNoteRepo : ISimpleNoteRepo
    {
        private IDbTransaction? _transaction;
        private readonly IDistributedCache _cache;
        private readonly IDemoReadDbConnection _readDbConnection;
        private readonly IDemoWriteDbConnection _writeDbConnection;
        private readonly IDemoReadRepo<SimpleNote, int> _readRepo;
        private readonly IDemoWriteRepo<SimpleNote, int> _writeRepo;
        
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

        public SimpleNoteRepo(IDistributedCache cache, IDemoReadDbConnection readDbConnection, IDemoWriteDbConnection writeDbConnection, IDemoReadRepo<SimpleNote, int> readRepo, IDemoWriteRepo<SimpleNote, int> writeRepo)
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