namespace DR.PodcastFeeds.Application.Interfaces;

public interface ICredentialsManager
{
    Task<bool> AddCredentials(string username, string password);
    Task<bool> UsernameExists(string username);
    Task<bool> VerifyCredentials(string username, string password);
}