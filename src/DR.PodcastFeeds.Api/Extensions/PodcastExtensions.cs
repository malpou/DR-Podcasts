using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Domain;

namespace DR.PodcastFeeds.Api.Extensions;

public static class PodcastExtensions
{
    public static List<PodcastResponse> ToResponses(this IEnumerable<Podcast> podcasts)
    {
        return podcasts.Select(p => p.ToResponse()).ToList();
    }

    private static PodcastResponse ToResponse(this Podcast podcast)
    {
        return new PodcastResponse(podcast.Name, podcast.Title, podcast.Description, podcast.ImageUrl, podcast.Category.Name);
    }
}