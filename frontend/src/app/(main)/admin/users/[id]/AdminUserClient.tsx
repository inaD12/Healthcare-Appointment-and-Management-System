"use client"
import { useRouter } from "next/navigation"
import { useState } from "react"
import DoctorSchedule from "@/components/schedule/DoctorSchedule"
import { DoctorRatings } from "@/components/ratings/DoctorRatings"
import { DoctorSpecialitiesCard } from "@/components/doctor/DoctorSpecialitiesCard"
import { PersonProfileCard } from "@/components/profile/PersonProfileCard"
import { AdminUserEditForm } from "@/components/admin/AdminUserEditForm"
import { getByDateAdmin } from "@/features/appointments/services/appointmentService"
import { addSpecialityByAdmin, removeSpecialityByAdmin } from "@/features/doctors/services/doctorService"
import { useDeleteRating } from "@/features/admin/hooks/useDeleteRating"
import { useDoctorRatings } from "@/features/admin/hooks/useDoctorRatings"
import { PatientProfile } from "@/features/patients/types/patientTypes"
import { UserQueryResponse } from "@/features/users/types/userTypes"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"

interface AdminUserClientProps {
  user: UserQueryResponse
  doctor: DoctorQueryViewModel | null
  patient: PatientProfile | null
  initialRatings: RatingQueryViewModel[]
  initialRatingsTotalPages: number
}

export default function AdminUserClient({
  user,
  doctor,
  patient,
  initialRatings,
  initialRatingsTotalPages,
}: AdminUserClientProps) {
  const router = useRouter()
  const [ratingsPage, setRatingsPage] = useState(1)

  const isDoctor = user?.roles?.includes("Doctor")

  const { data: ratingsData } = useDoctorRatings(user.id, ratingsPage, {
    items: initialRatings,
    total: initialRatingsTotalPages * 4,
  })

  const deleteRating = useDeleteRating()

  const ratings = ratingsData?.items ?? []
  const totalPages = Math.ceil((ratingsData?.total ?? 0) / 4) || 1

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">
      <PersonProfileCard
        user={user}
        patient={patient ?? undefined}
        doctor={doctor ?? undefined}
      />

      {isDoctor && doctor && (
        <>
          <DoctorSchedule
            fetchAppointments={(params) => getByDateAdmin(user.id, params)}
            onAppointmentClick={(a) => router.push(`/admin/appointments/${a.id}`)}
          />
          <DoctorRatings
            ratings={ratings}
            page={ratingsPage}
            totalPages={totalPages}
            onPageChange={setRatingsPage}
            onDeleteSuccess={(ratingId) => deleteRating.mutate(ratingId)}
          />
          <DoctorSpecialitiesCard
            specialities={doctor.specialities}
            onAdd={async (s) => { await addSpecialityByAdmin(doctor.userId, { speciality: s }) }}
            onRemove={async (s) => { await removeSpecialityByAdmin(doctor.userId, { speciality: s }) }}
          />
        </>
      )}

      <AdminUserEditForm
        userId={user.id}
        defaultValues={{
          firstName: user.firstName,
          lastName: user.lastName,
        }}
      />
    </div>
  )
}