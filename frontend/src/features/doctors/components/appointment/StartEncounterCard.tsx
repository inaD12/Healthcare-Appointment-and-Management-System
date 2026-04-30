"use client"

import { Card, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

import {
  Appointment,
  EncounterDetails,
  EncounterStatus,
} from "@/features/patients/types/patientsTypes"

import { patientService } from "@/features/patients/services/patientService"

export default function StartEncounterCard({
  appointment,
  setEncounter,
}: {
  appointment: Appointment
  setEncounter: React.Dispatch<React.SetStateAction<EncounterDetails | null>>
}) {
  const handleStartEncounter = async () => {
    const res = await patientService.startEncounter({
      appointmentId: appointment.id,
    })

    const encounterId = res.data.data.id

    setEncounter({
      id: encounterId,
      startedAt: new Date().toISOString(),
      status: EncounterStatus.InProgress,
      notes: [],
      diagnoses: [],
      prescriptions: [],
      addendums: [],
    })
  }

  return (
    <Card className="border-dashed border-2 bg-muted/30">
      <CardContent className="p-10 text-center space-y-4">
        <p className="text-lg font-medium">No encounter started</p>

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