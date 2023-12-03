<script lang="ts">
  export let text: string
  export let disabled: boolean
  export let onClick: () => void

  function handleClick() {
    if (!disabled) {
      onClick()
    }
  }

  $: style = disabled ? "display: none;" : ""
</script>

<button {style} on:click={handleClick} {disabled}>
  <h2>{text}</h2>
</button>

<style>
  button {
    margin: 0 0.75em;
    color: var(--black);
    background-color: var(--white);
    border: 1px solid var(--black);
    border-radius: 1em;
    padding: 0.25em 1.5em;
    cursor: pointer;
  }
</style>
