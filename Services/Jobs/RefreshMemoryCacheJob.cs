using Quartz;
using TranslateExample.Services.AppServices.Contracts;

namespace TranslateExample.Services.Jobs
{
    public class RefreshMemoryCacheJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public RefreshMemoryCacheJob(IServiceProvider sp)
        {
            _serviceProvider = sp;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var cacheUpdService = scope.ServiceProvider.GetRequiredService<ICacheUpdateService>();
                cacheUpdService.RefreshMemoryCache();
                var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                tokenService.UpdateToken();
            }
        }
    }
}