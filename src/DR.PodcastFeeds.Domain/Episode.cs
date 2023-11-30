namespace DR.PodcastFeeds.Domain;

public class Episode(string id, string title, string description, string duration, DateTime publishingDate)
{
    public readonly string Description = description;
    public readonly string Duration = duration;
    public readonly string Id = id;
    public readonly DateTime PublishingDate = publishingDate;
    public readonly string Title = title;
}