import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Appointment } from "@/features/patients/types/patientTypes"

export default function AppointmentInfoCard({ appointment }: { appointment: Appointment }) {

  return (
    <Card className="shadow-sm">

      <CardHeader>
        <CardTitle>
          Patient: {appointment.patientName}
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