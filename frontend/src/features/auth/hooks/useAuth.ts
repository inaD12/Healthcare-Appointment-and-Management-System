"use client"

import { ROLES } from "@/features/users/types/userTypes"
import { AuthContext } from "@/providers/AuthProvider"
import { useContext, useMemo } from "react"

export function useAuth() {
  const context = useContext(AuthContext)

  if (!context) {
    throw new Error("useAuth must be used inside AuthProvider")
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