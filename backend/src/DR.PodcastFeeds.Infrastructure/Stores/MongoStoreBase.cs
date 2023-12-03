using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores;

public abstract class MongoDbStoreBase<TRecord>
{
    protected readonly IMongoCollection<TRecord> Collection;

    protected MongoDbStoreBase(IOptions<MongoDbSettings> settings, string collectionName)
    {
        var clientSettings = MongoClientSettings.FromConnectionString(settings.Value.ConnectionString);
        clientSettings.ServerApi = new ServerApi(ServerApiVersion.V1);

        var client = new MongoClient(clientSettings);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = database.GetCollection<TRecord>(collectionName);
    }
}