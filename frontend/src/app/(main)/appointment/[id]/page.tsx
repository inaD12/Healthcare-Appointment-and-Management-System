import { patientService } from "@/features/patients/services/patientService"
import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"

import { AppointmentStatus } from "@/features/patients/types/patientsTypes"

import AppointmentPageClient from "./AppointmentPageClient"
import { ratingService } from "@/features/ratings/services/ratingService"

export default async function AppointmentPage({
  params,
}: {
  params: Promise<{ id: string }>
}) {
  const { id } = await params

  let appointment = null
  let rating = null

  try {
    const apiResponse =
      await patientService.getAppointmentWithEncounters(id)

    appointment =
      mapAppointmentResponseToAppointment(apiResponse)

    if (appointment?.status === AppointmentStatus.Completed) {
      try {
        const ratingRes =
          await ratingService.getRatingByAppointment(id)

        rating = ratingRes.data.data
      } catch (err: any) {
        if (err?.response?.status !== 404) {
          console.error("Failed to fetch rating")
        }
      }
    }
  } catch (err) {
    return (
      <div className="p-6 text-red-600">
        Failed to load appointment
      </div>
    )
  }

  if (!appointment) {
    return <div className="p-6">No appointment found</div>
  }

  return (
    <AppointmentPageClient
      initialAppointment={appointment}
      initialRating={rating}
    />
  )
}