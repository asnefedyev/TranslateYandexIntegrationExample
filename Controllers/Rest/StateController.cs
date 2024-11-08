using Microsoft.AspNetCore.Mvc;
using TranslateExample.Models.DTO;
using TranslateExample.Services.AppServices.Contracts;

namespace TranslateExample.Controllers.Rest
{
    [Route("base")]
    [ApiController()]
    public class ExampleController : Controller
    {
        private readonly ICacheUpdateService _cacheUpdateService;
        public ExampleController(ICacheUpdateService cacheService)
        {
            _cacheUpdateService = cacheService;
        }

        [HttpGet("state")]
        public async Task<CacheStateDTO> GetState()
        {
            return await Task.Run(_cacheUpdateService.GetCacheState);
        }
    }
}