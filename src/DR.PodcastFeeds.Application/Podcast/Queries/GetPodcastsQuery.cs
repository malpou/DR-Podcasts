using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Queries;

public record GetPodcastsQuery(string? Category = null) : IRequest<IEnumerable<Domain.Podcast>>;