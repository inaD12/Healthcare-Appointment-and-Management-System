"use client"

import { useEffect, useState } from "react"
import { Card, CardHeader, CardTitle, CardDescription, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Calendar, Users, FileText, Stethoscope } from "lucide-react"
import Link from "next/link"

import { useAuth } from "@/features/auth/hooks/useAuth"
import { patientService } from "@/features/patients/services/patientService"
import {
  PatientDashboard,
  DoctorDashboardView,
} from "@/features/patients/types/patientTypes"

import { DashboardCard } from "@/components/home/DashboardCard"
import { EncounterUpdateCard } from "@/components/home/EncounterUpdateCard"
import { Badge } from "@/components/ui/badge"
import { ScrollArea } from "@/components/ui/scroll-area"

export default function HomePage() {
  const { user, roles } = useAuth()

  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  const [dashboard, setDashboard] = useState<PatientDashboard | null>(null)
  const [doctorDashboard, setDoctorDashboard] =
    useState<DoctorDashboardView | null>(null)

  const [loading, setLoading] = useState(false)

  useEffect(() => {
    if (!isPatient) return

    const load = async () => {
      setLoading(true)
      try {
        const data = await patientService.getPatientDashboard()
        setDashboard(data)
      } finally {
        setLoading(false)
      }
    }

    load()
  }, [isPatient])

  useEffect(() => {
    if (!isDoctor || !user?.id) return

    const load = async () => {
      const data = await patientService.getDoctorDashboard(user.id)
      setDoctorDashboard(data)
    }

    load()
  }, [isDoctor, user?.id])

  const nextAppointment = dashboard?.upcomingAppointment?.nodes?.[0]
  const lastAppointment = dashboard?.lastAppointment?.nodes?.[0]
  const recentEncounters = dashboard?.myEncounters?.nodes ?? []

  const todayAppointments = doctorDashboard?.todayAppointments ?? []
  const nextDoctorAppointment = doctorDashboard?.nextAppointment
  // const unfinishedEncounters =
  //   doctorDashboard?.unfinishedEncounters ?? []
  const unfinishedEncounters = [
  {
    id: "enc_1",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_2",
    patientId: "Jane Smith",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_13",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_14",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_15",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_16",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_17",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_18",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_19",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_111",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_122",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_133",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },
  {
    id: "enc_144",
    patientId: "John Doe",
    status: "IN_PROGRESS",
  },

]

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

        {isDoctor && doctorDashboard && (
          <>
            <div className="grid gap-6 md:grid-cols-2">

              <DashboardCard
                icon={<Calendar className="h-5 w-5" />}
                title="Today's Appointments"
                value={doctorDashboard.totalToday}
                description="Scheduled for today"
              />

              <DashboardCard
                icon={<Calendar className="h-5 w-5" />}
                title="Next Appointment"
                value={
                  nextDoctorAppointment
                    ? new Date(nextDoctorAppointment.start).toLocaleString()
                    : "No upcoming appointment"
                }
                description={nextDoctorAppointment?.patientName ?? ""}
                href={
                  nextDoctorAppointment
                    ? `/appointments/${nextDoctorAppointment.id}`
                    : undefined
                }
              />

              <DashboardCard
                icon={<Users className="h-5 w-5" />}
                title="Minutes Until Next"
                value={
                  doctorDashboard.minutesUntilNext ?? "—"
                }
                description="Time remaining"
              />

              <DashboardCard
                icon={<FileText className="h-5 w-5" />}
                title="Active Encounters"
                value={unfinishedEncounters.length}
                description="In progress or finalized"
              />

              {unfinishedEncounters.length > 0 && (
                <div className="col-span-full space-y-4">

                  <div className="flex items-center justify-between">
                    <h2 className="text-xl font-semibold">Encounter Queue</h2>
                  </div>

                  <Card className="w-full">
                    <CardContent className="p-0">

                      <ScrollArea className="h-[520px] w-full">

                        <div className="divide-y">

                          {unfinishedEncounters.map((encounter) => (
                            <div
                              key={encounter.id}
                              className="flex items-center justify-between px-5 py-4 hover:bg-muted/50 transition"
                            >

                              <div className="space-y-1">
                                <div className="font-medium">
                                  Encounter #{encounter.id.slice(0, 6)}
                                </div>

                                <div className="text-sm text-muted-foreground">
                                  Patient ID: {encounter.patientId}
                                </div>
                              </div>

                              <div className="flex items-center gap-3">

                                <Badge variant="secondary">
                                  {encounter.status.replace("_", " ")}
                                </Badge>

                                <Button asChild size="sm">
                                  <Link href={`/encounter/${encounter.id}`}>
                                    Open
                                  </Link>
                                </Button>

                              </div>

                            </div>
                          ))}

                        </div>

                      </ScrollArea>

                    </CardContent>
                  </Card>
                </div>
              )}
            </div>
          </>
        )}

        {isAdmin && (
          <div className="grid gap-6 md:grid-cols-3">
            <DashboardCard
              icon={<Users className="h-5 w-5" />}
              title="Total Users"
              value="—"
              description="System-wide"
            />

            <DashboardCard
              icon={<Stethoscope className="h-5 w-5" />}
              title="Doctors"
              value="—"
              description="Active staff"
            />

            <DashboardCard
              icon={<Calendar className="h-5 w-5" />}
              title="Appointments Today"
              value="—"
              description="All clinics"
            />
          </div>
        )}

      </div>

      <Card>
        <CardHeader>
          <CardTitle>Quick Actions</CardTitle>
          <CardDescription>
            Common tasks you can perform.
          </CardDescription>
        </CardHeader>

        <div className="p-6 flex flex-wrap gap-3">

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

          {isAdmin && (
            <Button asChild>
              <Link href="/admin/users">
                Manage Users
              </Link>
            </Button>
          )}
        </div>
      </Card>

    </div>
  )
}