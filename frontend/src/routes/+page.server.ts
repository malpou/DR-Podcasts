import { fetchFromAPI } from "$lib/api"
import { addAllCategory } from "$lib/utils"

export async function load({ url }): Promise<{
  categories: Category[] | null
  podcasts: Podcast[] | null
  episodes: Episode[] | null
  pickedCategory: Category | null
}> {
  const page = url.searchParams.get("p")

  if (page === "todays") {
    try {
      const today = new Date().toISOString().slice(0, 10)
      const episodes = await fetchFromAPI<Episode[]>(
        `episodes?from=${today}&to=${today}`
      )
      return {
        categories: null,
        podcasts: null,
        episodes,
        pickedCategory: null,
      }
    } catch (error) {
      return {
        categories: null,
        podcasts: null,
        episodes: null,
        pickedCategory: null,
      }
    }
  }

  let categories = await fetchFromAPI<Category[]>("categories")
  categories = addAllCategory(categories)

  const categoryQuery = url.searchParams.get("c")
  const category = categories.find((c) => c.slug === categoryQuery)

  if (category) {
    const podcasts = await fetchFromAPI<Podcast[]>(
      `categories/${category.slug}/podcasts`
    )
    categories = categories.map((c) => ({
      ...c,
      picked: c.slug === categoryQuery,
    }))
    return { categories, podcasts, pickedCategory: category, episodes: null }
  }

  categories = categories.map((c) => ({ ...c, picked: c.name === "All" }))
  const podcasts = await fetchFromAPI<Podcast[]>("podcasts")
  return { categories, podcasts, pickedCategory: null, episodes: null }
}
