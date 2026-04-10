"use client"

import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Calendar, Users, FileText, Stethoscope } from "lucide-react"
import { useAuth } from "@/features/auth/hooks/useAuth"

export default function HomePage() {
  const { user, roles } = useAuth()

  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">
            Welcome back {user?.firstName ?? ""}
          </CardTitle>
          <CardDescription>
            Here is an overview of your dashboard.
          </CardDescription>
        </CardHeader>
      </Card>

      <div className="grid gap-6 md:grid-cols-3">
        {isPatient && (
          <>
            <DashboardCard
              icon={<Calendar className="h-5 w-5" />}
              title="Upcoming Appointment"
              value="Tomorrow 10:00"
              description="Dr. Smith"
            />

            <DashboardCard
              icon={<FileText className="h-5 w-5" />}
              title="Prescriptions"
              value="2 Active"
              description="Last updated yesterday"
            />

            <DashboardCard
              icon={<Stethoscope className="h-5 w-5" />}
              title="Last Checkup"
              value="12 March 2026"
              description="General consultation"
            />
          </>
        )}

        {isDoctor && (
          <>
            <DashboardCard
              icon={<Calendar className="h-5 w-5" />}
              title="Today's Appointments"
              value="12"
              description="Next at 09:30"
            />

            <DashboardCard
              icon={<Users className="h-5 w-5" />}
              title="Patients This Week"
              value="57"
              description="5 new patients"
            />

            <DashboardCard
              icon={<FileText className="h-5 w-5" />}
              title="Pending Reports"
              value="3"
              description="Awaiting review"
            />
          </>
        )}

        {isAdmin && (
          <>
            <DashboardCard
              icon={<Users className="h-5 w-5" />}
              title="Total Users"
              value="1,245"
              description="Across the system"
            />

            <DashboardCard
              icon={<Stethoscope className="h-5 w-5" />}
              title="Doctors"
              value="87"
              description="Active staff"
            />

            <DashboardCard
              icon={<Calendar className="h-5 w-5" />}
              title="Appointments Today"
              value="312"
              description="Across all clinics"
            />
          </>
        )}
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Quick Actions</CardTitle>
          <CardDescription>Common tasks you can perform.</CardDescription>
        </CardHeader>

        <CardContent className="flex flex-wrap gap-3">
          {isPatient && (
            <Button>
              <Calendar className="mr-2 h-4 w-4" />
              Book Appointment
            </Button>
          )}

          {isDoctor && (
            <Button>
              <Calendar className="mr-2 h-4 w-4" />
              View Schedule
            </Button>
          )}

          {isAdmin && (
            <Button>
              <Users className="mr-2 h-4 w-4" />
              Manage Users
            </Button>
          )}
        </CardContent>
      </Card>
    </div>
  )
}

function DashboardCard({
  icon,
  title,
  value,
  description,
}: {
  icon: React.ReactNode
  title: string
  value: string
  description: string
}) {
  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between pb-2">
        <CardTitle className="text-sm font-medium">
          {title}
        </CardTitle>
        {icon}
      </CardHeader>

      <CardContent>
        <div className="text-2xl font-bold">{value}</div>
        <p className="text-xs text-muted-foreground mt-1">
          {description}
        </p>
      </CardContent>
    </Card>
  )
}