<script lang="ts">
  export let selectedFilter: string
  export let onFilterChange: (filter: string) => void
  export let id: string
  export let text: string
</script>

<div>
  <input
    type="radio"
    {id}
    name="date"
    value={id}
    bind:group={selectedFilter}
    on:change={() => onFilterChange(id)}
  />
  <label for={id}>{text}</label>
</div>

<style>
  label {
    margin-right: 1em;
  }
</style>
