import { Badge } from "@/components/ui/badge"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { Appointment } from "@/features/patients/types/patientTypes"

export default function AppointmentHeader({
  appointment,
}: {
  appointment: Appointment
}) {
  const { isDoctor, isPatient, isAdmin } = useAuth()

  const getTitle = () => {
    if (isDoctor) return "Appointment"
    if (isPatient) return "Appointment"
    if (isAdmin) return "Appointment Overview"
    return "Appointment"
  }

  const getSubtitle = () => {
    if (isDoctor) {
      return "Manage encounter, notes, diagnoses and prescriptions"
    }
    if (isPatient) {
      return "View your visit details and medical records"
    }
    if (isAdmin) {
      return "System-wide appointment monitoring and management"
    }
    return ""
  }

  const getStatusLabel = () => {
    if (isDoctor) return `Status: ${appointment.status}`
    if (isPatient) return `Visit ${appointment.status.toLowerCase()}`
    if (isAdmin) return `System status: ${appointment.status}`
    return appointment.status
  }

  return (
    <div className="flex items-center justify-between">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">{getTitle()}</h1>
        <p className="text-muted-foreground text-sm">{getSubtitle()}</p>
      </div>

      <Badge className="px-4 py-1 text-sm">{getStatusLabel()}</Badge>
    </div>
  )
}