using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.DataAccess.Repositories.Core.Identities
{
    public class SysLoginRepo : ISysLoginRepo
    {
        private IDbTransaction? _transaction;
        private readonly IDistributedCache _cache;
        private readonly ICoreReadDbConnection _readDbConnection;
        private readonly ICoreWriteDbConnection _writeDbConnection;
        private readonly ICoreReadRepo<SysLogin, int> _readRepo;
        private readonly ICoreWriteRepo<SysLogin, int> _writeRepo;

        public void SetConnection(string connectionString)
        {            
            _readDbConnection.SetConnection(connectionString);
            _writeDbConnection.SetConnection(connectionString);
            _readRepo.SetConnection(connectionString);
            _writeRepo.SetConnection(connectionString);
        }

        public void SetTransaction(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public SysLoginRepo(IDistributedCache cache, ICoreReadDbConnection readDbConnection, ICoreWriteDbConnection writeDbConnection, ICoreReadRepo<SysLogin, int> readRepo, ICoreWriteRepo<SysLogin, int> writeRepo)
        {
            _cache = cache;
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public IQueryable<SysLogin> SysLogins => _readRepo.Entities;

        public async Task<List<SysLogin>> GetListAsync(string partitionKey)
        {
            var logins = await _readRepo.Entities
                .Where(x => x.PartitionKey == partitionKey)
                .ToListAsync();

            return logins;
        }

        public async Task<SysLogin?> GetFirstByRefreshTokenAsync(string partitionKey, string refreshToken)
        {
            var login = await _readRepo.Entities
                .Where(x => x.PartitionKey == partitionKey && x.RefreshToken == refreshToken)
                .FirstOrDefaultAsync();

            return login;
        }

        public async Task UpdateSecretAsync(SysLogin entity)
        {
            var entityToUpdate = await _writeRepo.GetByIdAsync(entity.Id);
            if (entityToUpdate == null)
            {
                throw new Exception("Login not found");
            }

            entityToUpdate.Password = entity.Password;
            entityToUpdate.PasswordHash = entity.PasswordHash;
            entityToUpdate.PasswordSalt = entity.PasswordSalt;
        }

        public async Task UpdateRefreshTokenAsync(SysLogin entity)
        {
            var entityToUpdate = await _writeRepo.GetByIdAsync(entity.Id);
            if (entityToUpdate == null)
            {
                throw new Exception("Login not found");
            }

            entityToUpdate.RefreshToken = entity.RefreshToken;
            entityToUpdate.RefreshTokenExpires = entity.RefreshTokenExpires;
        }

        public async Task<SysLogin> InsertAsync(SysLogin entity)
        {
            var entityToInsert = SysLogin.CopyAsNew(entity);
            var insertedEntity = await _writeRepo.InsertAsync(entityToInsert);
            return insertedEntity;
        }
    }
}