namespace DR.PodcastFeeds.Contracts;

public record PodcastResponse(string Slug, string Title, string Description, string ImageUrl,
    string Category);