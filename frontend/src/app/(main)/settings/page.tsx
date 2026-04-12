"use client"

import { useEffect, useState } from "react"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"

import {
  updateCurrentUser,
  deleteCurrentUser,
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

import {
  UpdateCurrentUserRequest,
  updateCurrentUserSchema,
} from "@/features/users/types/userTypes"

import { useAuth } from "@/features/auth/hooks/useAuth"

import { Loader2, Trash2, User } from "lucide-react"

export default function AccountPage() {
  const auth = useAuth()
  const user = auth?.user

  const [loading, setLoading] = useState(false)
  const [deleting, setDeleting] = useState(false)
  const [success, setSuccess] = useState(false)

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UpdateCurrentUserRequest>({
    resolver: zodResolver(updateCurrentUserSchema),
  })

  useEffect(() => {
    if (user) {
      reset({
        firstName: user.firstName,
        lastName: user.lastName,
      })
    }
  }, [user, reset])

  const onSubmit = async (data: UpdateCurrentUserRequest) => {
    setLoading(true)
    setSuccess(false)

    try {
        await updateCurrentUser(data)
        setSuccess(true)

        setTimeout(() => {
        setSuccess(false)
        }, 3000)
    } finally {
        setLoading(false)
    }
    }

  const handleDelete = async () => {
    if (!confirm("Delete your account permanently?")) return

    setDeleting(true)

    try {
      await deleteCurrentUser()
    } finally {
      setDeleting(false)
    }
  }

  return (
    <div className="flex min-h-[80vh] items-center justify-center bg-muted/30 px-6 py-10">

      <div className="w-full max-w-lg space-y-8">

        <Card className="shadow-sm">
          <CardHeader className="space-y-1">
            <div className="flex items-center gap-2">
              <User className="h-5 w-5 text-muted-foreground" />
              <CardTitle>Account Settings</CardTitle>
            </div>
            <CardDescription>
              Manage your personal information
            </CardDescription>
          </CardHeader>

          <CardContent>
            <form
              onSubmit={handleSubmit(onSubmit)}
              className="space-y-5"
            >
              <div className="space-y-2">
                <Label>First Name</Label>
                <Input
                  {...register("firstName")}
                  placeholder="John"
                />
                {errors.firstName && (
                  <p className="text-sm text-red-500">
                    {errors.firstName.message}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label>Last Name</Label>
                <Input
                  {...register("lastName")}
                  placeholder="Doe"
                />
                {errors.lastName && (
                  <p className="text-sm text-red-500">
                    {errors.lastName.message}
                  </p>
                )}
              </div>

                {success && (
                    <div className="rounded-md border border-green-200 bg-green-50 px-3 py-2 text-sm text-green-700">
                        Profile updated successfully.
                    </div>
                )}

              <Button
                type="submit"
                className="w-full"
                disabled={loading}
              >
                {loading && (
                  <Loader2 className="mr-2 h-4 w-4 animate-spin" />
                )}
                {loading ? "Updating..." : "Update Profile"}
              </Button>
            </form>
          </CardContent>
        </Card>

        <Card className="border-destructive/30 shadow-sm">
          <CardHeader>
            <CardTitle className="text-destructive">
              Danger Zone
            </CardTitle>
            <CardDescription>
              Permanently delete your account. This action cannot be undone.
            </CardDescription>
          </CardHeader>

          <CardContent>
            <Button
              variant="destructive"
              className="w-full"
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
                  Delete Account
                </>
              )}
            </Button>
          </CardContent>
        </Card>

      </div>

    </div>
  )
}