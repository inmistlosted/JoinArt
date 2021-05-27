using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer
{
    public interface ICommandAdapter
    {
        Task<IDataReader> ExecuteReaderAsync(DbCommand command);
    }
}
