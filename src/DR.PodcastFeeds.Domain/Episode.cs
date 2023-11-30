namespace DR.PodcastFeeds.Domain;

public class Episode(string id, string title, string description, DateTime publishingDate, Podcast? podcast = null!)
{
    public readonly string Description = description;
    public readonly string Id = id;
    public readonly Podcast? Podcast = podcast;
    public readonly DateTime PublishingDate = publishingDate;
    public readonly string Title = title;
}