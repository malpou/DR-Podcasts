using DR.PodcastFeeds.Application.Episodes.Queries;
using DR.PodcastFeeds.Application.Episodes.Queries.Handlers;
using DR.PodcastFeeds.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace DR.PodcastFeeds.Application.Tests.Episodes.Queries;

public class GetEpisodesQuerySpecification
{
    private readonly IEpisodeReadStore _episodeReadStore = Substitute.For<IEpisodeReadStore>();
    private readonly ILogger<GetEpisodesQueryHandler> _logger = Substitute.For<ILogger<GetEpisodesQueryHandler>>();
    private readonly IPodcastReadStore _podcastReadStore = Substitute.For<IPodcastReadStore>();

    [Fact]
    public async Task Should_return_null_when_name_not_exists()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(false);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", null, null, null, null, null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().BeNull();
        await _episodeReadStore.DidNotReceive().GetEpisodes();
    }

    [Fact]
    public async Task Should_call_episodes_store_with_last()
    {
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery(null, null, null, null, null, 10);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(10);
    }

    [Fact]
    public async Task Should_call_episodes_store_with_last_and_name()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(true);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", null, null, null, null, 10);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(10, "tiden");
    }

    [Fact]
    public async Task Should_call_episodes_store_with_from_and_to()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(true);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", null, null, new DateOnly(2021, 1, 1), new DateOnly(2021, 1, 2), null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(new DateOnly(2021, 1, 1), new DateOnly(2021, 1, 2), "tiden");
    }


    [Fact]
    public async Task Should_call_episodes_store_with_from_and_to_and_name()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(true);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", null, null, new DateOnly(2021, 1, 1), new DateOnly(2021, 1, 2), null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(new DateOnly(2021, 1, 1), new DateOnly(2021, 1, 2), "tiden");
    }

    [Fact]
    public async Task Should_call_episodes_store_with_page_and_size()
    {
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery(null, 1, 1, null, null, null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(1, 1);
    }

    [Fact]
    public async Task Should_call_episodes_store_with_page_and_size_and_name()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(true);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", 1, 1, null, null, null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes(1, 1, "tiden");
    }

    [Fact]
    public async Task Should_call_episodes_store()
    {
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery(null, null, null, null, null, null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes();
    }

    [Fact]
    public async Task Should_call_episodes_store_with_name()
    {
        _podcastReadStore.PodcastsExists("tiden").Returns(true);
        var sut = new GetEpisodesQueryHandler(_episodeReadStore, _podcastReadStore, _logger);
        var query = new GetEpisodesQuery("tiden", null, null, null, null, null);

        var result = await sut.Handle(query, CancellationToken.None);

        result.Should().NotBeNull();
        await _episodeReadStore.Received().GetEpisodes("tiden");
    }
}