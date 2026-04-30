import { useEffect } from "react"
import { useRouter } from "next/navigation"
import { useAuthGuard } from "./useAuthGuard"
import { ROLES } from "@/features/users/types/usersTypes"

type Role = typeof ROLES[keyof typeof ROLES]

export function useRequireRole(requiredRole: Role) {
  const auth = useAuthGuard()
  const router = useRouter()

  const hasRole = auth.roles.includes(requiredRole)

  useEffect(() => {
    if (!auth.ready) return

    if (!hasRole) {
      router.replace("/")
    }
  }, [auth.ready, auth.authenticated, hasRole, router])

  if (!auth) {
    throw new Error("useRequireRole must be used within AuthProvider")
  }

  return auth
}