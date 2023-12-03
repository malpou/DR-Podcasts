const API_URL = "https://dr-podcasts.azurewebsites.net"

export async function fetchFromAPI<T>(endpoint: string): Promise<T> {
  const response = await fetch(`${API_URL}/${endpoint}`)
  if (!response.ok) {
    throw new Error("Network response was not ok")
  }
  return response.json() as Promise<T>
}

export async function login(password: string): Promise<string | null> {
  const response = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ username: "Admin", password }),
  })

  if (response.ok) {
    return response.json().then((data) => data.token)
  }

  return null
}

export async function addPodcast(podcastSlug: string, token: string): Promise<void> {
  const response = await fetch(`${API_URL}/podcasts/${podcastSlug}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    throw new Error("Network response was not ok")
  }
}

export async function removePodcast(
  podcastSlug: string,
  token: string
): Promise<void> {
  const response = await fetch(`${API_URL}/podcasts/${podcastSlug}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  })

  if (!response.ok) {
    throw new Error("Network response was not ok")
  }
}
