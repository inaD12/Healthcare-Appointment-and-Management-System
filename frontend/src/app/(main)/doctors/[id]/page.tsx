import DoctorCalendarClient from "./DoctorCalendarClient"
import { getDoctorByUserId } from "@/features/doctors/services/doctorService"
import { getAppointmentsByDoctor } from "@/features/appointments/services/appointmentService"
import { getRatingsByDoctor } from "@/features/ratings/services/ratingService"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"

export default async function DoctorCalendarPage(props: {
  params: Promise<{ id: string }>
  searchParams: Promise<{ rescheduleId?: string }>
}) {
  const { params, searchParams } = props

  const { id } = await params
  const { rescheduleId } = await searchParams
  const doctorRes = await getDoctorByUserId(id)
  const doctor = doctorRes.data.data

  let appointments: BookingQueryResponse[] = []
  let ratings: RatingQueryViewModel[] = []
  let ratingsTotalPages = 1

  const now = new Date()
  const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1)
  const endOfMonth = new Date(now.getFullYear(), now.getMonth() + 1, 0, 23, 59, 59)

  try {
    const apptRes = await getAppointmentsByDoctor(doctor.userId, {
      startDate: startOfMonth.toISOString().split("T")[0],
      endDate: endOfMonth.toISOString().split("T")[0],
    })
    appointments = apptRes.data.data
  } catch {}

  try {
    const ratingRes = await getRatingsByDoctor(doctor.userId, {
      PatientId: "",
      AppointmentId: "",
      MinScore: null,
      MaxScore: null,
      SortOrder: "DESC",
      SortPropertyName: "CreatedAt",
      Page: 1,
      PageSize: 4,
    })

    ratings = ratingRes?.data?.data?.items ?? []
    ratingsTotalPages = Math.ceil(
      (ratingRes?.data?.data?.totalCount ?? ratings.length) / 4
    )
  } catch {}

  return (
    <DoctorCalendarClient
      doctor={doctor}
      initialAppointments={appointments}
      initialRatings={ratings}
      initialRatingsTotalPages={ratingsTotalPages}
      rescheduleId={rescheduleId ?? null}
    />
  )
}