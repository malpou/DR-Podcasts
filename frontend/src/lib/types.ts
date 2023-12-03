type Category = {
  name: string
  slug: string
  picked: boolean
}

type Podcast = {
  title: string
  description: string
  slug: string
  imageUrl: string
  category: string
}

type PodcastEpisodes = {
  podcastTitle: string
  podcastDescription: string
  podcastImageUrl: string
  podcastCategory: string
  podcastLink: string
  episodes: Episode[]
}

type Episode = {
  title: string
  published: string
  description: string
  audioUrl: string
  podcastTitle: string | null
  podcastImageUrl: string | null
}

type Token = {
  token: string
}