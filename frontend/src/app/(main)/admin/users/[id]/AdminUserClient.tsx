"use client"
import { useRouter } from "next/navigation"
import { useState, useCallback } from "react"
import DoctorSchedule from "@/features/doctors/components/DoctorSchedule"
import { DoctorRatings } from "@/features/ratings/components/DoctorRatings"
import { DoctorSpecialitiesCard } from "@/features/doctors/components/DoctorSpecialitiesCard"
import { PersonProfileCard } from "@/components/profile/PersonProfileCard"
import { AdminUserEditForm } from "@/features/admin/components/AdminUserEditForm"
import { useDeleteRating } from "@/features/admin/hooks/useDeleteRating"
import { useDoctorRatings } from "@/features/admin/hooks/useDoctorRatings"
import { PatientProfile } from "@/features/patients/types/patientsTypes"
import { ROLES, UserQueryResponse } from "@/features/users/types/usersTypes"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"
import { AppointmentList } from "@/features/appointments/components/AppointmentsList"
import { PatientMedicalCard } from "@/features/patients/components/PatientMedicalCard"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { patientService } from "@/features/patients/services/patientService"
import { useAppointmentsPagination } from "@/features/appointments/hooks/useAppointmentsPagination"
import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { doctorService } from "@/features/doctors/services/doctorService"
import { appointmentService } from "@/features/appointments/services/appointmentService"

interface AdminUserClientProps {
  user: UserQueryResponse
  doctor: DoctorQueryViewModel | null
  patient: PatientProfile | null
  initialRatings: RatingQueryViewModel[]
  initialRatingsTotalPages: number
  initialAppointments: any
}

export default function AdminUserClient({
  user,
  doctor,
  patient,
  initialRatings,
  initialRatingsTotalPages,
  initialAppointments
}: AdminUserClientProps) {
  useRequireRole(ROLES.ADMIN)
  
  const router = useRouter()
  const [ratingsPage, setRatingsPage] = useState(1)

  const isDoctor = user?.roles?.includes("Doctor")
  const isPatient = user?.roles?.includes("Patient")

  const { data: ratingsData } = useDoctorRatings(user.id, ratingsPage, {
    items: initialRatings,
    total: initialRatingsTotalPages * 4,
  })

  const deleteRating = useDeleteRating()

  const ratings = ratingsData?.items ?? []
  const totalPages = Math.ceil((ratingsData?.total ?? 0) / 4) || 1

  const fetchAppointments = useCallback(
    (after?: string) => patientService.getPatientAppointmentsPage(user.id, 5, after),
    [user.id]
  )

  const { appointments, hasNextPage, loadingMore, loadMore } = useAppointmentsPagination(
    fetchAppointments,
    5,
    initialAppointments ?? undefined
    )

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">
      <PersonProfileCard
        user={user}
        patient={patient ?? undefined}
        doctor={doctor ?? undefined}
      />

      {isPatient && patient && (
        <>
          <Card>
            <CardHeader>
              <CardTitle>Appointments</CardTitle>
            </CardHeader>
            <CardContent>
              <AppointmentList
                appointments={appointments}
                showLoadMore={hasNextPage}
                loadingMore={loadingMore}
                onLoadMore={loadMore}
                onSelectAppointment={(a) => router.push(`/admin/appointment/${a.id}`)}
              />
            </CardContent>
          </Card>

          <PatientMedicalCard
            patientId={user.id}
            allergies={patient.allergies}
            conditions={patient.conditions}
            onAddAllergy={patientService.addAllergyByAdmin}
            onRemoveAllergy={patientService.removeAllergyByAdmin}
            onAddCondition={patientService.addChronicConditionByAdmin}
            onRemoveCondition={patientService.removeChronicConditionByAdmin}
          />
        </>
      )}

      {isDoctor && doctor && (
        <>
          <DoctorSchedule
            fetchAppointments={(params) => appointmentService.getByDateAdmin(user.id, params)}
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
            onAdd={async (s) => { await doctorService.addSpecialityByAdmin(doctor.userId, { speciality: s }) }}
            onRemove={async (s) => { await doctorService.removeSpecialityByAdmin(doctor.userId, { speciality: s }) }}
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