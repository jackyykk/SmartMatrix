using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SmartMatrix.Application.Interfaces.DataAccess.DbConnections;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories;
using SmartMatrix.Application.Interfaces.DataAccess.Repositories.Core.Identities;
using SmartMatrix.Domain.Core.Identities.DbEntities;

namespace SmartMatrix.DataAccess.Repositories.Core.Identities
{
    public class SysUserRepo : ISysUserRepo
    {
        private IDbTransaction? _transaction;
        private readonly IDistributedCache _cache;
        private readonly ICoreReadDbConnection _readDbConnection;
        private readonly ICoreWriteDbConnection _writeDbConnection;
        private readonly ICoreReadRepo<SysUser, int> _readRepo;
        private readonly ICoreWriteRepo<SysUser, int> _writeRepo;
        
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

        public SysUserRepo(IDistributedCache cache, ICoreReadDbConnection readDbConnection, ICoreWriteDbConnection writeDbConnection, ICoreReadRepo<SysUser, int> readRepo, ICoreWriteRepo<SysUser, int> writeRepo)
        {
            _cache = cache;            
            _readDbConnection = readDbConnection;
            _writeDbConnection = writeDbConnection;
            _readRepo = readRepo;
            _writeRepo = writeRepo;
        }

        public IQueryable<SysUser> SysUsers => _readRepo.Entities;

        public async Task<SysUser?> GetFirstByUserNameAsync(string partitionKey, string userName)
        {
            // Get SysUser Copy To Avoid Cyclic References
            // Get Latest Un-Deleted User and Un-Deleted Logins
            var user = await _readRepo.Entities
                .Where(x => !x.IsDeleted)
                .Where(x => x.PartitionKey == partitionKey)
                .Where(x => x.UserName == userName)
                .OrderByDescending (x => x.Id)
                .Select(x => SysUser.Copy(x, x.Logins.Where(l => !l.IsDeleted).ToList(), x.Roles.Where(r => !r.IsDeleted).ToList()))
                .FirstOrDefaultAsync();
            
            return user;
        }

        public async Task<SysUser?> GetFirstByLoginNameAsync(string partitionKey,string loginName)
        {
            // Get Un-Deleted Login
            var login = _readRepo.Entities
                .SelectMany(x => x.Logins)
                .Where(x => !x.IsDeleted)
                .Where(x => x.PartitionKey == partitionKey)
                .Where(x => x.LoginName == loginName)
                .OrderByDescending (x => x.Id)
                .FirstOrDefault();

            if (login == null)
                return null;

            // Get SysUser Copy To Avoid Cyclic References
            var user = await _readRepo.Entities
                .Where(x => !x.IsDeleted &&  x.Id == login.SysUserId)                
                .Select(x => SysUser.Copy(x, x.Logins.Where(l => !l.IsDeleted).ToList(), x.Roles.Where(r => !r.IsDeleted).ToList()))
                .FirstOrDefaultAsync();
            
            return user;
        }
    }
}