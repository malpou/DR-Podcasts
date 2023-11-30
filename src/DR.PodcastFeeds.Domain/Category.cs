namespace DR.PodcastFeeds.Domain;

public class Category(string name, string? slug = null)
{
    public readonly string Name = name;
    public readonly string Slug = slug ?? name.ToLower().Trim().Replace(" ", "-");
}