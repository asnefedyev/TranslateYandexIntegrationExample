using Microsoft.Extensions.ObjectPool;
using Microsoft.Data.SqlClient;
using TranslatorExample.Services.Common.Contracts;
using TranslatorExample.Services.Common.Implements.SqlDbConnectionFactories;
using Npgsql;

namespace TranslatorExample.Services.Common.Implements
{
    public class NpsqlDbConnectionFactory : IDbConnectionFactory
    {
        private readonly ObjectPool<NpgsqlConnection> _connectionPool;

        public NpsqlDbConnectionFactory(IConfiguration configuration, ObjectPoolProvider poolProvider, string connectionAlias)
        {
            _connectionPool = poolProvider.Create(new NpgsqlConnectionPoolPolicy(configuration, connectionAlias));
        }

        public NpgsqlConnection CreateConnection()
        {
            return _connectionPool.Get();
        }
    }
}
