"use client"

import { PatientDashboard } from "@/features/patients/types/patientTypes"
import { DashboardCard } from "@/components/home/DashboardCard"
import { EncounterUpdateCard } from "@/components/home/EncounterUpdateCard"
import { Calendar } from "lucide-react"

interface Props {
  dashboard: PatientDashboard
}

export default function PatientHome({ dashboard }: Props) {
  const nextAppointment = dashboard?.upcomingAppointment?.nodes?.[0]
  const lastAppointment = dashboard?.lastAppointment?.nodes?.[0]
  const recentEncounters = dashboard?.myEncounters?.nodes ?? []

  return (
    <div className="space-y-8">
      <div className="space-y-3">
        <h2 className="text-lg font-semibold">Appointments</h2>

        <div className="grid gap-6 md:grid-cols-2">
          <DashboardCard
            icon={<Calendar className="h-5 w-5" />}
            title="Next Appointment"
            value={
              nextAppointment
                ? new Date(nextAppointment.start).toLocaleString()
                : "None scheduled"
            }
            description={nextAppointment?.doctorName ?? ""}
          />

          <DashboardCard
            icon={<Calendar className="h-5 w-5" />}
            title="Last Appointment"
            value={
              lastAppointment
                ? new Date(lastAppointment.start).toLocaleString()
                : "No previous visits"
            }
            description={lastAppointment?.doctorName ?? ""}
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
    </div>
  )
}