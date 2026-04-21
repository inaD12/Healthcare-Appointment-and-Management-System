"use client"

import { useState } from "react"

import {
  Appointment,
  EncounterDetails,
  AppointmentStatus,
} from "@/features/patients/types/patientTypes"

import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"

import AppointmentHeader from "@/components/doctor/appointment/AppointmentHeader"
import AppointmentInfoCard from "@/components/doctor/appointment/AppointmentInfoCard"
import RatingCard from "@/components/doctor/appointment/RatingCard"
import StartEncounterCard from "@/components/doctor/appointment/StartEncounterCard"
import EncounterCard from "@/components/doctor/appointment/EncounterCard"

import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { ROLES } from "@/features/users/types/userTypes"

export default function DoctorAppointmentClient({
  initialAppointment,
  initialRating,
}: {
  initialAppointment: Appointment | null
  initialRating: RatingQueryViewModel | null
}) {
  if (!initialAppointment) {
    return (
      <div className="p-6">
        <p className="text-red-600 font-medium">
          Error: Appointment not found.
        </p>
      </div>
    )
  }

  useRequireRole(ROLES.DOCTOR)

  const [appointment] = useState(initialAppointment)
  const [rating] = useState(initialRating)

  const [encounter, setEncounter] = useState<EncounterDetails | null>(
    appointment.encounterDetails ?? null
  )

  const updateEncounter = (patch: Partial<EncounterDetails>) => {
    if (!encounter) return
    setEncounter({ ...encounter, ...patch })
  }

  const canUseEncounter =
    appointment.status === AppointmentStatus.Scheduled ||
    appointment.status === AppointmentStatus.Completed

  return (
    <div className="max-w-5xl mx-auto p-6 space-y-6">
      <AppointmentHeader appointment={appointment} />

      <AppointmentInfoCard appointment={appointment} />

      {rating && <RatingCard rating={rating} />}

      {canUseEncounter && !encounter && (
        <StartEncounterCard
          appointment={appointment}
          setEncounter={setEncounter}
        />
      )}

      {canUseEncounter && encounter && (
        <EncounterCard
          encounter={encounter}
          encounterId={encounter.id}
          updateEncounter={updateEncounter}
        />
      )}
    </div>
  )
} 