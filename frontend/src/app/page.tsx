import AdminHome from "@/features/home/AdminHome"
import { DoctorHome } from "@/features/home/DoctorHome"
import PatientHome from "@/features/home/PatientHome"
import QuickActions from "@/features/home/QuickActions"
import UnauthenticatedHome from "@/features/home/UnauthenticatedHome"

import {
  Card,
  CardHeader,
  CardTitle,
  CardDescription,
  CardContent,
} from "@/components/ui/card"

import { patientService } from "@/features/patients/services/patientService"
import { getCurrentUser } from "@/features/users/services/userService"

export default async function HomePage() {
  let user = null

  try {
    const res = await getCurrentUser()
    user = res.data.data
  } catch (err: any) {
    user = null
  }

  if (!user) {
  return <UnauthenticatedHome />
  }

  const roles = user.roles ?? []

  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  const patientDashboard = isPatient
    ? await patientService.getPatientDashboard()
    : null

  const doctorDashboard =
    isDoctor && user?.id
      ? await patientService.getDoctorDashboard(user.id)
      : null

  const adminDashboard = isAdmin
    ? await patientService.getAdminDashboard()
    : null

  return (
    <div className="space-y-6">

      <Card>
        <CardHeader>
          <CardTitle className="text-2xl">
            Welcome back {user?.firstName ?? ""}
          </CardTitle>

          <CardDescription>
            Here is an overview of your dashboard.
          </CardDescription>
        </CardHeader>
      </Card>

      <Card>
        <CardContent className="space-y-8 pt-6">

          {isPatient && patientDashboard && (
            <PatientHome dashboard={patientDashboard} />
          )}

          {isDoctor && doctorDashboard && (
            <DoctorHome
              doctorDashboard={doctorDashboard}
              userId={user.id}
            />
          )}

          {isAdmin && adminDashboard && (
            <AdminHome dashboard={adminDashboard} />
          )}

          <QuickActions roles={roles} />

        </CardContent>
      </Card>

    </div>
  )
}