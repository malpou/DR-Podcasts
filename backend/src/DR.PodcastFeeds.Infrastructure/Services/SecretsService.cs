using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace DR.PodcastFeeds.Infrastructure.Services;

public class SecretsService(string connectionString) : ISecretsService
{
    private const string JwtSecretName = "JwtSecret";
    private const string PasswordPrefix = "Password-";
    private readonly SecretClient _client = new(new Uri(connectionString), new DefaultAzureCredential());

    public async Task<bool> AddCredentials(string username, string hashedPassword)
    {
        var response = await _client.SetSecretAsync($"{PasswordPrefix}{username}", hashedPassword);
        return response.Value != null;
    }

    public async Task<string> GetHashedPassword(string username)
    {
        var response = await _client.GetSecretAsync($"{PasswordPrefix}{username}");
        return response.Value.Value;
    }

    public async Task<string> GetJwtSecret()
    {
        var response = await _client.GetSecretAsync(JwtSecretName);
        return response.Value.Value;
    }
}