using Hangfire;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Podcast.Commands.Handlers;

public class AddPodcastCommandHandler(
    ISender sender,
    IBackgroundJobClient backgroundJob,
    ILogger<AddPodcastCommandHandler> logger
) : IRequestHandler<AddPodcastCommand, (bool, string)>
{
    public async Task<(bool, string)> Handle(
        AddPodcastCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var podcastName = request.Name;

        var validateNameCommand = new ValidatePodcastNameCommand(podcastName);
        var isValid = await sender.Send(validateNameCommand, cancellationToken);

        if (!isValid)
        {
            logger.LogWarning("Podcast name ({PodcastName}) is not valid or already exists", podcastName);

            return (false, "Podcast name  is not valid or already exists");
        }

        var loadPodcastCommand = new LoadPodcastCommand(podcastName);
        backgroundJob.Enqueue<ISender>(x => x.Send(loadPodcastCommand, cancellationToken));

        logger.LogInformation("Podcast ({Podcast}) added to system", podcastName);

        return (true, "Podcast added to system");
    }
}