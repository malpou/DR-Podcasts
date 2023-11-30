using System.Text.RegularExpressions;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.DbRecords;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DR.PodcastFeeds.Infrastructure.Stores.Read;

public class EpisodeReadStore(
    IOptions<MongoDbSettings> settings,
    ILogger<EpisodeReadStore> logger) : MongoDbStoreBase<PodcastRecord>(settings, settings.Value.PodcastCollectionName),
    IEpisodeReadStore
{
    public async Task<IEnumerable<Episode>> GetEpisodes(string? name = null)
    {
        var nameFilter = GetNameFilter(name);

        var episodes = await Collection.Aggregate()
            .Match(nameFilter)
            .Unwind<PodcastRecord, EpisodeRecord>(podcast => podcast.Episodes)
            .Project<EpisodeRecord>(EpisodeRecordProjection)
            .ToListAsync();

        if (episodes == null || episodes.Count == 0)
        {
            logger.LogInformation("No episodes found");

            return Enumerable.Empty<Episode>();
        }

        logger.LogInformation("Found {EpisodeCount} episodes", episodes.Count);

        return episodes.Select(episode => episode.ToDomain(episode.Podcast));
    }


    public async Task<IEnumerable<Episode>> GetEpisodes(DateOnly fromDate, DateOnly toDate, string? name = null)
    {
        var nameFilter = GetNameFilter(name);

        var dateFilter = GetTimeRangeFilter(fromDate, toDate);

        var episodes = await Collection.Aggregate()
            .Match(nameFilter)
            .Unwind<PodcastRecord, EpisodeRecord>(podcast => podcast.Episodes)
            .Match(dateFilter)
            .Project<EpisodeRecord>(EpisodeRecordProjection)
            .ToListAsync();
        
        if (episodes == null || episodes.Count == 0)
        {
            logger.LogInformation("No episodes found");

            return Enumerable.Empty<Episode>();
        }
        
        logger.LogInformation("Found {EpisodeCount} episodes", episodes.Count);

        return episodes.Select(episode => episode.ToDomain(episode.Podcast));
    }


    public async Task<IEnumerable<Episode>> GetEpisodes(int lastCount, string? name = null)
    {
        var filter = GetNameFilter(name);
        
        var episodes = await Collection.Aggregate()
            .Match(filter)
            .Unwind<PodcastRecord, EpisodeRecord>(podcast => podcast.Episodes)
            .Limit(lastCount)
            .Project<EpisodeRecord>(EpisodeRecordProjection)
            .ToListAsync();
        
        if (episodes == null || episodes.Count == 0)
        {
            logger.LogInformation("No episodes found");

            return Enumerable.Empty<Episode>();
        }
        
        logger.LogInformation("Found {EpisodeCount} episodes", episodes.Count);
        
        return episodes.Select(episode => episode.ToDomain(episode.Podcast));
    }

    public async Task<IEnumerable<Episode>> GetEpisodes(int pageNumber, int pageSize, string? name = null)
    {
        var nameFilter = GetNameFilter(name);
        
        var episodes = await Collection.Aggregate()
            .Match(nameFilter)
            .Unwind<PodcastRecord, EpisodeRecord>(podcast => podcast.Episodes)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .Project<EpisodeRecord>(EpisodeRecordProjection)
            .ToListAsync();
        
        if (episodes == null || episodes.Count == 0)
        {
            logger.LogInformation("No episodes found");

            return Enumerable.Empty<Episode>();
        }
        
        logger.LogInformation("Found {EpisodeCount} episodes", episodes.Count);
        
        return episodes.Select(episode => episode.ToDomain(episode.Podcast));
    }

    private static FilterDefinition<PodcastRecord> GetNameFilter(string? name)
    {
        var filter = Builders<PodcastRecord>.Filter.Empty;

        if (string.IsNullOrWhiteSpace(name)) return filter;

        var regex = new Regex(name, RegexOptions.IgnoreCase);
        filter = Builders<PodcastRecord>.Filter.Regex(podcast => podcast.Name, regex);

        return filter;
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
        {"Duration", "$Episodes.Duration"},
        {"PublishingDate", "$Episodes.PublishingDate"}
    };
}