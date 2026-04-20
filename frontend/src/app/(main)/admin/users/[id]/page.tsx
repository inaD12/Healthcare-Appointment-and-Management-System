import AdminUserClient from "./AdminUserClient"

import { getUserByAdmin } from "@/features/users/services/userService"
import { getDoctorByUserId } from "@/features/doctors/services/doctorService"
import { patientService } from "@/features/patients/services/patientService"
import { getRatingsByDoctor } from "@/features/ratings/services/ratingService"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"

export default async function Page({
  params,
}: {
  params: { id: string }
}) {
  const { id } = await params

  const userRes = await getUserByAdmin(id)
  const user = userRes.data.data

  let doctor = null
  let patient = null
  let initialRatings: RatingQueryViewModel[] = []
  let initialRatingsTotalPages = 1
  let initialAppointments = null

  if (user.roles.includes("Doctor")) {
    const doctorRes = await getDoctorByUserId(id)
    doctor = doctorRes.data.data

    const ratingsRes = await getRatingsByDoctor(id, {
      PatientId: "",
      AppointmentId: "",
      MinScore: null,
      MaxScore: null,
      SortOrder: "DESC",
      SortPropertyName: "CreatedAt",
      Page: 1,
      PageSize: 4,
    }).catch(() => null)

    const items = ratingsRes?.data?.data?.items ?? []

    initialRatings = items
    initialRatingsTotalPages = Math.ceil(
      (ratingsRes?.data?.data?.totalCount ?? items.length) / 4
    )
  }

  if (user.roles.includes("Patient")) {
    const patientRes = await patientService.getPatientInfoInitial(id, 5)
    patient = patientRes.profile

    const apptRes = await patientService.getPatientAppointmentsPage(id, 5)
    initialAppointments = apptRes
  }

  return (
    <AdminUserClient
      user={user}
      doctor={doctor}
      patient={patient}
      initialRatings={initialRatings}
      initialRatingsTotalPages={initialRatingsTotalPages}
      initialAppointments={initialAppointments}
    />
  )
}