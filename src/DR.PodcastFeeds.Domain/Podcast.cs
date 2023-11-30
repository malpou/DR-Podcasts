namespace DR.PodcastFeeds.Domain;

public class Podcast(string title, string name, string description, string link, string imageUrl, Category category,
    IEnumerable<Episode> episodes)
{
    public readonly Category Category = category;
    public readonly string Description = description;
    public readonly Episode[] Episodes = episodes.ToArray();
    public readonly string ImageUrl = imageUrl;
    public readonly string Link = link;
    public readonly string Name = name;
    public readonly string Title = title;
}