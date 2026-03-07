"use client"

import { createContext, useEffect, useState } from "react"
import keycloak from "../config/keycloak"

type AuthContextType = {
  keycloak: typeof keycloak
  authenticated: boolean
  token: string | null
}

export const AuthContext = createContext<AuthContextType | null>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [authenticated, setAuthenticated] = useState(false)
  const [token, setToken] = useState<string | null>(null)
  const [ready, setReady] = useState(false)

  useEffect(() => {
    keycloak
      .init({
        onLoad: "check-sso",
        pkceMethod: "S256",
        silentCheckSsoRedirectUri:
          window.location.origin + "/silent-check-sso.html",
      })
      .then((auth) => {
        setAuthenticated(auth)
        setToken(keycloak.token ?? null)
        setReady(true)

        setInterval(() => {
          keycloak.updateToken(60).then((refreshed) => {
            if (refreshed) {
              setToken(keycloak.token ?? null)
            }
          })
        }, 60000)
      })
  }, [])

  if (!ready) return <div>Loading...</div>

  return (
    <AuthContext.Provider value={{ keycloak, authenticated, token }}>
      {children}
    </AuthContext.Provider>
  )
}