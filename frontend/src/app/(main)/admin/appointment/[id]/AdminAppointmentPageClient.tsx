"use client"

import { useState } from "react"

import {
  AppointmentStatus,
  EncounterDetails,
  EncounterStatus,
} from "@/features/patients/types/patientTypes"

import AppointmentHeader from "@/components/doctor/appointment/AppointmentHeader"
import AppointmentInfoCard from "@/components/doctor/appointment/AppointmentInfoCard"
import AppointmentRatingCard from "@/features/ratings/components/AppointmentRatingCard"
import EncounterCard from "@/components/doctor/appointment/EncounterCard"

import { removeRating } from "@/features/ratings/services/ratingService"
import { patientService } from "@/features/patients/services/patientService"

import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { ROLES } from "@/features/users/types/userTypes"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

export default function AdminAppointmentPageClient({
  initialAppointment,
  initialRating,
}: any) {
  const [appointment] = useState(initialAppointment)
  const [rating, setRating] = useState(initialRating)

  const [encounter, setEncounter] = useState<EncounterDetails | null>(
    appointment?.encounterDetails ?? null
  )

  useRequireRole(ROLES.ADMIN)

  const updateEncounter = (patch: Partial<EncounterDetails>) => {
    if (!encounter) return
    setEncounter({ ...encounter, ...patch })
  }

  const handleDeleteRating = async () => {
    if (!rating?.id) return
    await removeRating(rating.id)
    setRating(null)
  }

  const handleUnlockEncounter = async () => {
    if (!encounter) return

    await patientService.unlockEncounter(encounter.id)

    updateEncounter({
      status: EncounterStatus.Finalized,
    })
  }

  const handleUnfinalizeEncounter = async () => {
    if (!encounter) return

    await patientService.unfinalizeEncounter(encounter.id)

    updateEncounter({
      status: EncounterStatus.InProgress,
    })
  }

  if (!appointment) {
    return <div className="p-6 text-red-600">Failed to load appointment</div>
  }

  return (
    <div className="max-w-4xl mx-auto p-6 space-y-8">

      <AppointmentHeader appointment={appointment} />

      <AppointmentInfoCard appointment={appointment} />

      {appointment.status === AppointmentStatus.Completed && (
        <AppointmentRatingCard
          rating={rating}
          onDelete={handleDeleteRating}
        />
      )}

      {encounter && (
        <EncounterCard
          encounter={encounter}
          encounterId={encounter.id}
          updateEncounter={() => {}}
        />
      )}

      {encounter &&
        (encounter.status === EncounterStatus.Locked ||
          encounter.status === EncounterStatus.Finalized) && (
          <Card>
            <CardHeader>
              <CardTitle>Encounter Controls</CardTitle>
            </CardHeader>

            <CardContent className="flex gap-3">
              {encounter.status === EncounterStatus.Locked && (
                <Button
                  variant="secondary"
                  onClick={handleUnlockEncounter}
                >
                  Unlock Encounter
                </Button>
              )}

              {encounter.status === EncounterStatus.Finalized && (
                <Button
                  variant="secondary"
                  onClick={handleUnfinalizeEncounter}
                >
                  Unfinalize Encounter
                </Button>
              )}
            </CardContent>
          </Card>
      )}

    </div>
  )
}