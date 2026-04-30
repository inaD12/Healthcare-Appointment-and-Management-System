import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"
import { AppointmentStatus } from "@/features/patients/types/patientsTypes"
import { patientService } from "@/features/patients/services/patientService"
import AdminAppointmentPageClient from "./AdminAppointmentPageClient"
import { ratingService } from "@/features/ratings/services/ratingService"

export default async function Page({ params }: { params: { id: string } }) {
  const { id } = await params

  const res = await patientService.getAppointmentWithEncounters(id)
  const appointment = mapAppointmentResponseToAppointment(res)

  let rating = null

  if (appointment?.status === AppointmentStatus.Completed) {
    try {
      const ratingRes = await ratingService.getRatingByAppointment(appointment.id)
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