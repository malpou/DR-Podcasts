## Functionality

### Endpoints
- Get categories
    - `GET /categories`
- Discovery endpoints that shows what podcasts are available in the system
    - `GET /podcasts` - list all podcasts
    -  Get podcasts by category id or slug
        - `GET /categories/{slug/id}/podcasts`
- List all episodes for a given podcast based on a podcast id or slug
    - `GET /podcasts/{slug/id}/episodes`
    - This endpoint should support date filtering (iso format YYYY-MM-DD)
      - `GET /podcasts/{slug/id}/episodes?from=2020-01-01&to=2020-01-31`
    - This endpoint should support receiving the last N episodes
      - `GET /podcasts/{slug/id}/episodes?last=10`
    - This endpoint should support pagination
      - `GET /podcasts/{slug/id}/episodes?page=0&size=10`
- Admin endpoints that allows to add/remove rss podcast feeds to the system (authentication required)
  - `POST /feed?rssUrl=https://api.dr.dk/podcasts/v1/feeds/orientering` - add new podcast (needs to validate that the rss feed is valid)
  - `DELETE /feed?rssUrl=https://api.dr.dk/podcasts/v1/feeds/orientering` - remove podcast


### Timed jobs
- Fetching new episodes from the rss feeds in the system every 15 minutes