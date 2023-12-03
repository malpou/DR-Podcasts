<script lang="ts">
  import Page from "./Page.svelte"

  export let isPodcasts: boolean = true
  export let selectPage: (page: "podcasts" | "todays") => void
</script>

<div>
  <Page
    text="Podcasts"
    isActive={isPodcasts}
    clickHandler={() => selectPage("podcasts")}
  />
  <Page
    text="Todays Episodes"
    isActive={!isPodcasts}
    clickHandler={() => selectPage("todays")}
  />
</div>

<style>
  div {
    display: flex;
    justify-content: space-evenly;
  }
</style>
