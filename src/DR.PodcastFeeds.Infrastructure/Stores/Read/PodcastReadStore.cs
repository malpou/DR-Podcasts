using System.Text.RegularExpressions;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class PodcastReadStore(
        IOptions<MongoDbSettings> settings,
        ILogger<PodcastReadStore> logger)
    :
        MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName), 
        IPodcastReadStore
{
    public async Task<bool> Exists(string name)
    {
        var filter = Builders<PodcastRecord>.Filter.Eq(podcast => podcast.Name, name);

        var podcast = await Collection.Find(filter).FirstOrDefaultAsync();

        if (podcast == null)
        {
            logger.LogInformation("Podcast {PodcastName} does not exist", name);

            return false;
        }

        logger.LogInformation("Podcast {PodcastName} exists", name);

        return true;
    }

    public Task<Podcast> Get(string name, bool includeEpisodes = false)
    {
        throw new NotImplementedException();
    }

    

    public async Task<IEnumerable<Podcast>> GetAll(bool includeEpisodes = false)
    {
        var podcasts = await Collection.Find(_ => true).ToListAsync();

        return podcasts.Select(podcast => podcast.ToDomain());
    }
}