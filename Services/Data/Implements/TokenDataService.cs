using Dapper;
using System.Transactions;
using TranslateExample.Services.Data.Contracts;
using TranslatorExample.Services.Common.Contracts;

namespace TranslateExample.Services.Data.Implements
{
    public class TokenDataService : ITokenDataService
    {
        private const string QueryInsert = "insert into public.token select @Token";
        private const string QueryGet = "select token_value from public.token";
        private const string QueryDelete = "delete from public.token";

        private const string YandexPassportOauthToken = "yandexPassportOauthToken";

        private readonly IConfiguration _configuration;
        private readonly IDatabaseConnection _connectionService;
        
        public TokenDataService(IConfiguration configuration, IHttpClientFactory httpClientFactory, IDatabaseConnection connService)
        {
            _configuration = configuration;
            _connectionService = connService;
        }

        public void AddTokenToDb(string token)
        {
            using (var conn = _connectionService.GetDbConnection())
            {
                using (var transactionScope = new TransactionScope())
                {
                    conn.Execute(QueryDelete);
                    conn.Execute(QueryInsert, new { Token = token });
                    transactionScope.Complete();
                }
            }
        }

        public string? GetTokenFromDb()
        {
            using (var conn = _connectionService.GetDbConnection())
            {
                var token = conn.ExecuteScalar(QueryGet);
                if (token is not null)
                {
                    return token.ToString();
                }
                else return null;
            }
        }
    }
}