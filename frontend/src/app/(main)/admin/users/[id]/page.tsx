"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"


import {
  UserQueryResponse,
  UpdateUserRequest,
} from "@/features/users/types/userTypes"

import {
  getUserByAdmin,
} from "@/features/users/services/userService"
import { AdminUserEditForm } from "@/components/admin/AdminUserEditForm"

export default function AdminUserPage() {
  const params = useParams()
  const id = params.id as string

  const [user, setUser] = useState<UserQueryResponse | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const res = await getUserByAdmin(id)
        setUser(res.data.data)
      } catch {
        setUser(null)
      } finally {
        setLoading(false)
      }
    }

    fetchUser()
  }, [id])

  if (loading) {
    return <div className="p-8">Loading user...</div>
  }

  if (!user) {
    return (
      <div className="p-8 text-center text-muted-foreground">
        User not found
      </div>
    )
  }

  return (
    <div className="p-8 max-w-3xl mx-auto space-y-6">

      <Card>
        <CardHeader>
          <CardTitle>User Details</CardTitle>
        </CardHeader>

        <CardContent className="space-y-4">

          <div>
            <p className="text-sm text-muted-foreground">ID</p>
            <p className="font-mono text-sm">{user.id}</p>
          </div>

          <div>
            <p className="text-sm text-muted-foreground">Email</p>
            <p>{user.email}</p>
          </div>

          <div>
            <p className="text-sm text-muted-foreground">Roles</p>
            <div className="flex gap-2 flex-wrap mt-1">
              {user.roles.map((role) => (
                <Badge key={role} variant="secondary">
                  {role}
                </Badge>
              ))}
            </div>
          </div>

          <div>
            <p className="text-sm text-muted-foreground">Email Verified</p>

            {user.emailVerified ? (
              <Badge className="bg-green-500">Verified</Badge>
            ) : (
              <Badge variant="destructive">Not Verified</Badge>
            )}
          </div>

        </CardContent>
      </Card>

      <Card>
        <CardContent>
          <AdminUserEditForm
            userId={user.id}
            defaultValues={{
                firstName: user.firstName,
                lastName: user.lastName,
            }}
            />
        </CardContent>
      </Card>

    </div>
  )
}