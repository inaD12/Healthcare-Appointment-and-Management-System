"use client"

import RegisterForm from "@/features/users/components/RegisterForm"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "@/components/ui/card"
import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { ROLES } from "@/features/users/types/userTypes"

export default function CreateDoctorPage() {
    useRequireRole(ROLES.ADMIN)

  return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-100 to-blue-200 p-4">
        <Card className="w-full max-w-md shadow-xl rounded-xl bg-white p-6">
          <CardHeader className="text-center mb-4">
            <CardTitle className="text-3xl font-bold">
              Create User
            </CardTitle>
          </CardHeader>
  
          <CardContent>
            <RegisterForm/>
          </CardContent>
        </Card>
      </div>
    )
}