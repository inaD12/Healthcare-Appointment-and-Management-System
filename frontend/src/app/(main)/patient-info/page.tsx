"use client"

import { useRouter } from "next/navigation"
import { Card, CardContent, CardTitle } from "@/components/ui/card"

import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { patientService } from "@/features/patients/services/patientService"
import { AppointmentList } from "@/components/appointments/AppointmentsList"
import { useCallback, useEffect, useState } from "react"
import { useAppointmentsPagination } from "@/components/appointments/useAppointmentsPagination"

const PAGE_SIZE = 5

export default function PatientDashboardPage() {
  useAuthGuard()
  const router = useRouter()

  const fetchAppointments = useCallback(
    (after?: string) =>
      patientService.getMyPatientAppointmentsPage(PAGE_SIZE, after),
    []
  )

  const {
    appointments,
    loading,
    hasNextPage,
    loadingMore,
    loadMore,
  } = useAppointmentsPagination(fetchAppointments, PAGE_SIZE)

  const [patient, setPatient] = useState<any>(null)

  useEffect(() => {
    const load = async () => {
      const data = await patientService.getMyPatientInfoInitial(PAGE_SIZE)
      setPatient(data.profile)
    }

    load()
  }, [])

  if (loading) return <p className="p-8 text-center">Loading...</p>

  return (
    <div className="max-w-5xl mx-auto p-8 space-y-8">

      <Card className="p-6 shadow-lg">
        <CardTitle className="text-xl font-semibold">
          Patient Info
        </CardTitle>

        <CardContent className="mt-2 space-y-1">
          <p><strong>Name:</strong> {patient?.fullName}</p>
          <p><strong>Birth Date:</strong> {patient?.birthDate}</p>

          <p>
            <strong>Allergies:</strong>{" "}
            {patient?.allergiesList?.length
              ? patient.allergiesList.join(", ")
              : "None"}
          </p>

          <p>
            <strong>Conditions:</strong>{" "}
            {patient?.conditionsList?.length
              ? patient.conditionsList.join(", ")
              : "None"}
          </p>
        </CardContent>
      </Card>

      <Card className="p-6 shadow-lg">
        <CardTitle className="text-xl font-semibold">
          Appointments
        </CardTitle>

        <CardContent className="mt-3">
          <AppointmentList
            appointments={appointments}
            emptyText="No appointments found."
            showLoadMore={hasNextPage}
            loadingMore={loadingMore}
            onLoadMore={loadMore}
            onSelectAppointment={(a) =>
              router.push(`/appointment/${a.id}`)
            }
          />
        </CardContent>
      </Card>

    </div>
  )
}