"use client"
import { createContext, useEffect, useState } from "react"
import keycloak from "../config/keycloak"
import { getCurrentUser } from "@/features/users/services/userService"
import { UserQueryResponse } from "@/features/users/types/userTypes"

type AuthContextType = {
  keycloak: typeof keycloak
  authenticated: boolean
  token: string | null
  user: UserQueryResponse | null
  roles: string[]
  ready: boolean
}

export const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [authenticated, setAuthenticated] = useState(false)
  const [token, setToken] = useState<string | null>(null)
  const [user, setUser] = useState<UserQueryResponse | null>(null)
  const [roles, setRoles] = useState<string[]>([])
  const [ready, setReady] = useState(false)

  const setAccessTokenCookie = (token: string) => {
    document.cookie = `access_token=${token}; path=/; SameSite=Strict`
  }

  const clearAccessTokenCookie = () => {
    document.cookie = "access_token=; Max-Age=0; path=/"
  }

  useEffect(() => {
    keycloak
      .init({
        onLoad: "check-sso",
        pkceMethod: "S256",
        silentCheckSsoRedirectUri: window.location.origin + "/silent-check-sso.html",
      })
      .then(async (auth) => {
        setAuthenticated(auth)
        setToken(keycloak.token ?? null)

        if (auth && keycloak.token) {
          setAccessTokenCookie(keycloak.token)
          try {
            const res = await getCurrentUser()
            const currentUser = res.data.data
            setUser(currentUser)
            setRoles(currentUser.roles)
          } catch (err) {
            console.error("Failed to fetch user", err)
          }
        } else {
          clearAccessTokenCookie()
        }

        setReady(true)

        const interval = setInterval(() => {
          keycloak.updateToken(60).then((refreshed) => {
            if (refreshed && keycloak.token) {
              setToken(keycloak.token)
              setAccessTokenCookie(keycloak.token)
            }
          })
        }, 60000)

        return () => clearInterval(interval)
      })
  }, [])

  if (!ready) return <div>Loading...</div>

  return (
    <AuthContext.Provider value={{ keycloak, authenticated, token, user, roles, ready }}>
      {children}
    </AuthContext.Provider>
  )
}