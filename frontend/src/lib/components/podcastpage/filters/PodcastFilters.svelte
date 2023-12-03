<script lang="ts">
  import FilterButton from "./FilterButton.svelte"
  export let selectedFilter: string
  export let onFilterChange: (filter: string) => void
</script>

<h3>Filters</h3>
<div>
  <FilterButton
    {selectedFilter}
    {onFilterChange}
    id="newest"
    text="Newest episodes"
  />
  <FilterButton
    {selectedFilter}
    {onFilterChange}
    id="last-week"
    text="Last week"
  />
  <FilterButton
    {selectedFilter}
    {onFilterChange}
    id="last-month"
    text="Last month"
  />
  <FilterButton
    {selectedFilter}
    {onFilterChange}
    id="paginated"
    text="Pagination"
  />
  <FilterButton {selectedFilter} {onFilterChange} id="all" text="All" />
</div>

<style>
  div {
    display: flex;
    justify-content: space-evenly;
    flex-wrap: wrap;
    margin: 0.5em;
  }

  h3 {
    margin: 0;
  }
</style>
