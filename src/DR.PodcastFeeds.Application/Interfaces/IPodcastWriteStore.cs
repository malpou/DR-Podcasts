namespace DR.PodcastFeeds.Application.Interfaces;

public interface IPodcastWriteStore
{
    Task<bool> Upsert(Domain.Podcast podcast);
}