using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Infrastructure.Services;
using Microsoft.IdentityModel.Tokens;

namespace DR.PodcastFeeds.Infrastructure.Managers;

public class TokenManager(ISecretsService secretsService) : ITokenManager
{
    public async Task<string> GenerateToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(await secretsService.GetJwtSecret());

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "DR.PodcastFeeds",
            Audience = "DR.PodcastFeeds",
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}