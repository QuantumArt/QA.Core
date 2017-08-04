using System.Collections.Concurrent;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace QA.Core.Data.Repository
{
    /// <summary>
    /// Менеджер подключений к БД
    /// </summary>
    public class ConnectionManager
    {
        private readonly ConcurrentDictionary<string, IDbConnection> _connections = new ConcurrentDictionary<string, IDbConnection>();

        /// <summary>
        /// Возвращает подключение к БД по имени
        /// </summary>
        /// <param name="connectionString">Строка подключения или название строки подключения в .config</param>
        /// <returns></returns>
        public IDbConnection GetConnection(string connectionString)
        {
            Throws.IfArgumentNullOrEmpty(connectionString, _ => connectionString);

            return _connections.GetOrAdd(
                connectionString, connStr =>
                {
                    var connectionStringFromConfig = ConfigurationManager.ConnectionStrings[connectionString];
                    if (connectionStringFromConfig != null)
                    {
                        connectionString = connectionStringFromConfig.ConnectionString;
                    }

                    var conn = new SqlConnection(connectionString);
                    return conn;
                });
        }
    }
}
