export const setCookie = (name: string, value: string, seconds: number) => {
  let expires = ""
  if (seconds) {
    let date = new Date()
    date.setTime(date.getTime() + seconds * 1000)
    expires = "; expires=" + date.toUTCString()
  }
  document.cookie = name + "=" + (value || "") + expires + "; path=/"
}

export const getCookie = (name: string) => {
  let nameEQ = name + "="
  let ca = document.cookie.split(";")
  for (var i = 0; i < ca.length; i++) {
    let c = ca[i]
    while (c.charAt(0) == " ") c = c.substring(1, c.length)
    if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length)
  }
  return null
}
