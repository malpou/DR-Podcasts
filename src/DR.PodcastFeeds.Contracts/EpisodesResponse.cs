namespace DR.PodcastFeeds.Contracts;

public record EpisodesResponse(
    string PodcastTitle,
    string PodcastDescription,
    string PodcastImageUrl,
    string PodcastCategory,
    List<EpisodeResponse> Episodes);