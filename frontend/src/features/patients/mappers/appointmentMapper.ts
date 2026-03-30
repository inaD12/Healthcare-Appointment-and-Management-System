import { Appointment, AppointmentByIdResponse, AppointmentStatus, EncounterDetails } from "../types/patientTypes"

export function mapAppointmentResponseToAppointment(
  response: AppointmentByIdResponse | null
): Appointment | null {
  if (!response) return null
  const raw = response.appointmentById?.[0]
  if (!raw) return null

  return {  
    id: raw.id,
    start: raw.start,
    end: raw.end,
    status: raw.status as AppointmentStatus,
    doctorId: raw.doctorId,
    patientId: raw.patientId,
    doctorName: raw.doctorName || "",
    encounterDetails: raw.encounterDetails || null,
  }
}