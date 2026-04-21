import { getRatingByAppointment } from "@/features/ratings/services/ratingService"
import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"
import { AppointmentStatus } from "@/features/patients/types/patientTypes"
import { patientService } from "@/features/patients/services/patientService"
import AdminAppointmentPageClient from "./AdminAppointmentPageClient"

export default async function Page({ params }: { params: { id: string } }) {
  const { id } = await params

  const res = await patientService.getAppointmentWithEncounters(id)
  const appointment = mapAppointmentResponseToAppointment(res)

  let rating = null

  if (appointment?.status === AppointmentStatus.Completed) {
    try {
      const ratingRes = await getRatingByAppointment(appointment.id)
      rating = ratingRes.data.data
    } catch {
      rating = null
    }
  }

  return (
    <AdminAppointmentPageClient
      initialAppointment={appointment}
      initialRating={rating}
    />
  )
}