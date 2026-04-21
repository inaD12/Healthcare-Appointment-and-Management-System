"use client"

import { useEffect, useState } from "react"
import { InfiniteData, useInfiniteQuery } from "@tanstack/react-query"
import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Calendar, Users, FileText, Stethoscope } from "lucide-react"
import Link from "next/link"

import { useAuth } from "@/features/auth/hooks/useAuth"
import { patientService } from "@/features/patients/services/patientService"

import {
  PatientDashboard,
  DoctorDashboardView,
  DoctorEncounterConnection,
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

  const {
    data: encounterPages,
    fetchNextPage,
    hasNextPage,
    isFetchingNextPage,
  } = useInfiniteQuery<
    DoctorEncounterConnection,
    Error,
    InfiniteData<DoctorEncounterConnection>,
    (string | undefined)[],
    string | null
  >({
    queryKey: ["doctor-encounters", user?.id],
    initialPageParam: null,
    enabled: isDoctor && !!user?.id,
    queryFn: ({ pageParam }) =>
      patientService.getDoctorEncounters(user!.id, pageParam ?? undefined),
    getNextPageParam: (lastPage) =>
      lastPage.pageInfo.hasNextPage
        ? lastPage.pageInfo.endCursor
        : undefined,
  })

  const unfinishedEncounters =
    encounterPages?.pages.flatMap((p) => p.nodes) ?? []

  const totalEncountersCount =
    encounterPages?.pages?.[0]?.totalCount ?? 0

  const isScrollable = unfinishedEncounters.length > 4

  const nextAppointment = dashboard?.upcomingAppointment?.nodes?.[0]
  const lastAppointment = dashboard?.lastAppointment?.nodes?.[0]
  const recentEncounters = dashboard?.myEncounters?.nodes ?? []

  const nextDoctorAppointment = doctorDashboard?.nextAppointment

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
                      ? new Date(nextAppointment.start).toLocaleString(
                          undefined,
                          { dateStyle: "medium", timeStyle: "short" }
                        )
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
                      ? new Date(lastAppointment.start).toLocaleString(
                          undefined,
                          { dateStyle: "medium", timeStyle: "short" }
                        )
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
              <h2 className="text-lg font-semibold">
                Recent Medical Updates
              </h2>

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
                value={doctorDashboard.minutesUntilNext ?? "—"}
                description="Time remaining"
              />

              <DashboardCard
                icon={<FileText className="h-5 w-5" />}
                title="Active Encounters"
                value={totalEncountersCount}
                description="In progress or finalized"
              />

              {unfinishedEncounters.length > 0 && (
                <div className="col-span-full space-y-4">
                  <div className="flex items-center justify-between">
                    <h2 className="text-xl font-semibold">
                      Encounter Queue
                    </h2>
                  </div>

                  <Card className="w-full">
                    <CardContent className="p-0">
                      <ScrollArea
                        className={`w-full ${
                          isScrollable ? "h-[520px]" : "max-h-fit"
                        }`}
                      >
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
                                  Patient: {encounter.patientName}
                                </div>

                                <div className="text-xs text-muted-foreground">
                                  Started:{" "}
                                  {encounter.startedAt
                                    ? new Date(encounter.startedAt).toLocaleString(
                                        undefined,
                                        {
                                          dateStyle: "medium",
                                          timeStyle: "short",
                                        }
                                      )
                                    : "—"}
                                </div>
                              </div>

                              <div className="flex items-center gap-3">
                                <Badge variant="secondary">
                                  {encounter.status.replace("_", " ")}
                                </Badge>

                                <Button asChild size="sm">
                                  <Link
                                    href={`/doctors/appointment/${encounter.appointmentId}`}
                                  >
                                    Open
                                  </Link>
                                </Button>
                              </div>
                            </div>
                          ))}
                        </div>

                        {hasNextPage && (
                          <div className="flex justify-center py-4">
                            <Button
                              variant="outline"
                              size="sm"
                              onClick={() => fetchNextPage()}
                              disabled={isFetchingNextPage}
                            >
                              {isFetchingNextPage
                                ? "Loading..."
                                : "Load more"}
                            </Button>
                          </div>
                        )}
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

          {isDoctor && (
            <Button asChild>
              <Link href="/doctors/profile">
                <Users className="mr-2 h-4 w-4" />
                View Profile
              </Link>
            </Button>
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