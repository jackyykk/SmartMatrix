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

        public async Task<SysUser?> GetFirstByUserNameAsync(string userName)
        {
            // Get SysUser Copy To Avoid Cyclic References
            var user = await _readRepo.Entities.Where(u => u.UserName == userName)
                .Include(u => u.Logins)                
                .Select(u => SysUser.Copy(u))
                .FirstOrDefaultAsync();
            
            return user;
        }        
    }
}