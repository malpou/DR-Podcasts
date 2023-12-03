using DR.PodcastFeeds.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace DR.PodcastFeeds.Infrastructure.Stores.DbRecords;

public class PodcastRecord
{
    [BsonId] public string Name { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string CategorySlug { get; set; } = null!;

    public string Link { get; set; } = null!;

    public DateTime Updated { get; set; } = DateTime.UtcNow;

    [BsonIgnoreIfNull] public EpisodeRecord[]? Episodes { get; set; }
}

public static class PodcastRecordExtensions
{
    public static Podcast ToDomain(this PodcastRecord podcastRecord)
    {
        return new Podcast(
            name: podcastRecord.Name,
            title: podcastRecord.Title,
            description: podcastRecord.Description,
            imageUrl: podcastRecord.ImageUrl,
            category: new Category(
                podcastRecord.Category,
                podcastRecord.CategorySlug
            ),
            link: podcastRecord.Link,
            episodes: podcastRecord.Episodes?
                .Select(episodeRecord => episodeRecord.ToDomain())
                .ToList() ?? null
        );
    }

    public static PodcastRecord ToRecord(this Podcast podcast)
    {
        return new PodcastRecord
        {
            Name = podcast.Name,
            Title = podcast.Title,
            Description = podcast.Description,
            ImageUrl = podcast.ImageUrl,
            Category = podcast.Category.Name,
            CategorySlug = podcast.Category.Slug,
            Link = podcast.Link,
            Episodes = podcast.Episodes
                .Select(episode => episode.ToRecord())
                .ToArray()
        };
    }
}