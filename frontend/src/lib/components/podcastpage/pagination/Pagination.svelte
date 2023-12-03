<script lang="ts">
  import PaginationButton from "./PaginationButton.svelte"

  export let currentPage: number
  export let totalPages: number
  export let onChangePage: (page: number) => void

  function changePage(page: number) {
    currentPage = page
    onChangePage(page)
  }

  let isFirstPage = true
  let isLastPage = false

  $: isFirstPage = currentPage <= 1
  $: isLastPage = currentPage >= totalPages
</script>

<div>
  <PaginationButton
    text="Previous"
    disabled={isFirstPage}
    onClick={() => changePage(currentPage - 1)}
  />
  <span>Page {currentPage} of {totalPages}</span>

  <PaginationButton
    text="Next"
    disabled={isLastPage}
    onClick={() => changePage(currentPage + 1)}
  />
</div>

<style>
  div {
    display: flex;
    justify-content: center;
    align-items: center;
    margin: 20px 0;
  }
</style>
