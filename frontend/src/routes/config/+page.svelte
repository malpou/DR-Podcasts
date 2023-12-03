<script lang="ts">
  import { addPodcast, fetchFromAPI, login, removePodcast } from "$lib/api.js"
  import ActionButton from "$lib/components/shared/ActionButton.svelte"
  import CoverCard from "$lib/components/shared/CoverCard.svelte"
  import { getCookie, setCookie } from "$lib/cookies.js"
  import { onMount } from "svelte"
  import { writable } from "svelte/store"

  let password = ""
  let newPodcastName = ""
  let loading = false
  let podcasts = writable([] as Podcast[])

  let bearerToken = writable("")

  const handleLogin = async () => {
    loading = true
    var token = await login(password)
    

    if (token) {
      bearerToken.set(token)
      const payload = JSON.parse(atob(token.split(".")[1]))
      const expirySeconds = payload.exp - payload.iat
      setCookie("jwt", token, expirySeconds)
      password = ""
    } else {
      alert("Wrong password")
    }
    loading = false
  }

  const handleLogout = () => {
    bearerToken.set("")
    setCookie("jwt", "", 0)
  }

  onMount(async () => {
    const token = getCookie("jwt")
    if (token) {
      bearerToken.set(token)
    }

    podcasts.set(await fetchFromAPI<Podcast[]>("podcasts"))
  })

  const removeHandler = async (slug: string) => {
    console.log("removeHandler", slug)
    loading = true
    try {
      await removePodcast(slug, $bearerToken)
      await waitAndRefresh()
    } catch (error) {
      alert(
        "Something went wrong when removing the podcast either it does not exist or something else went wrong"
      )
    }

    loading = false
  }

  const addHandler = async () => {
    loading = true

    try {
      await addPodcast(newPodcastName, $bearerToken)
      await waitAndRefresh()
      newPodcastName = ""
    } catch (error) {
      alert(
        "Something went wrong when adding the podcast either it already exist, name is invalid in DRs system or something else went wrong"
      )
    }

    loading = false
  }

  const waitAndRefresh = async () => {
    await new Promise((resolve) => setTimeout(resolve, 2500))
    location.reload()
  }
</script>

<head>
  <title>DR Podcasts - Configuration</title>
</head>

<h2>Configuration Page</h2>

{#if $bearerToken}
  <h4>Add new podcast</h4>
  <p>
    Enter the name (ex. absolut-mathias-helt) of the podcast you want to add
    (needs to be valid in DRs systems)
  </p>
  <input type="text" bind:value={newPodcastName} />
  <div style="display: flex; justify-content: space-between;">
    <ActionButton
      text="Add"
      disabled={loading}
      color="green"
      handleClick={addHandler}
    />
    <ActionButton
      text="Logout"
      disabled={loading}
      color="red"
      handleClick={handleLogout}
    />
  </div>

  <h3>Current podcasts</h3>
  <hr />
  {#each $podcasts as podcast}
    <div>
      <CoverCard
        title={podcast.title}
        subtitle={undefined}
        audioUrl={undefined}
        imageUrl={podcast.imageUrl}
        slug={undefined}
      />
      <ActionButton
        text="Remove"
        disabled={loading}
        color="red"
        handleClick={() => removeHandler(podcast.slug)}
      />
    </div>
    <hr />
  {/each}
{:else}
  <h4>Enter password for 'Admin' user</h4>
  <input type="password" bind:value={password} />
  <ActionButton
    text="Login"
    disabled={loading}
    color="green"
    handleClick={handleLogin}
  />
{/if}

<style>
  input {
    width: 100%;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 4px;
    box-sizing: border-box;
    margin-bottom: 10px;
  }

  h2 {
    margin-bottom: 20px;
  }

  h4 {
    margin-bottom: 10px;
  }
</style>
