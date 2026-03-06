"use client"

import { AuthContext } from "@/src/providers/AuthProvider"
import { useContext } from "react"

export function useAuth() {
  return useContext(AuthContext)
}