using System.Data;

namespace TranslatorExample.Services.Common.Contracts
{
    public interface IDatabaseConnection
    {
        IDbConnection GetDbConnection();
        IDbConnection GetDbConnection(string alias);

        public string ToString()
        {
            return "some connection settings";
        }
    }
}