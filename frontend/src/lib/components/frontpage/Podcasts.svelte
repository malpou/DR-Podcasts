<script lang="ts">
  import CategoryPicker from "./categorypicker/CategoryPicker.svelte"
  import CoverCard from "../shared/CoverCard.svelte"

  export let categories: Category[]
  export let podcasts: Podcast[]
  export let onCategoryChange: (category: string) => void
</script>

<CategoryPicker {categories} {onCategoryChange} />

<h2>Podcasts</h2>
{#each podcasts as podcast}
  <CoverCard
    title={podcast.title}
    subtitle={undefined}
    audioUrl={undefined}
    imageUrl={podcast.imageUrl}
    slug={podcast.slug}
  />
{/each}
