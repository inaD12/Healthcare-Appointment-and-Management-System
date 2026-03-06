"use client"

import { createContext, useEffect, useState } from "react"
import keycloak from "../config/keycloak"

export const AuthContext = createContext<any>(null)

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [authenticated, setAuthenticated] = useState(false)
  const [token, setToken] = useState<string | null>(null)

  useEffect(() => {
    keycloak
      .init({
        onLoad: "login-required",
        pkceMethod: "S256",
      })
      .then((auth) => {
        setAuthenticated(auth)
        setToken(keycloak.token ?? null)
      })
  }, [])

  return (
    <AuthContext.Provider value={{ keycloak, authenticated, token }}>
      {children}
    </AuthContext.Provider>
  )
}