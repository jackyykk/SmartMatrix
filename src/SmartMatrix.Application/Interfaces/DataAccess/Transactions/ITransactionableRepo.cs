using System.Data;

namespace SmartMatrix.Domain.Interfaces.DataAccess.Transactions
{
    public interface ITransactionableRepo: IDisposable
    {
        void SetTransaction(IDbTransaction transaction);        
    }
}