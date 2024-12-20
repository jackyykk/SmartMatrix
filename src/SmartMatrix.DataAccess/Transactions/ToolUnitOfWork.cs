using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using SmartMatrix.Application.Interfaces.DataAccess.DbContexts;
using SmartMatrix.Application.Interfaces.DataAccess.Transactions;
using SmartMatrix.Application.Interfaces.Services.Essential;

namespace SmartMatrix.DataAccess.Transactions
{
    public class ToolUnitOfWork : IToolUnitOfWork
    {
        private readonly IAuthenticatedUserService _userSvc;
        private readonly IToolWriteDbContext _writeDbContext;
        private bool disposed;

        public IToolWriteDbContext WriteDbContext => _writeDbContext;

        public ToolUnitOfWork(IToolWriteDbContext dbContext, IAuthenticatedUserService userSvc)
        {
            _writeDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userSvc = userSvc;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _writeDbContext.SaveChangesAsync(cancellationToken);
        }

        public void Open()
        {
            _writeDbContext.Connection.Open();
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _writeDbContext.Connection.BeginTransaction();
            _writeDbContext.Database.UseTransaction(transaction as DbTransaction);

            return transaction;
        }

        public void Close()
        {
            _writeDbContext.Connection.Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _writeDbContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
    }
}