namespace DR.PodcastFeeds.Contracts;

public record EpisodeResponse(string Title, DateTime Published, string Description, string? PodcastTitle, string? PodcastImageUrl);