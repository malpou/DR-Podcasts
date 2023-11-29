namespace DR.PodcastFeeds.Infrastructure.Stores.Write;

public interface IPodcastWriteStore
{
    Task Add(Domain.Podcast podcast);
    
    Task Update(Domain.Podcast podcast);
}