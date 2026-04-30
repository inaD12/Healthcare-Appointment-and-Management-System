"use client"

import Link from "next/link"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

import { Stethoscope, LogIn, UserPlus } from "lucide-react"
import keycloak from "@/config/keycloak"

export default function UnauthenticatedHome() {
  return (
    <div className="min-h-screen flex items-center bg-gradient-to-b from-muted/40 to-background">

      <div className="mx-auto w-full max-w-6xl px-6 grid gap-12 lg:grid-cols-2 items-center">

        <div className="space-y-6">

          <div className="flex items-center gap-2 text-primary font-semibold">
            <Stethoscope className="h-5 w-5" />
            Mediflow
          </div>

          <h1 className="text-4xl font-semibold tracking-tight">
            Healthcare management made simple
          </h1>

          <p className="text-muted-foreground max-w-md">
            A streamlined platform for managing patients, appointments, and clinical workflows
            in one place.
          </p>

          <div className="flex gap-3">
           <Button
              onClick={() =>
                keycloak.login({
                  redirectUri: window.location.href,
                })
              }
              >
              <LogIn className="mr-2 h-4 w-4" />
              Sign in
          </Button>

            <Button asChild variant="outline">
              <Link href="/register">
                <UserPlus className="mr-2 h-4 w-4" />
                Create account
              </Link>
            </Button>
          </div>

        </div>

        <div className="space-y-4">

          <Card>
            <CardHeader>
              <CardTitle className="text-lg">Appointments</CardTitle>
              <CardDescription>
                Schedule and manage patient visits efficiently
              </CardDescription>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle className="text-lg">Patient Records</CardTitle>
              <CardDescription>
                Access and update medical history in real time
              </CardDescription>
            </CardHeader>
          </Card>

          <Card>
            <CardHeader>
              <CardTitle className="text-lg">Encounters</CardTitle>
              <CardDescription>
                Track consultations and treatment progress
              </CardDescription>
            </CardHeader>
          </Card>

        </div>

      </div>

    </div>
  )
}