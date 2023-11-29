using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Write;

public class PodcastWriteStore : IPodcastWriteStore
{
    private readonly IMongoCollection<PodcastRecord> _podcasts;
    
    public PodcastWriteStore(IOptions<PodcastStoreDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        
        var database = client.GetDatabase(settings.Value.DatabaseName);
        
        _podcasts = database.GetCollection<PodcastRecord>(settings.Value.PodcastCollectionName);
    }
    
    public Task Add(Podcast podcast)
    {
        var record = podcast.ToRecord();
        
        return _podcasts.InsertOneAsync(record);
    }

    public Task Update(Podcast podcast)
    {
        throw new NotImplementedException();
    }
}