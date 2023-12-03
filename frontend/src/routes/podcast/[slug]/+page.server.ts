import { fetchFromAPI } from "$lib/api"
import { calculateTotalPages, getFormattedDate } from "$lib/utils"

export async function load({
  params,
  url,
}): Promise<{ podcastEpisodes: PodcastEpisodes; totalPages: number }> {
  let endpoint = `podcasts/${params.slug}/episodes`
  const filter = url.searchParams.get("f")
  const page = url.searchParams.get("p") || 1
  const size = 5

  const totalEpisodesResponse = await fetchFromAPI<PodcastEpisodes>(endpoint)
  const totalPages = calculateTotalPages(
    totalEpisodesResponse.episodes.length,
    size
  )

  endpoint
  switch (filter) {
    case "last-week":
      endpoint += `?from=${getFormattedDate(7)}&to=${getFormattedDate()}`
      break
    case "last-month":
      endpoint += `?from=${getFormattedDate(30)}&to=${getFormattedDate()}`
      break
    case "paginated":
      endpoint += `?page=${page}&size=${size}`
      break
    case "newest":
      endpoint += `?last=3`
      break
    default:
      break
  }

  try {
    const podcastEpisodes = await fetchFromAPI<PodcastEpisodes>(endpoint)

    return { podcastEpisodes, totalPages }
  } catch (error) {
    return {
      podcastEpisodes: {
        episodes: [],
        podcastCategory: totalEpisodesResponse.podcastCategory,
        podcastTitle: totalEpisodesResponse.podcastTitle,
        podcastDescription: totalEpisodesResponse.podcastDescription,
        podcastImageUrl: totalEpisodesResponse.podcastImageUrl,
        podcastLink: totalEpisodesResponse.podcastLink,
      },
      totalPages: totalPages,
    }
  }
}
