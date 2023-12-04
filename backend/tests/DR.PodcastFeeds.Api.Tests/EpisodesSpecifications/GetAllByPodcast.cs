using DR.PodcastFeeds.Api.Endpoints;
using DR.PodcastFeeds.Api.Extensions;
using DR.PodcastFeeds.Application.Episodes.Queries;
using DR.PodcastFeeds.Contracts;
using DR.PodcastFeeds.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DR.PodcastFeeds.Api.Tests.EpisodesSpecifications;

public class GetAllByPodcast
{
    [Fact]
    public async Task Should_return_200_and_episodes_response_when_found()
    {
        var episodes = GetListOfEpisodes();
        var sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<GetEpisodesQuery>())
            .Returns(episodes);

        var result = await EpisodesByPodcastEndpoint.Handle("tiden", null, null, null, null, null, sender);

        var okResult = (Ok<EpisodesResponse>) result;
        okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
        okResult.Value?.Episodes.Should().Contain(episodes.ToResponse().Episodes);
    }

    [Fact]
    public async Task Should_return_404_when_not_found()
    {
        var sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<GetEpisodesQuery>())
            .Returns((List<Episode>) null!);

        var result = await EpisodesByPodcastEndpoint.Handle("tiden", null, null, null, null, null, sender);

        result.Should().BeOfType<NotFound>();
    }

    [Theory]
    [InlineData(0, 1, null, null, null)]
    [InlineData(1, 0, null, null, null)]
    [InlineData(1, 200, null, null, null)]
    [InlineData(null, null, "2021-01-01", null, null)]
    [InlineData(null, null, null, "2021-01-01", null)]
    [InlineData(null, null, "2021-01-02", "2021-01-01", null)]
    [InlineData(null, null, null, null, 0)]
    [InlineData(null, null, null, null, -1)]
    [InlineData(null, null, null, null, 200)]
    [InlineData(1, 1, "2021-01-01", "2021-01-01", null)]
    [InlineData(1, 1, null, null, 1)]
    [InlineData(null, null, "2021-01-01", "2021-01-01", 2)]
    public async Task Should_return_400_when_invalid_request(int? page, int? size, string? from, string? to, int? last)
    {
        var sender = Substitute.For<ISender>();

        DateOnly? fromDate = null;
        if (from is not null)
            fromDate = DateOnly.FromDateTime(DateTime.Parse(from));

        DateOnly? toDate = null;
        if (to is not null)
            toDate = DateOnly.FromDateTime(DateTime.Parse(to));

        var result = await EpisodesByPodcastEndpoint.Handle("tiden", page, size, fromDate, toDate, last, sender);

        result.Should().BeOfType<BadRequest<string>>();
    }

    [Fact]
    public async Task Should_return_204_with_when_no_episodes_where_found()
    {
        var sender = Substitute.For<ISender>();
        sender.Send(Arg.Any<GetEpisodesQuery>())
            .Returns(new List<Episode>());

        var result = await EpisodesByPodcastEndpoint.Handle("tiden", null, null, null, null, null, sender);

        result.Should().BeOfType<NoContent>();
    }

    private static List<Episode> GetListOfEpisodes(bool addPodcast = false)
    {
        var podcast = new Podcast(
            "Tiden",
            "tiden",
            "Bliv klar til dagen med tre af tidens vigtigste nyhedshistorier - alle hverdage. Fortællinger fra nær og fjern på cirka 20 minutter som du kan fortælle videre. \nVært: Annika Wetterling.",
            "https://www.dr.dk/lyd/special-radio/tiden-3364647813000",
            "https://api.dr.dk/podcasts/v1/images/urn:dr:podcast:image:6523c1657d0c1a140697e0ac.jpg",
            new Category("News", "news")
        );

        var episodes = new List<Episode>
        {
            new(
                "11802351222",
                "Fantasiløs fond, diplomatiets mand og CO2-lotteri",
                "175 millioner kroner sender Danmark til den aftalte klimafond, der skal kompensere skader i udviklingslande. Men hvad siger en aktivist fra Argentina? \nDen ene dag er der våbenhvile, den næste kamp igen. Så har diplomatiet overhovedet en betydning i krigen mellem Israel og Hamas? \nOg så begynder bygningen af det første CO2 fangst anlæg i Danmark. \nVærter: Annika Wetterling og Adrian Busk.\nMedvirkende:\nBruno Sirote, klimaaktivist fra Argentina.\nFriis Arne Petersen, tidl. direktør i Udenrigsministeriet.\nFilip Knaack Kirkegaard, redaktør på Klimamonitor.",
                DateTime.Parse("2023-12-04T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351222/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3",
                addPodcast ? podcast : null),
            new(
                "11802351223",
                "Klimafond, diplomati og CO2 fangst",
                "",
                DateTime.Parse("2023-12-03T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351223/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3",
                addPodcast ? podcast : null),
            new(
                "11802351224",
                "Klimafond, diplomati og CO2 fangst",
                "",
                DateTime.Parse("2023-12-02T02:00:00.000+00:00"),
                "https://api.dr.dk/podcasts/v1/assets/urn:dr:podcast:item:11802351224/fe6ed19be0262f4417f449273d35d3eb20de8e0cafd5fc4aeb133cf75a7fe506.mp3",
                addPodcast ? podcast : null)
        };

        return episodes;
    }
}