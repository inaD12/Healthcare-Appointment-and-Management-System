"use client"

import { InfiniteData, useInfiniteQuery } from "@tanstack/react-query"
import { Calendar, Users, FileText } from "lucide-react"
import Link from "next/link"

import { DashboardCard } from "@/components/home/DashboardCard"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Card, CardContent } from "@/components/ui/card"
import { ScrollArea } from "@/components/ui/scroll-area"

import { patientService } from "@/features/patients/services/patientService"
import {
  DoctorDashboardView,
  DoctorEncounterConnection,
} from "@/features/patients/types/patientTypes"

interface Props {
  doctorDashboard: DoctorDashboardView
  userId: string
}

export function DoctorHome({ doctorDashboard, userId }: Props) {
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
    queryKey: ["doctor-encounters", userId],
    initialPageParam: null,
    queryFn: ({ pageParam }) =>
      patientService.getDoctorEncounters(userId, pageParam ?? undefined),
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

  const nextDoctorAppointment = doctorDashboard?.nextAppointment

  return (
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

          <h2 className="text-xl font-semibold">
            Encounter Queue
          </h2>

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
  )
}