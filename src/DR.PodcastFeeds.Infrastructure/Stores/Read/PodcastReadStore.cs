using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class PodcastReadStore : IPodcastReadStore
{
    private readonly IMongoCollection<PodcastRecord> _podcasts;
    
    public PodcastReadStore(IOptions<PodcastStoreDatabaseSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        
        var database = client.GetDatabase(settings.Value.DatabaseName);
        
        _podcasts = database.GetCollection<PodcastRecord>(settings.Value.PodcastCollectionName);
    }
    
    public Task<bool> Exists(string name)
    {
        var filter = Builders<PodcastRecord>.Filter.Eq(podcast => podcast.Name, name);
        
        return _podcasts.Find(filter).AnyAsync();
    }

    public Task<Podcast> Get(string name, bool includeEpisodes = false)
    {
        throw new NotImplementedException();
    }
}