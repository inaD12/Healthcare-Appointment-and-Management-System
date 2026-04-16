import { useContext, useEffect } from "react"
import { AuthContext } from "@/providers/AuthProvider"

export function useAuthGuard() {
  const auth = useContext(AuthContext)

  useEffect(() => {
    if (!auth) return

    if (!auth.authenticated) {
      auth.keycloak.login()
    }
  }, [auth])

  if (!auth) {
    throw new Error("useAuthGuard must be used within AuthProvider")
  }

  return auth
}