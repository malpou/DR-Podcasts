<script lang="ts">
  import { goto } from "$app/navigation"
  import PagePicker from "$lib/components/frontpage/pagepicker/PagePicker.svelte"
  import Podcasts from "$lib/components/frontpage/Podcasts.svelte"
  import TodaysEpisodes from "$lib/components/frontpage/TodaysEpisodes.svelte"
  import { onMount } from "svelte"

  export let data
  let category = "all"

  let isPodcasts = true

  function selectCategory(slug: string) {
    category = slug

    if (slug === "all") {
      goto(`/`)
      return
    }

    goto(`?c=${slug}`)
  }

  function selectPage(page: "podcasts" | "todays") {
    if (page === "podcasts") {
      isPodcasts = true
      if (category === "all") goto(`/`)
      else goto(`?c=${category}`)
    } else {
      isPodcasts = false
      goto(`?p=${page}`)
    }
  }

  onMount(() => {
    const urlParams = new URLSearchParams(window.location.search)
    const pickedCategory = urlParams.get("c")
    const pickedPage = urlParams.get("p")

    if (pickedCategory) {
      category = pickedCategory
    }

    if (pickedPage && pickedPage === "todays") {
      isPodcasts = false
    }
  })

  $: title = `DR Podcasts${
    isPodcasts
      ? data.pickedCategory
        ? ` - ${data.pickedCategory.name}`
        : ""
      : " - Todays Episodes"
  }`
</script>

<head>
  <title>{title}</title>
</head>

<PagePicker {isPodcasts} {selectPage} />

{#if isPodcasts === true && data.podcasts && data.categories}
  <Podcasts
    podcasts={data.podcasts}
    categories={data.categories}
    onCategoryChange={selectCategory}
  />
{:else if isPodcasts === false}
  {#if data.episodes && data.episodes.length > 0}
    <TodaysEpisodes episodes={data.episodes} />
  {:else}
    <h3>No podcasts released today...</h3>
  {/if}
{/if}
