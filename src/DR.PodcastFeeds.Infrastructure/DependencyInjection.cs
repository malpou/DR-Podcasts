using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Clients;
using DR.PodcastFeeds.Infrastructure.Managers;
using DR.PodcastFeeds.Infrastructure.Receivers;
using DR.PodcastFeeds.Infrastructure.Stores;
using DR.PodcastFeeds.Infrastructure.Stores.Read;
using DR.PodcastFeeds.Infrastructure.Stores.Write;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddSingleton<IPodcastManager, PodcastManager>();
        services.AddSingleton<IPodcastReceiver, PodcastReceiver>();
        
        services.AddHttpClient<IPodcastFeedClient, PodcastFeedClient>();
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // docker run -d -p 27017:27017 --name example-mongo mongo:latest
        
        services.Configure<PodcastStoreDatabaseSettings>(
            configuration.GetSection("PodcastDatabaseSettings"));
        
        services.AddSingleton<IPodcastReadStore, PodcastReadStore>();
        services.AddSingleton<IPodcastWriteStore, PodcastWriteStore>();
    }
}