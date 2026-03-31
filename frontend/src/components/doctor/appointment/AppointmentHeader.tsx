import { Badge } from "@/components/ui/badge"
import { Appointment } from "@/features/patients/types/patientTypes"

export default function AppointmentHeader({ appointment }: { appointment: Appointment }) {

  return (
    <div className="flex items-center justify-between">

      <div>
        <h1 className="text-3xl font-bold tracking-tight">Appointment</h1>
        <p className="text-muted-foreground text-sm">
          Manage patient encounter, notes, diagnoses and prescriptions
        </p>
      </div>

      <Badge className="px-4 py-1 text-sm">
        {appointment.status}
      </Badge>

    </div>
  )
}