using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record LoadPodcastCommand(string Name) : IRequest;