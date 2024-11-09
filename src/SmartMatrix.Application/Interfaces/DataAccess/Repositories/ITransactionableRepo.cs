using System.Data;

namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories
{
    public interface ITransactionableRepo
    {
        void SetTransaction(IDbTransaction transaction);        
    }
}