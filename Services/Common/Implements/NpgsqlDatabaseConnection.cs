using System.Data;
using TranslatorExample.Services.Common.Contracts;
using TranslatorExample.Services.Common.DbConnections;

namespace TranslatorExample.Services.Common.Implements
{
    public class NpgsqlDatabaseConnection : IDatabaseConnection
    {
        private readonly DbConnectionFactoryBuilder _builderFactory;

        public NpgsqlDatabaseConnection(DbConnectionFactoryBuilder builderFactory)
        {
            _builderFactory = builderFactory;
        }

        public IDbConnection GetDbConnection(string alias)
        {
            return _builderFactory.GetConnectionFactory(alias).CreateConnection();
        }

        public IDbConnection GetDbConnection()
        {
            return _builderFactory
                .GetConnectionFactory(new DefaultConnection().ToString())
                .CreateConnection();
        }
    }
}