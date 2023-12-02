namespace DR.PodcastFeeds.Contracts;

// Needs to be public for the API to be able to deserialize it
public record AdminCredentials(string Username, string Password);