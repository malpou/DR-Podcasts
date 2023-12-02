using DR.PodcastFeeds.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Admin.Commands.Handlers;

public class AddAdminCredentialsCommandHandler(
        ICredentialsManager credentialsManager,
        ILogger<AddAdminCredentialsCommandHandler> logger)
    : IRequestHandler<AddAdminCredentialsCommand, bool>
{
    public async Task<bool> Handle(AddAdminCredentialsCommand request, CancellationToken cancellationToken)
    {
        var username = request.Username;
        var password = request.Password;

        var exists = await credentialsManager.UsernameExists(username);

        if (exists)
        {
            logger.LogInformation("Credentials already exists for user {Username}", username);
            return false;
        }

        var success = await credentialsManager.AddCredentials(username, password);

        if (success)
        {
            logger.LogInformation("Credentials added for user {Username}", username);

            return true;
        }

        logger.LogCritical("Credentials not added for user {Username}", username);

        return false;
    }
}