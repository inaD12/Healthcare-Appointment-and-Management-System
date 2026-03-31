import { getAppointmentWithEncounters } from "@/features/patients/services/patientService"
import { getRatingByAppointment } from "@/features/ratings/services/ratingService"
import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"
import DoctorAppointmentClient from "./DoctorAppointmentClient"
import { AppointmentStatus } from "@/features/patients/types/patientTypes"

export default async function Page({ params }: { params: { id: string } }) {
  const { id } = await params

  const res = await getAppointmentWithEncounters(id)
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
    <DoctorAppointmentClient
      initialAppointment={appointment}
      initialRating={rating}
    />
  )
}