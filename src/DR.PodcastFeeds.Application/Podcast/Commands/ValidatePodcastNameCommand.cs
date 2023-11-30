using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record ValidatePodcastNameCommand(string Name) : IRequest<bool>;