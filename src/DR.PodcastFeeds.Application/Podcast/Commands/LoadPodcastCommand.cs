using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record LoadPodcastCommand(Domain.Podcast Podcast, bool Reload) : IRequest;