using DR.PodcastFeeds.Application.Interfaces;
using DR.PodcastFeeds.Domain;
using DR.PodcastFeeds.Infrastructure.Stores.Read;
using DR.PodcastFeeds.Infrastructure.Stores.Write;

namespace DR.PodcastFeeds.Infrastructure.Tests.Stores;

public class EpisodeReadStoreSpecification(MongoDbFixture fixture) : IClassFixture<MongoDbFixture>, IDisposable
{
    private readonly IPodcastWriteStore _podcastWriteStore
        = new PodcastWriteStore(fixture.MongoDbSettings);

    private readonly IEpisodeReadStore _sut
        = new EpisodeReadStore(fixture.MongoDbSettings);

    public void Dispose()
    {
        _podcastWriteStore.Remove("tiden");
    }

    [Fact]
    public async Task Should_return_episodes()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes();

        foundEpisodes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes()
    {
        var foundEpisodes = await _sut.GetEpisodes();

        foundEpisodes.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_episodes_by_name()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes("tiden");

        foundEpisodes.Should().HaveCount(3);
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes_by_name()
    {
        var foundEpisodes = await _sut.GetEpisodes("tiden");

        foundEpisodes.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_episodes_by_date()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(new DateOnly(2023, 12, 3), new DateOnly(2023, 12, 4));

        foundEpisodes.Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes_by_date()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(new DateOnly(2023, 12, 5), new DateOnly(2023, 12, 5));

        foundEpisodes.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_episodes_by_last_count()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(2);

        foundEpisodes.Should().HaveCount(2);
    }

    [Fact]
    public async Task Should_return_episodes_by_last_count_and_name()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(2, "tiden");

        foundEpisodes.Should().HaveCount(2);
    }

    [Fact]
    private async Task Should_return_all_available_episodes_by_last_count()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(4);

        foundEpisodes.Should().HaveCount(3);
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes_by_last_count()
    {
        var foundEpisodes = await _sut.GetEpisodes(2);

        foundEpisodes.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_one_episode_by_page_size()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(1, 1);

        foundEpisodes.Should().HaveCount(1);
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes_by_page_size()
    {
        var foundEpisodes = await _sut.GetEpisodes(1, 1);

        foundEpisodes.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_one_episode_by_page_size_and_name()
    {
        await SeedDbWithPodcast();

        var foundEpisodes = await _sut.GetEpisodes(1, 1, "tiden");

        foundEpisodes.Should().HaveCount(1);
    }

    [Fact]
    public async Task Should_return_empty_list_if_no_episodes_by_page_size_and_name()
    {
        var foundEpisodes = await _sut.GetEpisodes(1, 1, "tiden");

        foundEpisodes.Should().BeEmpty();
    }


    private async Task SeedDbWithPodcast()
    {
        var episodes = new List<Episode>
        {
            new(
                "11802351222",
                "Fantasiløs fond, diplomatiets mand og CO2-lotteri",
                "175 millioner kroner sender Danmark til den aftalte klimafond, der skal kompensere skader i udviklingslande. Men hvad siger en aktivist fra Argentina? \nDen ene dag er der våbenhvile, den næste kamp igen. Så har diplomatiet overhovedet en betydning i krigen mellem Israel og Hamas? \nOg så begynder bygningen af det første CO2 fangst anlæg i Danmark. \nVærter: Annika Wetterling og Adrian Busk.\nMedvirkende:\nBruno Sirote, klimaaktivist fra Argentina.\nFriis Arne Petersen, tidl. direktør i Udenrigsministeriet.\nFilip Knaack Kirkegaard, redaktør på Klimamonitor.",
                DateTime.Parse("2023-12-04T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351222/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3"),
            new(
                "11802351223",
                "Klimafond, diplomati og CO2 fangst",
                "",
                DateTime.Parse("2023-12-03T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351223/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3"),
            new(
                "11802351224",
                "Klimafond, diplomati og CO2 fangst",
                "",
                DateTime.Parse("2023-12-02T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351224/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3")
        };
        var podcast = new Podcast(
            "Tiden",
            "tiden",
            "Bliv klar til dagen med tre af tidens vigtigste nyhedshistorier - alle hverdage. Fortællinger fra nær og fjern på cirka 20 minutter som du kan fortælle videre. \nVært: Annika Wetterling.",
            "https://www.dr.dk/lyd/special-radio/tiden-3364647813000",
            "https://api.dr.dk/podcasts/v1/images/urn:dr:podcast:image:6523c1657d0c1a140697e0ac.jpg",
            new Category("News", "news"),
            episodes
        );
        await _podcastWriteStore.Upsert(podcast);
    }
}