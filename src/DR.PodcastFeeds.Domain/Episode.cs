namespace DR.PodcastFeeds.Domain;

public class Episode(string id, string title, string description, string duration, DateTime publishingDate)
{
    public readonly string Id = id;
    public readonly string Title = title;
    public readonly string Description = description;
    public readonly string Duration = duration;
    public readonly DateTime PublishingDate = publishingDate;
}