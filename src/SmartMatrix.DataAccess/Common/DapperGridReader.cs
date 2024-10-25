using SmartMatrix.Application.Interfaces.DataAccess.Common;
using static Dapper.SqlMapper;

namespace SmartMatrix.DataAccess.Common
{
    public class DapperGridReader : IGridReader
    {
        private readonly GridReader _gridReader;

        public DapperGridReader(GridReader gridReader)
        {
            _gridReader = gridReader;
        }

        public IEnumerable<T> Read<T>()
        {
            return _gridReader.Read<T>();
        }

        public void Dispose()
        {
            _gridReader.Dispose();
        }        
    }
}