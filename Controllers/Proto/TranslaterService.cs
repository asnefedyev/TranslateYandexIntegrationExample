namespace TranslateExample.Controllers.Proto
{
    using ExampleSpace;
    using Grpc.Core;
    using System.Threading.Tasks;
    using TranslatorExample.Services.CustomBackend.Contracts;

    public class TranslaterService : Translater.TranslaterBase
    {
        private readonly IExternalTranslateService _translateServices;
        public TranslaterService(IExternalTranslateService translatorService)
        {
            _translateServices = translatorService;
        }

        public async override Task<TranslateResponse> DoTranslate(TranslateRequest request, ServerCallContext context)
        {
            var result = await _translateServices.DoTranslateCacheAsync(request.SourceLang, request.TargetLang, request.TranslateText);
            return new TranslateResponse { TranslateResult = result, SourceHash = "1", TargetHash = "2" };
        }
    }
}