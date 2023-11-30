using DR.PodcastFeeds.Application.Podcast.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public class DeletePodcastEndpoint
{
    public static async Task<IResult> Handle([FromQuery] string name, ISender sender)
    {
        var command = new RemovePodcastCommand(name);
        
        var (result, message) = await sender.Send(command);
        
        return result ? Results.Ok(message) : Results.NotFound(message);
    }
}