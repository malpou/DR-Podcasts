namespace DR.PodcastFeeds.Contracts;

public record EpisodeResponse(string Id, string Title, DateTime Published, string Description, string AudioUrl);