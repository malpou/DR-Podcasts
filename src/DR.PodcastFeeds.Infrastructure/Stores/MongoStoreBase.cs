using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores;

public abstract class MongoDbStoreBase<TRecord>
{
    protected readonly IMongoCollection<TRecord> Collection;

    protected MongoDbStoreBase(IOptions<MongoDbSettings> settings, string collectionName)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        Collection = database.GetCollection<TRecord>(collectionName);
    }
}