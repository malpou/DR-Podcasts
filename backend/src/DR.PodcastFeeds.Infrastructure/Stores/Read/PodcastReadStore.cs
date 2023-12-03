using System.Text.RegularExpressions;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class PodcastReadStore(IOptions<MongoDbSettings> settings)
    : MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName), IPodcastReadStore
{
    public async Task<bool> PodcastsExists(string name)
    {
        var filter = Builders<PodcastRecord>.Filter.Eq(podcast => podcast.Name, name);

        var podcast = await Collection.Find(filter).FirstOrDefaultAsync();

        return podcast != null;
    }

    public Task<IEnumerable<Podcast>> GetAll(string? category = null)
    {
        var filter = Builders<PodcastRecord>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(category))
        {
            var regex = new Regex(category, RegexOptions.IgnoreCase);

            filter = Builders<PodcastRecord>.Filter.Regex(podcast => podcast.CategorySlug, regex);
        }

        var podcasts = Collection.Find(filter).SortBy(p => p.Title).ToList();

        return Task.FromResult(podcasts.Select(podcast => podcast.ToDomain()));
    }
}