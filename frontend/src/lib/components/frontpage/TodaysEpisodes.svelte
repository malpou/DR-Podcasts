<script lang="ts">
  import CoverCard from "../shared/CoverCard.svelte"

  export let episodes: Episode[]
</script>

{#each episodes as episode}
  <CoverCard
    title={episode.podcastTitle || ""}
    subtitle={episode.title}
    audioUrl={episode.audioUrl}
    imageUrl={episode.podcastImageUrl || ""}
    slug={undefined}
  />
{/each}
