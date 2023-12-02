using System.Security.Claims;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Auth;
using DR.PodcastFeeds.Infrastructure.Clients;
using DR.PodcastFeeds.Infrastructure.Managers;
using DR.PodcastFeeds.Infrastructure.Services;
using DR.PodcastFeeds.Infrastructure.Stores;
using DR.PodcastFeeds.Infrastructure.Stores.Read;
using DR.PodcastFeeds.Infrastructure.Stores.Write;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace DR.PodcastFeeds.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices(configuration);
        services.AddPersistence(configuration);
        services.AddJwtAuthentication();
        services.AddAdminAuthorization();
    }

    private static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPodcastFeedClient, PodcastFeedClient>();
        services.AddSingleton<ISecretsService>(_ => new SecretsService(configuration.GetConnectionString("KeyVault")!));
        services.AddSingleton<ICredentialsManager, CredentialsManager>();
        services.AddSingleton<ITokenManager, TokenManager>();
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(nameof(MongoDbSettings)));

        var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!);

        services.AddHangfire(config => { config.UseRedisStorage(redis); });
        services.AddHangfireServer();

        services.AddSingleton<ICategoryReadStore, CategoryReadStore>();
        services.AddSingleton<IPodcastReadStore, PodcastReadStore>();
        services.AddSingleton<IEpisodeReadStore, EpisodeReadStore>();

        services.AddSingleton<IPodcastWriteStore, PodcastWriteStore>();
    }
}