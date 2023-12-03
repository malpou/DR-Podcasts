using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Api.Extensions;

public static class EpisodeExtensions
{
    public static EpisodesResponse ToResponses(this IEnumerable<Episode> episodes)
    {
        var episodeList = episodes.ToList();

        return new EpisodesResponse(
            episodeList.First().Podcast?.Title ?? string.Empty,
            episodeList.First().Podcast?.Description ?? string.Empty,
            episodeList.First().Podcast?.ImageUrl ?? string.Empty,
            episodeList.First().Podcast?.Category.Name ?? string.Empty,
            episodeList.First().Podcast?.Link ?? string.Empty,
            episodeList.Select(e => e.ToResponse()).ToList());
    }

    public static EpisodeResponse ToResponse(this Episode episode, string? podcastTitle = null,
        string? podcastImageUrl = null)
    {
        return new EpisodeResponse(
            episode.Title,
            episode.PublishingDate,
            episode.Description,
            episode.AudioUrl,
            podcastTitle,
            podcastImageUrl);
    }
}