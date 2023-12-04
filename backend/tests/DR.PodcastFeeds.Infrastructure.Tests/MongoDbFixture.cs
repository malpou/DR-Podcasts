using DR.PodcastFeeds.Infrastructure.Stores;
using Microsoft.Extensions.Options;
using Testcontainers.MongoDb;

namespace DR.PodcastFeeds.Infrastructure.Tests;

public class MongoDbFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer
        = new MongoDbBuilder().Build();

    public IOptions<MongoDbSettings> MongoDbSettings { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        MongoDbSettings = Options.Create(new MongoDbSettings
        {
            ConnectionString = _mongoDbContainer.GetConnectionString(),
            DatabaseName = "PodcastFeeds",
            PodcastCollectionName = "Podcasts"
        });
    }

    public Task DisposeAsync()
    {
        return _mongoDbContainer.DisposeAsync().AsTask();
    }
}