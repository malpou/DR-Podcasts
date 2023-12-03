<script lang="ts">
  export let category: Category
  export let onCategoryChange: (category: string) => void

  $: style = `color: ${category.picked ? "var(--red)" : "var(--black)"};`
</script>

<button {style} on:click={() => onCategoryChange(category.slug)}>
  <h2>{category.name}</h2></button
>

<style>
  button {
    margin-left: 2em;
  }
</style>
