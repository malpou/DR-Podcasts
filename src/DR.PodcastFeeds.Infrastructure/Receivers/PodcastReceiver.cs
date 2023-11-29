using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Infrastructure.Receivers;

internal class PodcastReceiver : IPodcastReceiver
{
    public Task<Podcast> GetPodcastWithEpisodes(string name, int? last = null)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Podcast>> GetPodcasts(string? category = null)
    {
        throw new NotImplementedException();
    }
}