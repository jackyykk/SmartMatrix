namespace SmartMatrix.Application.Interfaces.DataAccess.Repositories
{
    public interface IDbConnectionChangeable
    {
        void SetConnection(string connectionString);
    }
}