"use client"

import { useParams, useRouter } from "next/navigation"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"

import { getUserByAdmin } from "@/features/users/services/userService"
import { patientService } from "@/features/patients/services/patientService"

import { AdminUserEditForm } from "@/components/admin/AdminUserEditForm"
import { AppointmentList } from "@/components/appointments/AppointmentsList"
import { useAppointmentsPagination } from "@/components/appointments/useAppointmentsPagination"
import { useState, useEffect, useCallback } from "react"
import { PatientInfoCard } from "@/components/patients/PatientInfoCard"

export default function AdminUserPage() {
  const { id } = useParams<{ id: string }>()

  const router = useRouter()

  const fetchAppointments = useCallback(
  (after?: string) =>
    patientService.getPatientAppointmentsPage(id, 5, after),
  [id]
)

  const {
    appointments,
    loading,
    hasNextPage,
    loadingMore,
    loadMore,
  } = useAppointmentsPagination(fetchAppointments, 5)

  const [user, setUser] = useState<any>(null)
  const [patient, setPatient] = useState<any>(null)

  useEffect(() => {
    const load = async () => {
      const userRes = await getUserByAdmin(id)
      const patientRes = await patientService.getPatientInfoInitial(id, 5)

      setUser(userRes.data.data)
      setPatient(patientRes.profile)
    }

    load()
  }, [id])

  if (loading) return <div className="p-8">Loading...</div>
  if (!user) return <div className="p-8">User not found</div>

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">

      <Card>
        <CardHeader>
          <CardTitle>User Details</CardTitle>
        </CardHeader>

        <CardContent>
          <p><strong>Email:</strong> {user.email}</p>

          <div className="flex gap-2">
            {user.roles.map((r: string) => (
              <Badge key={r}>{r}</Badge>
            ))}
          </div>
        </CardContent>
      </Card>

      {patient && (
        <PatientInfoCard patient={patient} />
      )}

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

      <Card>
         <CardContent>
           <AdminUserEditForm
            userId={user.id}
             defaultValues={{ firstName: user.firstName, lastName: user.lastName, }}
            /> 
          </CardContent>
       </Card>

    </div>
  )
}