<script lang="ts">
  export let text: string
  export let isActive: boolean
  export let clickHandler: () => void

  $: style = `color: ${isActive ? "var(--red)" : "var(--black)"};`
</script>

<button {style} on:click={clickHandler}><h1>{text}</h1></button>
