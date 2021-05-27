using diplom.api.Settings;
using Npgsql;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace diplom.api.DataAccessLayer.Implementation
{
    public class PgSqlCommandAdapter : ICommandAdapter
    {
        private readonly DataAccessSettings _dataAccessSettings;

        public PgSqlCommandAdapter(DataAccessSettings dataAccessSettings)
        {
            this._dataAccessSettings = dataAccessSettings ?? throw new ArgumentNullException(nameof(dataAccessSettings));
        }

        public async Task<IDataReader> ExecuteReaderAsync(DbCommand sqlCommand)
        {
            if (sqlCommand == null)
            {
                throw new ArgumentNullException();
            }

            NpgsqlConnection sqlConnection = await GetConnectionAsync();

            try
            {
                sqlCommand.Connection = sqlConnection;
                return await sqlCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch
            {
                sqlConnection.Close();
                throw;
            }
        }

        private async Task<NpgsqlConnection> GetConnectionAsync()
        {
            NpgsqlConnection conn = new NpgsqlConnection(this._dataAccessSettings.ConnectionString);

            await conn.OpenAsync();

            return conn;
        }
    }
}
