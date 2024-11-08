using Example.Models.RequestModels;
using Example.Models.ResponseModels;
using Refit;
using TranslateExample.Models.RequestModels;
using TranslateExample.Models.ResponseModels;

namespace Example.Contracts.WebClientContracts
{
    public interface IYandexRefitContract
    {
        [Post("/translate/v2/translate")]
        Task<TranslationResponse> DoYandexTranslate([Body] TranslationRequest request);

        [Post("/iam/v1/tokens")]
        Task<TokenInfoResponse> DoYandexGetToken([Body] KeyApiRequest request);
    }
}