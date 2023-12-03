using DR.PodcastFeeds.Application.Podcast.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class DeletePodcastEndpoint
{
    public static async Task<IResult> Handle([FromRoute] string podcastSlug, ISender sender)
    {
        var command = new RemovePodcastCommand(podcastSlug);

        var (result, message) = await sender.Send(command);

        return result ? Results.NoContent() : Results.NotFound(message);
    }
}