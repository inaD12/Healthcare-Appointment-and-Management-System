"use client"

import RegisterForm from "@/features/users/components/RegisterForm"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "@/components/ui/card"

export default function RegisterPage() {
  return (
    <div className="min-h-screen flex items-center justify-center p-4">
      <Card className="w-full max-w-md shadow-xl rounded-xl bg-white p-6">
        <CardHeader className="text-center mb-4">
          <CardTitle className="text-3xl font-bold">
            Create Account
          </CardTitle>
        </CardHeader>

        <CardContent>
          <RegisterForm/>
        </CardContent>
      </Card>
    </div>
  )
}