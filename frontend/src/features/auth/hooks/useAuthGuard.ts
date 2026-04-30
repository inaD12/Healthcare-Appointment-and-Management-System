import { useContext, useEffect, useMemo } from "react"
import { AuthContext } from "@/providers/AuthProvider"
import { ROLES } from "@/features/users/types/usersTypes"

export function useAuthGuard() {
  const context = useContext(AuthContext)

  useEffect(() => {
    if (!context) return

    if (!context.authenticated) {
      context.keycloak.login()
    }
  }, [context])

  if (!context) {
    throw new Error("useAuthGuard must be used within AuthProvider")
  }

  const { roles } = context
  
    const roleFlags = useMemo(() => {
      return {
        isPatient: roles.includes(ROLES.PATIENT),
        isAdmin: roles.includes(ROLES.ADMIN),
        isDoctor: roles.includes(ROLES.DOCTOR),
      }
    }, [roles])
  
    return {
      ...context,
      ...roleFlags,
    }
}