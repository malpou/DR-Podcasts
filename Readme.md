# DR Coding Test

## Assignment
The task is to build a REST API that exposes data from our existing podcast feeds at DR, with the following requirements. You are very welcome to justify any choices or rejections, in case you do not complete everything.
- It should be able to expose JSON data for a podcast feed based on a given name/id, e.g., "tiden" or "orientering".
- For a podcast feed, at a minimum, it should expose the title, description, link, category, and contained items (episodes).
- For each item, at a minimum, it should expose the id (guid), title, description, and publication date.
- It should be able to filter podcast feed items so that only items from a given publication date (pubDate) onwards are exposed.
- It should be able to limit how many items are exposed for the given podcast feed.
- We would like to see an example of a unit test.

### Examples of podcast feeds:
- https://api.dr.dk/podcasts/v1/feeds/tiden.xml?limit=500
- https://api.dr.dk/podcasts/v1/feeds/orientering.xml?limit=10

More podcasts can be found here: https://www.dr.dk/lyd, and you can find the actual podcast feed by clicking into a given podcast and selecting "RSS".


## Solution
The rest API is implemented in .Net 8 using C# and following the . The solution is divided into 5 source projects:
- DR.PodcastFeeds.Api: The REST API (built on minimal APIs)
- DR.PodcastFeeds.Domain: The domain class models
- DR.PodcastFeeds.Infrastructure: The infrastructure layer
- DR.PodcastFeeds.Contracts: The contracts for the REST API
- DR.PodcastFeeds.Application: The application layer ()
