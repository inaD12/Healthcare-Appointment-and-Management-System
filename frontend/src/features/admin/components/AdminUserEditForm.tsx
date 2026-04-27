"use client"

import { useState } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"
import { useRouter } from "next/navigation"

import {
  updateUserByAdmin,
  deleteUserByAdmin,
} from "@/features/users/services/userService"

import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card"

import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"

import { Loader2, Trash2, User } from "lucide-react"

import {
  UpdateUserRequest,
  updateUserSchema,
} from "@/features/users/types/userTypes"

interface Props {
  userId: string
  defaultValues: UpdateUserRequest
}

export function AdminUserEditForm({ userId, defaultValues }: Props) {
  const router = useRouter()

  const [loading, setLoading] = useState(false)
  const [deleting, setDeleting] = useState(false)
  const [success, setSuccess] = useState(false)

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdateUserRequest>({
    resolver: zodResolver(updateUserSchema),
    defaultValues,
  })

  const onSubmit = async (data: UpdateUserRequest) => {
    setLoading(true)
    setSuccess(false)

    try {
      await updateUserByAdmin(data, userId)
      setSuccess(true)
      setTimeout(() => setSuccess(false), 3000)
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async () => {
    if (!confirm("Delete this user permanently?")) return

    setDeleting(true)

    try {
      await deleteUserByAdmin(userId)
      router.push("/admin/users")
    } finally {
      setDeleting(false)
    }
  }

  return (
    <div className="w-full space-y-8">

      <Card className="shadow-sm w-full">
        <CardHeader className="space-y-1">
          <div className="flex items-center gap-2">
            <User className="h-5 w-5 text-muted-foreground" />
            <CardTitle>Edit User</CardTitle>
          </div>

          <CardDescription>
            Update the user's information
          </CardDescription>
        </CardHeader>

        <CardContent>
          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">

            <div className="space-y-2">
              <Label>First Name</Label>
              <Input {...register("firstName")} />
              {errors.firstName && (
                <p className="text-sm text-red-500">
                  {errors.firstName.message}
                </p>
              )}
            </div>

            <div className="space-y-2">
              <Label>Last Name</Label>
              <Input {...register("lastName")} />
              {errors.lastName && (
                <p className="text-sm text-red-500">
                  {errors.lastName.message}
                </p>
              )}
            </div>

            {success && (
              <div className="rounded-md border border-green-200 bg-green-50 px-3 py-2 text-sm text-green-700">
                User updated successfully.
              </div>
            )}

            <div className="flex justify-end">
              <Button
                type="submit"
                className="w-auto px-6"
                disabled={loading}
              >
                {loading && (
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                )}
                {loading ? "Updating..." : "Update User"}
              </Button>
            </div>

          </form>
        </CardContent>
      </Card>

      <Card className="border-destructive/30 shadow-sm w-full">
        <CardHeader>
          <CardTitle className="text-destructive">
            Danger Zone
          </CardTitle>

          <CardDescription>
            Permanently delete this user. This action cannot be undone.
          </CardDescription>
        </CardHeader>

        <CardContent>
          <div className="flex justify-end">
            <Button
              variant="destructive"
              className="w-auto px-6"
              onClick={handleDelete}
              disabled={deleting}
            >
              {deleting ? (
                <>
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                  Deleting...
                </>
              ) : (
                <>
                  <Trash2 className="mr-2 h-4 w-4" />
                  Delete User
                </>
              )}
            </Button>
          </div>
        </CardContent>
      </Card>

    </div>
  )
}