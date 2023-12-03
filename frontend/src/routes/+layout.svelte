<script lang="ts">
  import Footer from "$lib/components/Footer.svelte"
  import Header from "$lib/components/Header.svelte"
  import "../globals.css"
</script>

<main>
  <Header />
  <slot />
  <Footer />
</main>

<style>
  main {
    padding: 1em;
  }
</style>