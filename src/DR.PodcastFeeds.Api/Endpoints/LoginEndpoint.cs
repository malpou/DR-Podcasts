using DR.PodcastFeeds.Application.Admin.Commands;
using DR.PodcastFeeds.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class LoginEndpoint
{
    public static async Task<IResult> Handle([FromBody] AdminCredentials credentials, ISender sender)
    {
        var token = await sender.Send(new LoginCommand(credentials.Username, credentials.Password));

        return token == null
            ? Results.Unauthorized()
            : Results.Ok(new LoginResponse(token));
    }
}