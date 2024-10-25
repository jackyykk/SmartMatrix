using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Demos.SimpleNotes;
using SmartMatrix.Domain.Entities.Demos;

namespace SmartMatrix.DataAccess.Repositories.Demos.SimpleNotes
{
    public class SimpleNoteRepo : ISimpleNoteRepo
    {
        private IDbTransaction? _transaction;
        private readonly IDistributedCache _cache;
        private readonly IDemoRepo<SimpleNote, int> _repo;
        private readonly IDemoReadDbConnection _readDbConnection;
        private readonly IDemoWriteDbConnection _writeDbConnection;

        public SimpleNoteRepo(IDistributedCache cache, IDemoRepo<SimpleNote, int> repo, IDemoReadDbConnection readDbConnection, IDemoWriteDbConnection writeDbConnection)
        {
            _cache = cache;
            _repo = repo;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
        }

        public IQueryable<SimpleNote> SimpleNotes => _repo.Entities;

        public async Task<SimpleNote?> GetByIdAsync(int id)
        {            
            return await _repo.Entities.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<SimpleNote>> GetListAsync(string owner)
        {
            return await _repo.Entities.Where(p => p.Owner == owner).ToListAsync();
        }
    }
}