using MediatR;

namespace DR.PodcastFeeds.Application.Podcast.Commands;

public record RemovePodcastCommand(string Name) : IRequest<(bool, string)>;