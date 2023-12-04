# Assignment
The task is to build a REST API that exposes data from our existing podcast feeds at DR, with the following requirements. You are very welcome to justify any choices or rejections, in case you do not complete everything.
- It should be able to expose JSON data for a podcast feed based on a given name/id, e.g., "tiden" or "orientering".
- For a podcast feed, at a minimum, it should expose the title, description, link, category, and contained items (episodes).
- For each item, at a minimum, it should expose the id (guid), title, description, and publication date.
- It should be able to filter podcast feed items so that only items from a given publication date (pubDate) onwards are exposed.
- It should be able to limit how many items are exposed for the given podcast feed.
- We would like to see an example of a unit test.

## Examples of podcast feeds:
- https://api.dr.dk/podcasts/v1/feeds/tiden.xml?limit=500
- https://api.dr.dk/podcasts/v1/feeds/orientering.xml?limit=10

More podcasts can be found here: https://www.dr.dk/lyd, and you can find the actual podcast feed by clicking into a given podcast and selecting "RSS".


# Solution
The Rest API is implemented in .Net 8 using C#. The solution is following Clean Architecture therefor divided into 5 source projects:
- DR.PodcastFeeds.Api: The REST API (built on minimal APIs acting as the presentation layer)
- DR.PodcastFeeds.Domain: The domain class models (in this case the Podcast, Episode and Category classes)
- DR.PodcastFeeds.Infrastructure: The infrastructure layer (in this case the MongoDB database and HttpClient)
- DR.PodcastFeeds.Contracts: The contracts for the REST API (in this case the DTOs)
- DR.PodcastFeeds.Application: The application layer (implemented as a mediator pattern using MediatR)

## API Specification
The API specification is available at https://dr-podcasts.azurewebsites.net/swagger

## Client application
A client application that consumes the API is available at https://podcasts.malpou.dev/

This is built using https://kit.svelte.dev/

## Categories
The categories can be retrieved using a GET request to the `/categories` endpoint.

## Podcasts
The podcasts can be retrieved using a GET request to the `/podcasts` endpoint. The podcasts can be filtered by category using the `/categories/{categorySlug}/podcasts` endpoint.

## Episodes
The episodes can be retrieved using a GET request to the `/podcasts/{podcastSlug}/episodes` endpoint or the `/episodes` endpoint.

### Episode filtering (Query parameters)
The episodes can be filtered in 3 ways:
- Last n episodes: Will return the last n episodes, specified by the `?last=n` parameter.
- Episodes in a date range: Will return the episodes in a date range, specified by the `?from=yyyy-MM-dd&to=yyyy-MM-dd` parameters.
- Pagination: Will return episodes in a paginated manner, specified by the `?page=n&size=n` parameters.

#### Examples
- https://dr-podcasts.azurewebsites.net/podcasts/tiden/episodes?last=10
- https://dr-podcasts.azurewebsites.net/podcasts/tiden/episodes?from=2023-11-01&to=2023-11-30
- https://dr-podcasts.azurewebsites.net/podcasts/tiden/episodes?page=5&size=5

## Configuration
The system can get added or removed podcast feeds using HTTP POST and DELETE requests to the `/podcasts/{podcastSlug}` endpoint. The endpoint is secured using basic authentication.
The podcast slug is the slug of the podcast feed, e.g. `tiden` or `orientering`.

### Example
- POST https://dr-podcasts.azurewebsites.net/podcasts/tiden
- DELETE https://dr-podcasts.azurewebsites.net/podcasts/tiden

## Authentication
The system is using basic authentication for the `/podcasts/{podcastSlug}` endpoints. You get an Bearer token by calling the `/login` endpoint with the username and password as JSON in the body.

## Packages
The solution is using the following packages:
- **Azure.Security.KeyVault.Secrets:** For accessing the Azure Key Vault secrets which are used for the storing of passwords and other sensitive data.
- **BCrypt.Net-Next:** For hashing of passwords.
- **Hangfire.AspNetCore:** For background jobs.
- **MediatR:** For the mediator pattern.
- **MongoDB.Driver:** For the MongoDB database actions.
- **Swashbuckle.AspNetCore:** For the API documentation.


# Testing
There have been added unit tests for the fetcing of episodes to display how unit tests can be implemented in a project like this. The unit tests can be found in the following projects:
- DR.PodcastFeeds.Infrastructure.Tests
- DR.PodcastFeeds.Application.Tests
- DR.PodcastFeeds.Api.Tests

*There are no unit tests for the domain classes as they are just simple POCOs.*

## Packages
The solution is using the following testing frameworks:
- **xUnit:** For unit testing.
- **NSubstitute:** For mocking.
- **FluentAssertions:** For assertions.
- **Testcontainers.MongoDb:** For integration testing of the database (running on Docker).
