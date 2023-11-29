using DR.PodcastFeeds.Domain;
using MongoDB.Bson.Serialization.Attributes;

namespace DR.PodcastFeeds.Infrastructure.DbRecords;

public class PodcastRecord
{
    [BsonId]
    public string Name { get; set; } = null!;
    
    public string Title { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public string ImageUrl { get; set; } = null!;
    
    public string Category { get; set; } = null!;
    
    public string CategorySlug { get; set; } = null!;
    
    public string Link { get; set; } = null!;
    
    public EpisodeRecord[] Episodes { get; set; } = null!;
}

public static class PodcastRecordExtensions
{
    public static Podcast ToPodcast(this PodcastRecord podcastRecord) => new(
        name: podcastRecord.Name,
        title: podcastRecord.Title,
        description: podcastRecord.Description,
        imageUrl: podcastRecord.ImageUrl,
        category: new Category(
            name: podcastRecord.Category,
            slug: podcastRecord.CategorySlug
        ),
        link: podcastRecord.Link,
        episodes: podcastRecord.Episodes.Select(episodeRecord => new Episode(
            id: episodeRecord.Id,
            title: episodeRecord.Title,
            description: episodeRecord.Description,
            duration: episodeRecord.Duration,
            publishingDate: episodeRecord.PublishingDate
        )).ToList()
    );
    
    public static PodcastRecord ToRecord(this Podcast podcast) => new()
    {
        Name = podcast.Name,
        Title = podcast.Title,
        Description = podcast.Description,
        ImageUrl = podcast.ImageUrl,
        Category = podcast.Category.Name,
        CategorySlug = podcast.Category.Slug,
        Link = podcast.Link,
        Episodes = podcast.Episodes.Select(episode => new EpisodeRecord
        {
            Id = episode.Id,
            Title = episode.Title,
            Description = episode.Description,
            Duration = episode.Duration,
            PublishingDate = episode.PublishingDate
        }).ToArray()
    };
}