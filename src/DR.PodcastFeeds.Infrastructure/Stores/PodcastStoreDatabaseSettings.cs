namespace DR.PodcastFeeds.Infrastructure.Stores;

public class PodcastStoreDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string PodcastCollectionName { get; set; } = null!;

}