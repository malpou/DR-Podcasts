<script lang="ts">
  import moment from "moment"
  import AudioPlayer from "../shared/AudioPlayer.svelte"

  export let episode: Episode

  $: timeAgo = moment(episode.published).fromNow()
  $: published = moment(episode.published).format("ll")
</script>

<h3>{episode.title}</h3>
<p>{episode.description}</p>
<div>
  <AudioPlayer audioUrl={episode.audioUrl} />
</div>
<b>Published {timeAgo} on {published}</b>

<style>
  div {
    display: flex;
    justify-content: center;
    margin: 1em;
  }
</style>
