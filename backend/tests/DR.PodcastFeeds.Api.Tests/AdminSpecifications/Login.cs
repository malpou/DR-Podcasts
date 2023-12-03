using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Application.Podcast.Commands;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DR.PodcastFeeds.Api.Tests.AdminSpecifications;

public class Post
{
    private const string RssFeed = "https://api.dr.dk/podcasts/v1/feeds/djaevlen-i-detaljen";

    [Fact]
    public async Task Should_return_201_when_feed_is_added()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<AddPodcastCommand>()).Returns((true, "Feed added"));

        // Act
        var response = await AddPodcastEndpoint.Handle(RssFeed, sender);

        // Assert
        response.Should().BeOfType<Ok<string>>();
    }

    [Fact]
    public async Task Should_return_400_when_feed_is_not_added()
    {
        // Arrange
        var sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<AddPodcastCommand>()).Returns((false, "Feed not added"));

        // Act
        var response = await AddPodcastEndpoint.Handle(RssFeed, sender);

        // Assert
        response.Should().BeOfType<BadRequest<string>>();
    }
}