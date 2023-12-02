using DR.PodcastFeeds.Application.Admin.Commands;
using DR.PodcastFeeds.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class RegisterAdminEndpoints
{
    public static async Task<IResult> Handle([FromBody] AdminCredentials credentials, ISender sender)
    {
        var result = await sender.Send(new AddAdminCredentialsCommand(credentials.Username, credentials.Password));

        return result ? Results.Ok() : Results.BadRequest();
    }
}