using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Clients;
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
        services.AddServices();
        services.AddPersistence(configuration);
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddHttpClient<IPodcastFeedClient, PodcastFeedClient>();
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