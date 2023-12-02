namespace DR.PodcastFeeds.Application.Interfaces;

public interface ITokenManager
{
    Task<string> GenerateToken(string username);
}