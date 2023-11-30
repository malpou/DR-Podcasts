namespace DR.PodcastFeeds.Contracts;

public record EpisodesResponse(
    string PodcastTitle,
    string PodcastDescription,
    string PodcastImageUrl,
    string PodcastCategory,
    string PodcastLink,
    List<EpisodeResponse> Episodes);