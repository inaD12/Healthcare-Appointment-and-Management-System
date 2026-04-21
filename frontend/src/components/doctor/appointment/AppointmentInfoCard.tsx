import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { Appointment } from "@/features/patients/types/patientTypes"

export default function AppointmentInfoCard({
  appointment,
}: {
  appointment: Appointment
}) {
  const { isDoctor, isPatient, isAdmin } = useAuth()

  const getTitle = () => {
    if (isDoctor) return `Patient Appointment`
    if (isPatient) return `Your Appointment`
    if (isAdmin) return `Appointment Details`
    return "Appointment"
  }

  const getPartyInfo = () => {
    if (isDoctor) {
      return `Patient: ${appointment.patientName}`
    }
    if (isPatient) {
      return `Doctor: ${appointment.doctorName}`
    }
    return null
  }

  const partyInfo = getPartyInfo()

  return (
    <Card className="shadow-sm">
      <CardHeader>
        <CardTitle>
          {getTitle()}
          {partyInfo && (
            <span className="block text-sm font-normal text-muted-foreground mt-1">
              {partyInfo}
            </span>
          )}
        </CardTitle>
      </CardHeader>

      <CardContent className="grid grid-cols-2 gap-4 text-sm">
        <div className="bg-muted/40 p-3 rounded-md">
          <p className="text-muted-foreground text-xs">Start Time</p>
          <p className="font-medium">
            {new Date(appointment.start).toLocaleString()}
          </p>
        </div>

        <div className="bg-muted/40 p-3 rounded-md">
          <p className="text-muted-foreground text-xs">End Time</p>
          <p className="font-medium">
            {new Date(appointment.end).toLocaleString()}
          </p>
        </div>
      </CardContent>
    </Card>
  )
}