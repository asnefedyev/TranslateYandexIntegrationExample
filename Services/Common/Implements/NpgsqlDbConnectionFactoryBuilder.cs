using Microsoft.Extensions.ObjectPool;
using TranslatorExample.Services.Common.Contracts;

namespace TranslatorExample.Services.Common.Implements
{
    public class NpgsqlDbConnectionFactoryBuilder : DbConnectionFactoryBuilder
    {
        private readonly IConfiguration _configuration;
        private readonly ObjectPoolProvider _poolProvider;

        public NpgsqlDbConnectionFactoryBuilder(IConfiguration configuration, ObjectPoolProvider poolProvider)
        {
            _configuration = configuration;
            _poolProvider = poolProvider;
        }

        public IDbConnectionFactory GetConnectionFactory(string alias)
        {
            return new NpsqlDbConnectionFactory(_configuration, _poolProvider, alias);
        }
    }
}