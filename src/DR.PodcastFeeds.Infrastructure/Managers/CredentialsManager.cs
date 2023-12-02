using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Services;

namespace DR.PodcastFeeds.Infrastructure.Managers;

public class CredentialsManager(ISecretsService secretsService) : ICredentialsManager
{
    public async Task<bool> AddCredentials(string username, string password)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var success = await secretsService.AddCredentials(username, hashedPassword);

        return success;
    }

    public async Task<bool> UsernameExists(string username)
    {
        var password = await secretsService.GetHashedPassword(username);

        var exists = !string.IsNullOrEmpty(password);

        return exists;
    }

    public async Task<bool> VerifyCredentials(string username, string password)
    {
        var hashedPassword = await secretsService.GetHashedPassword(username);

        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}