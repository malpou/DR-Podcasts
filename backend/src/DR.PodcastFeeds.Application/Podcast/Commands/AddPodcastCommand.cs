using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record AddPodcastCommand(string Name)
    : IRequest<(bool, string)>;