using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Admin.Commands.Handlers;

public class LoginCommandHandler(
        ICredentialsManager credentialsManager,
        ITokenManager tokenManager,
        ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, string?>
{
    public async Task<string?> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var username = request.Username;
        var password = request.Password;

        var validCredentials = await credentialsManager.VerifyCredentials(username, password);

        if (validCredentials == false)
        {
            logger.LogWarning("Invalid credentials for user {Username}", username);

            return null;
        }

        var token = await tokenManager.GenerateToken(username);

        logger.LogInformation("User {Username} logged in", username);

        return token;
    }
}