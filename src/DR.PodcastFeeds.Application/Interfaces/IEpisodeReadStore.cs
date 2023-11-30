using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Application.Interfaces;

public interface IEpisodeReadStore
{
    Task<IEnumerable<Episode>> GetEpisodes(string? name = null);
    Task<IEnumerable<Episode>> GetEpisodes(DateOnly fromDate, DateOnly toDate, string? name = null);
    Task<IEnumerable<Episode>> GetEpisodes(int lastCount, string? name = null);
    Task<IEnumerable<Episode>> GetEpisodes(int pageNumber, int pageSize, string? name = null);
}