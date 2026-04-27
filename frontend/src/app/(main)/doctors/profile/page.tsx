import { getMyDoctorInfo } from "@/features/doctors/services/doctorService"
import { getMyAppointments } from "@/features/appointments/services/appointmentService"
import DoctorProfileClient from "./DoctorProfileClient"
import { AppointmentResponse } from "@/features/appointments/types/appointmentsTypes"

export default async function DoctorProfilePage() {

  const doctorRes = await getMyDoctorInfo()

  const start = new Date()
  const end = new Date()
  end.setDate(end.getDate() + 7)

  const formatDate = (d: Date) => d.toISOString().split("T")[0]

  let appointments: AppointmentResponse[] = []

  try {

    const apptRes = await getMyAppointments({
      startDate: formatDate(start),
      endDate: formatDate(end)
    })

    const data = apptRes?.data?.data ?? apptRes?.data ?? []

    appointments = [...data].sort(
      (a, b) =>
        new Date(a.duration.start).getTime() -
        new Date(b.duration.start).getTime()
    )

  } catch (err: any) {

    if (err?.response?.status !== 404) {
      console.error("Failed to fetch appointments:", err)
    }

    appointments = []
  }

  return (
    <DoctorProfileClient
      doctor={doctorRes.data.data}
      appointments={appointments}
    />
  )
}