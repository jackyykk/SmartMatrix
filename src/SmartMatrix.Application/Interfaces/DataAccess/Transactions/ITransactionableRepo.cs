using System.Data;

namespace SmartMatrix.Application.Interfaces.DataAccess.Transactions
{
    public interface ITransactionableRepo: IDisposable
    {
        void SetTransaction(IDbTransaction transaction);        
    }
}