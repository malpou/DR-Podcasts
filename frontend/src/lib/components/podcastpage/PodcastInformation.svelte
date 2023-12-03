<script lang="ts">
  import PodcastCover from "./PodcastCover.svelte"

  export let podcastEpisodes: PodcastEpisodes
</script>

<div>
  <PodcastCover
    link={podcastEpisodes.podcastLink}
    image={podcastEpisodes.podcastImageUrl}
    title={podcastEpisodes.podcastTitle}
  />
  <h1>{podcastEpisodes.podcastTitle}</h1>
  <p>{@html podcastEpisodes.podcastDescription}</p>
</div>

<style>
  div {
    display: flex;
    flex-direction: column;
    align-items: center;
  }

  h1 {
    margin-top: 0;
  }

  p {
    max-width: 50em;
  }
</style>
