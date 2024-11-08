using Example.Contracts.WebClientContracts;
using Microsoft.Extensions.Caching.Memory;
using Refit;
using TranslateExample.Services.AppServices.Contracts;
using TranslatorExample.Services.Common.Contracts;
using TranslateExample.Services.Data.Contracts;

namespace TranslateExample.Services.AppServices.Implements
{
    public class TokenService : ITokenService
    {
        private const string YandexPassportOauthToken = "yandexPassportOauthToken";
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IDatabaseConnection _connService;
        private readonly ITokenDataService _tokenDataService;

        public TokenService(IConfiguration configuration, IHttpClientFactory httpClientFactory, ITokenDataService tokenDataService) 
        {
            _httpClient = httpClientFactory.CreateClient("YandexClient_KeyApi");
            _configuration = configuration;
            _tokenDataService = tokenDataService;
        }

        public async Task UpdateToken()
        {
            var token = _configuration.GetValue<string>(YandexPassportOauthToken);
            if (!string.IsNullOrEmpty(token))
            {
                var service = RestService.For<IYandexRefitContract>(_httpClient);
                var result = await service.DoYandexGetToken(new Models.RequestModels.KeyApiRequest
                {
                    Token = token
                });
                _tokenDataService.AddTokenToDb(result.IamToken);
            }
            else throw new NullReferenceException("Токен не получен из конфигурации!");
        }
    }
}