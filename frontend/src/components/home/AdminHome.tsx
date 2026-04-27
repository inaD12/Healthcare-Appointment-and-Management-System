"use client"

import { AdminDashboard } from "@/features/patients/types/patientTypes"
import { DashboardCard } from "@/components/home/DashboardCard"
import { Users, FileText, Calendar } from "lucide-react"

interface Props {
  dashboard: AdminDashboard
}

export default function AdminHome({ dashboard }: Props) {
  return (
    <div className="grid gap-6 md:grid-cols-3">
      <DashboardCard
        icon={<Users className="h-5 w-5" />}
        title="Total Patients"
        value={dashboard.patientsTotalCount}
        description="Registered in system"
      />

      <DashboardCard
        icon={<FileText className="h-5 w-5" />}
        title="Encounters"
        value={dashboard.encountersTotalCount}
        description="Total recorded"
      />

      <DashboardCard
        icon={<Calendar className="h-5 w-5" />}
        title="Appointments"
        value={dashboard.appointmentsTotalCount}
        description="Total scheduled"
      />
    </div>
  )
}