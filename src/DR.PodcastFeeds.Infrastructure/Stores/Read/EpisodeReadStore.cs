using System.Text.RegularExpressions;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class EpisodeReadStore(IOptions<MongoDbSettings> settings)
    : MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName), IEpisodeReadStore
{
    private static readonly BsonDocument EpisodeRecordProjection = new()
    {
        {
            "Podcast", new BsonDocument
            {
                {"_id", "$_id"},
                {"Title", "$Title"},
                {"Description", "$Description"},
                {"Category", "$Category"},
                {"Updated", "$Updated"},
                {"CategorySlug", "$CategorySlug"},
                {"ImageUrl", "$ImageUrl"},
                {"Link", "$Link"}
            }
        },
        {"_id", "$Episodes._id"},
        {"Title", "$Episodes.Title"},
        {"Description", "$Episodes.Description"},
        {"PublishingDate", "$Episodes.PublishingDate"},
        {"AudioUrl", "$Episodes.AudioUrl"}
    };

    public async Task<IEnumerable<Episode>> GetEpisodes(string? name = null)
    {
        var nameFilter = GetNameFilter(name);
        return await GetEpisodesCore(nameFilter);
    }

    public async Task<IEnumerable<Episode>> GetEpisodes(DateOnly fromDate, DateOnly toDate, string? name = null)
    {
        var nameFilter = GetNameFilter(name);
        var dateFilter = GetTimeRangeFilter(fromDate, toDate);

        return await GetEpisodesCore(nameFilter, dateFilter: dateFilter);
    }

    public async Task<IEnumerable<Episode>> GetEpisodes(int lastCount, string? name = null)
    {
        var filter = GetNameFilter(name);
        return await GetEpisodesCore(filter, limit: lastCount);
    }

    public async Task<IEnumerable<Episode>> GetEpisodes(int pageNumber, int pageSize, string? name = null)
    {
        var nameFilter = GetNameFilter(name);
        var skip = (pageNumber - 1) * pageSize;
        return await GetEpisodesCore(nameFilter, skip, pageSize);
    }

    private async Task<IEnumerable<Episode>> GetEpisodesCore(FilterDefinition<PodcastRecord> filter, int skip = 0,
        int limit = 0, FilterDefinition<EpisodeRecord>? dateFilter = null)
    {
        var query = Collection.Aggregate()
            .Match(filter)
            .Unwind<PodcastRecord, EpisodeRecord>(podcast => podcast.Episodes)
            .Match(dateFilter ?? Builders<EpisodeRecord>.Filter.Empty)
            .Skip(skip);

        if (limit > 0) query = query.Limit(limit);

        query = query.Project<EpisodeRecord>(EpisodeRecordProjection);

        var episodes = await query.ToListAsync();

        return ProcessEpisodes(episodes);
    }

    private static IEnumerable<Episode> ProcessEpisodes(IEnumerable<EpisodeRecord>? episodes)
    {
        var episodeRecords = episodes?.ToList();

        if (episodeRecords == null || episodeRecords.Count == 0) return Enumerable.Empty<Episode>();

        return episodeRecords.Select(episode => episode.ToDomain(episode.Podcast));
    }

    private static FilterDefinition<PodcastRecord> GetNameFilter(string? name)
    {
        var filter = Builders<PodcastRecord>.Filter.Empty;

        if (string.IsNullOrWhiteSpace(name)) return filter;

        var regex = new Regex(name, RegexOptions.IgnoreCase);
        return Builders<PodcastRecord>.Filter.Regex(podcast => podcast.Name, regex);
    }

    private static BsonDocument GetTimeRangeFilter(DateOnly fromDate, DateOnly toDate)
    {
        var fromDay = fromDate.Day;
        var fromMonth = fromDate.Month;
        var fromYear = fromDate.Year;
        var toDay = toDate.Day;
        var toMonth = toDate.Month;
        var toYear = toDate.Year;

        var dateFilter = new BsonDocument("Episodes.PublishingDate", new BsonDocument
        {
            {"$gte", new DateTime(fromYear, fromMonth, fromDay, 0, 0, 0)},
            {"$lte", new DateTime(toYear, toMonth, toDay, 23, 59, 59)}
        });
        return dateFilter;
    }
}