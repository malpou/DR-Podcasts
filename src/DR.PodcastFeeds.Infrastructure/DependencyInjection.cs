using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Clients;
using DR.PodcastFeeds.Infrastructure.Stores;
using DR.PodcastFeeds.Infrastructure.Stores.Read;
using DR.PodcastFeeds.Infrastructure.Stores.Write;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

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

        
        
        var mongoUrlBuilder = new MongoUrlBuilder(configuration["MongoDbSettings:ConnectionString"] + "/jobs");
        var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

        services.AddHangfire(config =>
        {
            config.UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new CollectionMongoBackupStrategy()
                },
                CheckConnection = true,
                CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.Poll
            });
        });
        services.AddHangfireServer();

        services.AddSingleton<ICategoryReadStore, CategoryReadStore>();
        services.AddSingleton<IPodcastReadStore, PodcastReadStore>();
        services.AddSingleton<IPodcastWriteStore, PodcastWriteStore>();
    }
}