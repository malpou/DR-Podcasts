using DR.PodcastFeeds.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace DR.PodcastFeeds.Infrastructure.Stores.DbRecords;

public class EpisodeRecord
{
    [BsonId] public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime PublishingDate { get; set; }

    public string AudioUrl { get; set; } = null!;

    [BsonIgnoreIfNull] public PodcastRecord? Podcast { get; set; } = null!;
}

public static class EpisodeRecordExtensions
{
    public static Episode ToDomain(this EpisodeRecord episodeRecord, PodcastRecord? podcastRecord = null)
    {
        return new Episode(
            episodeRecord.Id,
            episodeRecord.Title,
            episodeRecord.Description,
            episodeRecord.PublishingDate,
            episodeRecord.AudioUrl,
            podcastRecord?.ToDomain());
    }

    public static EpisodeRecord ToRecord(this Episode episode)
    {
        return new EpisodeRecord
        {
            Id = episode.Id,
            Title = episode.Title,
            Description = episode.Description,
            AudioUrl = episode.AudioUrl,
            PublishingDate = episode.PublishingDate
        };
    }
}