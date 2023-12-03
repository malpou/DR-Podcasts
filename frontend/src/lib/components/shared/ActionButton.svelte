<script lang="ts">
  export let text: string;
  export let disabled: boolean;
  export let color: "red" | "green";
  export let handleClick: () => void;
</script>

<button class={color} {disabled} on:click={handleClick}>{text}</button>

<style>
  button {
    color: white;
    padding: 10px;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    margin: 1em 0;
  }

  button:disabled {
    background-color: #cccccc;
    cursor: not-allowed;
  }

  button.green {
    background-color: #4caf50;
  }

  button.green:hover {
    background-color: #45a049;
  }

  button.red {
    background-color: #f44336;
  }

  button.red:hover {
    background-color: #da190b;
  }
</style>
