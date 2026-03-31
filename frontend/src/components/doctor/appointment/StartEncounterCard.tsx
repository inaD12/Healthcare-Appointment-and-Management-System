"use client"

import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

import { Appointment, EncounterStatus } from "@/features/patients/types/patientTypes"
import { patientService } from "@/features/patients/services/patientService"

export default function StartEncounterCard({
  appointment,
  setEncounterId,
  setEncounterStatus
}: {
  appointment: Appointment
  setEncounterId: (id: string) => void
  setEncounterStatus: (status: EncounterStatus) => void
}) {

  const handleStartEncounter = async () => {
    const res = await patientService.startEncounter({
      appointmentId: appointment.id
    })

    setEncounterId(res.data.data.id)
    setEncounterStatus(EncounterStatus.InProgress)
  }

  return (
    <Card className="border-dashed border-2 bg-muted/30">

      <CardContent className="p-10 text-center space-y-4">

        <p className="text-lg font-medium">
          No encounter started
        </p>

        <p className="text-sm text-muted-foreground">
          Start the patient encounter to begin recording notes, diagnoses and prescriptions.
        </p>

        <Button onClick={handleStartEncounter}>
          Start Encounter
        </Button>

      </CardContent>

    </Card>
  )
}