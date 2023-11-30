﻿using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Podcast.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DR.PodcastFeeds.Api.Endpoints;

public static class PodcastsByCategoryEndpoint
{
    public static async Task<IResult> Handle([FromRoute] string category, ISender sender)
    {
        var podcasts = await sender.Send(new GetPodcastsQuery(category));
        
        return Results.Ok(podcasts.ToResponses());
    }
}