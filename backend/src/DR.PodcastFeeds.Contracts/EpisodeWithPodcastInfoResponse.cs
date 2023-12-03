namespace DR.PodcastFeeds.Contracts;

public record EpisodeWithPodcastInfoResponse(string Id, string Title, DateTime Published, string Description,
    string AudioUrl,
    string PodcastTitle, string PodcastImageUrl) : EpisodeResponse(Id, Title, Published, Description, AudioUrl);