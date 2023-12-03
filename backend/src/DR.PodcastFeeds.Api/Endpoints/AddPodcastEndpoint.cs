using DR.PodcastFeeds.Application.Podcast.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class AddPodcastEndpoint
{
    public static async Task<IResult> Handle([FromRoute] string podcastSlug, ISender sender)
    {
        var command = new AddPodcastCommand(podcastSlug);

        var (result, message) = await sender.Send(command);

        return result ? Results.Created("podcasts/" + podcastSlug, message) : Results.BadRequest(message);
    }
}