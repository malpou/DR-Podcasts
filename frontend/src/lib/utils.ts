export function calculateTotalPages(
  totalItems: number,
  pageSize: number
): number {
  return Math.ceil(totalItems / pageSize)
}

export function getFormattedDate(daysAgo: number = 0): string {
  const date = new Date()
  date.setDate(date.getDate() - daysAgo)
  return date.toISOString().split("T")[0]
}


export function addAllCategory(categories: Category[]) {
  return [{ name: "All", slug: "all", picked: false }, ...categories]
}