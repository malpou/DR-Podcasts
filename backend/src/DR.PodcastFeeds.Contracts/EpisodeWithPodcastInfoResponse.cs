namespace DR.PodcastFeeds.Contracts;

public record EpisodeWithPodcastInfoResponse(string Title, DateTime Published, string Description, string AudioUrl,
    string PodcastTitle, string PodcastImageUrl) : EpisodeResponse(Title, Published, Description, AudioUrl);