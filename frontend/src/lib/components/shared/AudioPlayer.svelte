<script lang="ts">
  export let audioUrl: string
</script>

<audio controls>
  <source src={audioUrl} type="audio/mpeg" />
</audio>
