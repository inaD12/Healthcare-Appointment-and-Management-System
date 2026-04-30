import axios from "axios"

const isServer = typeof window === "undefined"

export const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
})

api.interceptors.request.use(async (config) => {
  let token: string | undefined

  if (isServer) {
    const { cookies } = await import("next/headers")
    const cookieStore = await cookies()
    token = cookieStore.get("access_token")?.value
  } else {
    const { default: keycloak } = await import("@/config/keycloak")
    token = keycloak.token
  }

  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }

  return config
})