using Microsoft.Data.SqlClient;
using Npgsql;

namespace TranslatorExample.Services.Common.Contracts
{
    public interface IDbConnectionFactory
    {
        NpgsqlConnection CreateConnection();
    }
}
