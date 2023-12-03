<script lang="ts">
  import { goto } from "$app/navigation"
  import PodcastEpisodes from "$lib/components/podcastpage/PodcastEpisodes.svelte"
  import PodcastInformation from "$lib/components/podcastpage/PodcastInformation.svelte"
  import PodcastFilters from "$lib/components/podcastpage/filters/PodcastFilters.svelte"
  import { onMount } from "svelte"
  export let data

  let selectedFilter = "newest"
  let currentPage = 1
  const totalPages = data.totalPages

  onMount(() => {
    const urlParams = new URLSearchParams(window.location.search)
    selectedFilter = urlParams.get("f") || "newest"
    currentPage = parseInt(urlParams.get("p")!) || 1

    handleFilterChange(selectedFilter)
  })

  function handleFilterChange(newFilter: string) {
    selectedFilter = newFilter
    if (selectedFilter === "paginated") {
      goto(`?f=paginated&p=${currentPage}`)
    } else {
      goto(`?f=${selectedFilter}`)
    }
  }

  function changePage(page: number) {
    currentPage = page
    handleFilterChange(selectedFilter)
  }

  $: isPagination = selectedFilter === "paginated"
</script>

<head>
  <title>DR Podcasts - {data.podcastEpisodes.podcastTitle}</title>
</head>

<PodcastInformation podcastEpisodes={data.podcastEpisodes} />

<PodcastFilters {selectedFilter} onFilterChange={handleFilterChange} />
{#if data.podcastEpisodes.episodes.length === 0}
  <hr />
  <h2>No episodes found for this filter...</h2>
{:else}
  <PodcastEpisodes
    {isPagination}
    {currentPage}
    {totalPages}
    onChangePage={changePage}
    episodes={data.podcastEpisodes.episodes}
  />
{/if}
