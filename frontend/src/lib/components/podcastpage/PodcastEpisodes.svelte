<script lang="ts">
  import Pagination from "./pagination/Pagination.svelte"
  import PodcastEpisode from "./PodcastEpisode.svelte"

  export let isPagination: boolean
  export let currentPage: number
  export let totalPages: number
  export let onChangePage: (page: number) => void
  export let episodes: Episode[]
</script>

{#if isPagination}
  <hr />
  <Pagination {currentPage} {totalPages} {onChangePage} />
{/if}

{#each episodes as episode}
  <hr />
  <PodcastEpisode {episode} />
{/each}

{#if isPagination}
  <hr />
  <Pagination {currentPage} {totalPages} {onChangePage} />
{/if}
