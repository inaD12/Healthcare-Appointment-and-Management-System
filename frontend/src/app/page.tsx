"use client"

import { useEffect, useState } from "react"
import { Card, CardContent, CardHeader, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Calendar, Users, FileText, Stethoscope } from "lucide-react"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { PatientDashboard } from "@/features/patients/types/patientTypes"
import { patientService } from "@/features/patients/services/patientService"
import Link from "next/link"
import { DashboardCard } from "@/components/home/DashboardCard"
import { EncounterUpdateCard } from "@/components/home/EncounterUpdateCard"

export default function HomePage() {
  const { user, roles } = useAuth()

  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  const [dashboard, setDashboard] = useState<PatientDashboard | null>(null)

  useEffect(() => {
    if (!isPatient) return

    const loadDashboard = async () => {
      const data = await patientService.getPatientDashboard()
      setDashboard(data)
    }

    loadDashboard()
  }, [isPatient])

  const nextAppointment = dashboard?.upcomingAppointment?.nodes?.[0]
  const lastAppointment = dashboard?.lastAppointment?.nodes?.[0]
  const recentEncounters = dashboard?.myEncounters?.nodes ?? [] 

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

      <div className="space-y-8">

      {isPatient && dashboard && (
        <>
          <div className="space-y-3">
            <h2 className="text-lg font-semibold">Appointments</h2>

            <div className="grid gap-6 md:grid-cols-2">
              <DashboardCard
                icon={<Calendar className="h-5 w-5" />}
                title="Next Appointment"
                value={
                  nextAppointment
                    ? new Date(nextAppointment.start).toLocaleString(undefined, {
                        dateStyle: "medium",
                        timeStyle: "short",
                      })
                    : "None scheduled"
                }
                description={nextAppointment?.doctorName ?? ""}
                href={
                  nextAppointment
                    ? `/appointment/${nextAppointment.id}`
                    : undefined
                }
              />

              <DashboardCard
                icon={<Calendar className="h-5 w-5" />}
                title="Last Appointment"
                value={
                  lastAppointment
                    ? new Date(lastAppointment.start).toLocaleString(undefined, {
                        dateStyle: "medium",
                        timeStyle: "short",
                      })
                    : "No previous visits"
                }
                description={lastAppointment?.doctorName ?? ""}
                href={
                  lastAppointment
                    ? `/appointment/${lastAppointment.id}`
                    : undefined
                }
              />
            </div>
          </div>

          <div className="space-y-3">
            <h2 className="text-lg font-semibold">Recent Medical Updates</h2>

            <div className="grid gap-6 md:grid-cols-3">
              {recentEncounters.slice(0, 3).map((encounter) => (
                <EncounterUpdateCard
                  key={encounter.id}
                  encounter={encounter}
                />
              ))}
            </div>
          </div>
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
            <CardDescription>
              Common tasks you can perform.
            </CardDescription>
          </CardHeader>

          <CardContent className="flex flex-wrap gap-3">
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
              </>
            )}

            {isDoctor && (
              <Button asChild>
                <Link href="/doctor/schedule">
                  <Calendar className="mr-2 h-4 w-4" />
                  View Schedule
                </Link>
              </Button>
            )}

            {isAdmin && (
              <Button asChild>
                <Link href="/admin/users">
                  <Users className="mr-2 h-4 w-4" />
                  Manage Users
                </Link>
              </Button>
            )}
          </CardContent>
        </Card>
      </div>
    )
  }