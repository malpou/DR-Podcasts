<script lang="ts">
  import CategoryComp from "./Category.svelte"

  export let categories: Category[]
  export let onCategoryChange: (category: string) => void
</script>

<h2>Categories</h2>
<div>
  {#each categories as category}
    <CategoryComp {category} {onCategoryChange} />
  {/each}
</div>

<style>
  div {
    display: flex;
    justify-content: space-around;
    flex-wrap: wrap;
  }
</style>
