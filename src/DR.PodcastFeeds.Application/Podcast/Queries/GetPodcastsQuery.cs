using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Queries;

public record GetPodcastsQuery(string? category = null) : IRequest<IEnumerable<Domain.Podcast>>;