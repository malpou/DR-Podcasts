using MongoDB.Bson.Serialization.Attributes;

namespace DR.PodcastFeeds.Infrastructure.Stores.DbRecords;

public class EpisodeRecord
{
    [BsonId] public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public DateTime PublishingDate { get; set; }
}