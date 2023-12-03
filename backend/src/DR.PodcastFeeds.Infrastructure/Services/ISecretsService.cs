namespace DR.PodcastFeeds.Infrastructure.Services;

public interface ISecretsService
{
    public Task<bool> AddCredentials(string username, string hashedPassword);
    public Task<string> GetHashedPassword(string username);
    public Task<string> GetJwtSecret();
}