"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Users, Stethoscope } from "lucide-react"

interface Props {
  roles: string[]
}

export default function QuickActions({ roles }: Props) {
  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  return (
    <div className="p-6 flex flex-wrap gap-3">

      {isPatient && (
        <>
          <Button asChild>
            <Link href="/patient-info">
              <Users className="mr-2 h-4 w-4" />
              View Profile
            </Link>
          </Button>

          <Button asChild>
            <Link href="/doctors">
              <Stethoscope className="mr-2 h-4 w-4" />
              Browse Doctors
            </Link>
          </Button>

          <Button asChild>
            <Link href="/settings">
              Settings
            </Link>
          </Button>
        </>
      )}

      {isDoctor && (
        <>
          <Button asChild>
            <Link href="/doctors/profile">
              <Users className="mr-2 h-4 w-4" />
              View Profile
            </Link>
          </Button>

          <Button asChild>
            <Link href="/settings">
              Settings
            </Link>
          </Button>
        </>
      )}

      {isAdmin && (
        <>
          <Button asChild>
            <Link href="/admin/users">
              Manage Users
            </Link>
          </Button>

          <Button asChild>
            <Link href="/admin/users/create">
              Create User
            </Link>
          </Button>

          <Button asChild>
            <Link href="/settings">
              Settings
            </Link>
          </Button>
        </>
      )}

    </div>
  )
}