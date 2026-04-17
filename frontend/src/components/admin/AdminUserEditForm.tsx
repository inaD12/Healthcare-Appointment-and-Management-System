"use client"

import { useForm } from "react-hook-form"
import { Form } from "@/components/ui/form"
import { zodResolver } from "@hookform/resolvers/zod"

import { updateUserByAdmin } from "@/features/users/services/userService"

import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { UpdateUserRequest, updateUserSchema } from "@/features/users/types/userTypes"
import { FormField, FormItem, FormLabel, FormControl, FormMessage } from "../ui/form"

interface Props {
  userId: string
  defaultValues: UpdateUserRequest
}

export function AdminUserEditForm({ userId, defaultValues }: Props) {
  const form = useForm<UpdateUserRequest>({
    resolver: zodResolver(updateUserSchema),
    defaultValues,
  })

  const onSubmit = async (values: UpdateUserRequest) => {
    await updateUserByAdmin(values, userId)
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-6"
      >
        <FormField
          control={form.control}
          name="firstName"
          render={({ field }) => (
            <FormItem>
              <FormLabel>First Name</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="lastName"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Last Name</FormLabel>

              <FormControl>
                <Input {...field} />
              </FormControl>

              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          disabled={form.formState.isSubmitting}
        >
          Update User
        </Button>
      </form>
    </Form>
  )
}