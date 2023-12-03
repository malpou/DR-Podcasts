<script lang="ts">
  import { goto } from "$app/navigation"

  export let link: string
  export let image: string
  export let title: string
</script>

<button on:click={() => goto(link)}>
  <img src={image} alt={title} />
</button>

<style>
  button {
    flex-direction: column;
    color: var(--black);
  }

  img {
    max-width: 25em;
    max-height: 25em;
  }

  b {
    margin-top: 0.25em;
    font-size: 1.25em;
  }
</style>
