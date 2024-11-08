namespace TranslatorExample.Services.Common.Contracts
{
    public interface DbConnectionFactoryBuilder
    {
        public IDbConnectionFactory GetConnectionFactory(string alias);
    }
}