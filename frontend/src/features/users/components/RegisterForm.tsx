"use client"

import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"

import {
  registerUserSchema,
  registerUserByAdminSchema,
  ROLES,
  RegisterUserRequest,
  RegisterUserByAdminRequest,
} from "@/features/users/types/usersTypes"

import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Button } from "@/components/ui/button"

import {
  Select,
  SelectTrigger,
  SelectContent,
  SelectItem,
  SelectValue,
} from "@/components/ui/select"

import { useAuth } from "@/features/auth/hooks/useAuth"
import { userService } from "../services/userService"

export default function RegisterForm() {
  const { roles} = useAuth()

  const isAdmin = roles.includes(ROLES.ADMIN)

  const schema = isAdmin
    ? registerUserByAdminSchema
    : registerUserSchema

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    setValue,
  } = useForm<any>({
    resolver: zodResolver(schema),
  })

  const onSubmit = async (data: any) => {
    try {
      const payload = {
        ...data,
        dateOfBirth: new Date(data.dateOfBirth),
      }

      if (isAdmin) {
        await userService.registerUserByAdmin(payload as RegisterUserByAdminRequest)
        alert("User created successfully")
      } else {
        await userService.registerUser(payload as RegisterUserRequest)
        alert("Registration successful!")
      }
    } catch (err: any) {
      alert(err?.response?.data?.message || "Operation failed")
    }
  }

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">

      <div>
        <Label>Email</Label>
        <Input {...register("email")} />
        {errors.email && (
          <p className="text-red-500 text-sm">
            {errors.email.message as string}
          </p>
        )}
      </div>

      <div>
        <Label>Password</Label>
        <Input type="password" {...register("password")} />
      </div>

      <div className="grid grid-cols-2 gap-4">
        <div>
          <Label>First Name</Label>
          <Input {...register("firstName")} />
        </div>

        <div>
          <Label>Last Name</Label>
          <Input {...register("lastName")} />
        </div>
      </div>

      <div>
        <Label>Date of Birth</Label>
        <Input type="date" {...register("dateOfBirth")} />
      </div>

      <div>
        <Label>Phone Number</Label>
        <Input {...register("phoneNumber")} />
      </div>

      <div>
        <Label>Address</Label>
        <Input {...register("address")} />
      </div>

      {isAdmin && (
        <div>
          <Label>Role</Label>

          <Select
            onValueChange={(val) =>
              setValue("role", val as "Patient" | "Doctor" | "Admin")
            }
          >
            <SelectTrigger>
              <SelectValue placeholder="Select role" />
            </SelectTrigger>

            <SelectContent>
              <SelectItem value={ROLES.PATIENT}>Patient</SelectItem>
              <SelectItem value={ROLES.DOCTOR}>Doctor</SelectItem>
              <SelectItem value={ROLES.ADMIN}>Admin</SelectItem>
            </SelectContent>
          </Select>
        </div>
      )}

      <Button type="submit" className="w-full" disabled={isSubmitting}>
        {isSubmitting ? "Submitting..." : "Submit"}
      </Button>
    </form>
  )
}