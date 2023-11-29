namespace DR.PodcastFeeds.Domain;

public class Category(string name, string slug = null)
{
    public readonly string Slug = slug ?? name.ToLower().Trim().Replace(" ", "-");
    public readonly string Name = name;
}