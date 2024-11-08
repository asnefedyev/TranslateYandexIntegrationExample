using Microsoft.Extensions.ObjectPool;
using Npgsql;

namespace TranslatorExample.Services.Common.Implements.SqlDbConnectionFactories
{
    public class NpgsqlConnectionPoolPolicy : IPooledObjectPolicy<NpgsqlConnection>
    {
        private readonly string _connectionString;

        public NpgsqlConnectionPoolPolicy(IConfiguration configuration, string connectionAlias)
        {
            _connectionString = configuration.GetConnectionString(connectionAlias);
        }

        public NpgsqlConnection Create()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public bool Return(NpgsqlConnection obj)
        {
            return true;
        }
    }
}