"use client"

import { useParams, useRouter } from "next/navigation"
import { useEffect, useState, useCallback } from "react"

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

import { getUserByAdmin } from "@/features/users/services/userService"
import { patientService } from "@/features/patients/services/patientService"
import {
  getDoctorByUserId,
  addSpecialityByAdmin,
  removeSpecialityByAdmin,
} from "@/features/doctors/services/doctorService"

import { AdminUserEditForm } from "@/components/admin/AdminUserEditForm"
import { AppointmentList } from "@/components/appointments/AppointmentsList"
import { useAppointmentsPagination } from "@/components/appointments/useAppointmentsPagination"
import { PersonProfileCard } from "@/components/profile/PersonProfileCard"
import { PatientMedicalCard } from "@/components/patient/PatientMedicalCard"
import { DoctorSpecialitiesCard } from "@/components/doctor/DoctorSpecialitiesCard"

import {
  UserQueryResponse,
  Roles,
  ROLES,
} from "@/features/users/types/userTypes"

import { PatientProfile } from "@/features/patients/types/patientTypes"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import DoctorSchedule from "@/components/schedule/DoctorSchedule"
import { getByDateAdmin } from "@/features/appointments/services/appointmentService"

export default function AdminUserPage() {
  const { id } = useParams<{ id: string }>()
  const router = useRouter()

  const [user, setUser] = useState<UserQueryResponse | null>(null)
  const [patient, setPatient] = useState<PatientProfile | null>(null)
  const [doctor, setDoctor] = useState<DoctorQueryViewModel | null>(null)
  const [loading, setLoading] = useState(true)

  useEffect(() => {
    const loadUser = async () => {
      setLoading(true)

      const userRes = await getUserByAdmin(id)
      const userData = userRes.data.data

      setUser(userData)

      const roles = userData.roles

      if (roles.includes(ROLES.PATIENT)) {
        const patientRes = await patientService.getPatientInfoInitial(id, 5)
        setPatient(patientRes.profile)
      }

      if (roles.includes(ROLES.DOCTOR)) {
        const doctorRes = await getDoctorByUserId(id)
        setDoctor(doctorRes.data.data)
      }

      setLoading(false)
    }

    loadUser()
  }, [id])

  const fetchAppointments = useCallback(
    (after?: string) =>
      patientService.getPatientAppointmentsPage(id, 5, after),
    [id]
  )

  const {
    appointments,
    loading: appointmentsLoading,
    hasNextPage,
    loadingMore,
    loadMore,
  } = useAppointmentsPagination(fetchAppointments, 5)

  if (loading) return <div className="p-8">Loading...</div>
  if (!user) return <div className="p-8">User not found</div>

  const isPatient = user.roles.includes("Patient")
  const isDoctor = user.roles.includes("Doctor")

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">

      <PersonProfileCard user={user} patient={patient ?? undefined} doctor={doctor ?? undefined} />

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
                onSelectAppointment={(a) =>
                  router.push(`/admin/appointments/${a.id}`)
                }
              />
            </CardContent>
          </Card>

          <PatientMedicalCard
            patientId={id}
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
            fetchAppointments={(params) => getByDateAdmin(id, params)}
            onAppointmentClick={(appointment) => {
                router.push(`/admin/appointments/${appointment.id}`)
            }}
          />

          <DoctorSpecialitiesCard
            specialities={doctor.specialities}
            onAdd={async (s) => {
              await addSpecialityByAdmin(doctor.userId, { speciality: s })
              return
            }}

            onRemove={async (s) => {
              await removeSpecialityByAdmin(doctor.userId, { speciality: s })
              return
            }}
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