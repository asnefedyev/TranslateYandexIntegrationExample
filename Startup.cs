using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using Microsoft.Extensions.ObjectPool;
using TranslatorExample.Services.Common.Contracts;
using TranslatorExample.Services.Common.Implements;
using System.Net.Http.Headers;
using Example.Contracts.Models.Clients;
using Microsoft.AspNetCore.Localization;
using TranslatorExample.Services.CustomBackend.Contracts;
using TranslatorExample.Services.CustomData.Implements;
using TranslateExample.Services.AppServices.Contracts;
using Example.Services.Common.Contracts;
using Example.Services.Common.Implements;
using Example.Services.Common.Implements.Contexts;
using Microsoft.EntityFrameworkCore;
using TranslateExample.Mapping.Profiles;
using TranslateExample.Services.Data.Contracts;
using TranslateExample.Services.Data.Implements;
using TranslateExample.Services.Factories.Contracts;
using TranslateExample.Services.Factories.Implements;
using Quartz;
using TranslateExample.Services.AppServices.Implements;
using TranslateExample.Services.Jobs;
using TranslateExample.Controllers.Proto;

namespace ExampleSource;
public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        /*Http-clients*/
        var httpClients = Configuration.GetSection("HttpClients").Get<List<ClientContractModel>>();
        if (httpClients != null)
        {
            httpClients.ForEach(clientFromConfig =>
            {
                services.AddHttpClient(clientFromConfig.ClientId, client =>
                {
                    client.BaseAddress = new Uri(clientFromConfig.ClientURI);
                    client.DefaultRequestHeaders.Add("User-Agent", clientFromConfig.ClientAgent);
                    if (clientFromConfig.BearerToken is not null)
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", clientFromConfig.BearerToken);
                });
            });
        }

        /*DbContext*/
        services.AddDbContext<ExampleDbContext>(options =>
        {
            var conn = services.BuildServiceProvider().GetService<IDatabaseConnection>();
            options.UseNpgsql(conn.GetDbConnection().ConnectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null));
        }, ServiceLifetime.Scoped);

        /*Cache*/
        services.AddMemoryCache(options =>
        {
            options.TrackStatistics = true;
        });

        /*Mapper*/
        services.AddAutoMapper(typeof(MainProfile));

        /*Data-configuration (repository)*/
        services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        services.AddSingleton<DbConnectionFactoryBuilder, NpgsqlDbConnectionFactoryBuilder>();
        services.AddSingleton<IDatabaseConnection, NpgsqlDatabaseConnection>();
        services.AddSingleton<IDataEngineServiceBuilder, DataEngineServiceBuilder>();
        services.AddTransient<IDataEngineService, NpgsqlDataEngineService>();
      
        /*Data-services*/
        services.AddScoped<ICacheDataService, CacheDataService>();
        services.AddScoped<ITokenDataService, TokenDataService>();

        /*Backend*/
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<PhraseMemoryCache>();
        services.AddScoped<PhraseDbCache>();
        services.AddScoped<IPhraseCacheFactory, PhraseCacheFactory>();
        services.AddScoped<IExternalTranslateService, ExternalYandexTranslateServices>();
        services.AddScoped<ICacheUpdateService, CacheUpdateService>();
        
        /*Auto tasks*/
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey("RefreshCacheMemoryJob");
            q.AddJob<RefreshMemoryCacheJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("RefreshCacheMemoryTrigger")
                //.WithCronSchedule("0 0 * * * ?")
                .WithSimpleSchedule(it => it.WithIntervalInHours(1)));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        /*Web*/
        services.AddGrpc();
        services.AddMvc().AddControllersAsServices();
        services.AddRouting();

        /*Compression*/
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
        services.AddResponseCompression(options =>
        {
            options.Providers.Add<GzipCompressionProvider>();
            options.EnableForHttps = true;
        });

        var serviceProvider = services.BuildServiceProvider();
                
        services.AddCors(options =>
        {
            options.AddPolicy("ExamplePolicy",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
        });
        //services.AddHttpContextAccessor();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("ru-RU"),  
        });        

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGrpcService<TranslaterService>();
        });
    }
}