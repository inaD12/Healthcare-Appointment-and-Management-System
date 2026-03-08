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

  return auth
}