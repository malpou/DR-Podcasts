<script lang="ts">
  export let title: string
  export let slug: string | undefined
  export let subtitle: string | undefined
  export let audioUrl: string | undefined
  export let imageUrl: string

  import { goto } from "$app/navigation"
  import AudioPlayer from "./AudioPlayer.svelte"

  function handleClick() {
    if (slug) {
      goto(`podcast/${slug}`)
    }
  }

  $: style = !slug ? "cursor:auto;" : "cursor:pointer;"
  $: alt = title + `${subtitle ? " - " + subtitle : ""}`
</script>

<button {style} on:click={handleClick}>
  <img src={imageUrl} {alt} />
  <div>
    <h1>
      {title}
    </h1>
    {#if subtitle}
      <h2>
        {subtitle}
      </h2>
    {/if}
    {#if audioUrl}
      <AudioPlayer {audioUrl} />
    {/if}
  </div>
</button>

<style>
  button {
    margin-top: 1em;
    color: var(--black);
  }

  button img {
    max-width: 10em;
    max-height: 10em;
    margin-right: 1em;
  }

  div {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
  }

  h1 {
    margin-bottom: 0;
    text-align: left;
  }

  h2 {
    margin-top: 0;
    margin-bottom: 1.25em;
    text-align: left;
  }
</style>
