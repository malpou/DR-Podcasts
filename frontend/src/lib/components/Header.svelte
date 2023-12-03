<script lang="ts">
  import { goto } from "$app/navigation"
  import { page } from "$app/stores"
  import Logo from "../../logo.jpg"

  $: isHome = $page.url.pathname === "/"
  $: isConfig = $page.url.pathname === "/config"
  
  $: style = (isOnRoute: boolean) => isOnRoute ? "pointer-events: none;" : ""
</script>

<nav>
  <button style={style(isHome)} disabled={isHome} on:click={() => goto("/")}
    ><img src={Logo} alt="DR Lyd Logo" />
    <h1>Podcasts</h1></button
  >
  <button style={style(isConfig)} disabled={isConfig} on:click={() => goto("/config")}
    ><h3>Configuration</h3></button
  >
</nav>

<style>
  nav {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 1em;
    background-color: var(--white);
  }

  button {
    margin-bottom: 3em;
    color: var(--black) !important;
  }

  img {
    max-width: 5em;
    max-height: 5em;
    margin-right: 1em;
  }
</style>
