"use client"

import { useState } from "react"

import { Appointment, EncounterStatus, AppointmentStatus, Note, Diagnosis, Prescription, Addendum } from "@/features/patients/types/patientTypes"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"

import AppointmentHeader from "@/components/doctor/appointment/AppointmentHeader"
import AppointmentInfoCard from "@/components/doctor/appointment/AppointmentInfoCard"
import RatingCard from "@/components/doctor/appointment/RatingCard"
import StartEncounterCard from "@/components/doctor/appointment/StartEncounterCard"
import EncounterCard from "@/components/doctor/appointment/EncounterCard"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

export default function DoctorAppointmentClient({
  initialAppointment,
  initialRating
}: {
  initialAppointment: Appointment | null
  initialRating: RatingQueryViewModel | null
}) {
  if (!initialAppointment) {
    return (
      <div className="p-6">
        <p className="text-red-600 font-medium">Error: Appointment not found.</p>
      </div>
    )
  }

  useAuthGuard();

  const [appointment] = useState(initialAppointment)
  const [rating] = useState(initialRating)

  const [encounterId, setEncounterId] = useState<string | null>(
    appointment?.encounterDetails?.id ?? null
  )

  const [encounterStatus, setEncounterStatus] = useState<EncounterStatus | null>(
    appointment?.encounterDetails?.status ?? null
  )

  const [notes, setNotes] = useState<Note[]>(
    appointment?.encounterDetails?.notes ?? []
  )

  const [diagnoses, setDiagnoses] = useState<Diagnosis[]>(
    appointment?.encounterDetails?.diagnoses ?? []
  )

  const [prescriptions, setPrescriptions] = useState<Prescription[]>(
    appointment?.encounterDetails?.prescriptions ?? []
  )

  const [addendums, setAddendums] = useState<Addendum[]>(
    appointment?.encounterDetails?.addendums ?? []
  )

  const canUseEncounter =
    appointment?.status === AppointmentStatus.Scheduled ||
    appointment?.status === AppointmentStatus.Completed

  return (
    <div className="max-w-5xl mx-auto p-6 space-y-6">

      <AppointmentHeader appointment={appointment} />

      <AppointmentInfoCard appointment={appointment} />

      {rating && <RatingCard rating={rating} />}

      {canUseEncounter && !encounterId && (
        <StartEncounterCard
          appointment={appointment}
          setEncounterId={setEncounterId}
          setEncounterStatus={setEncounterStatus}
        />
      )}

      {canUseEncounter && encounterId && (
        <EncounterCard
          encounterId={encounterId}
          encounterStatus={encounterStatus}
          setEncounterStatus={setEncounterStatus}
          notes={notes}
          setNotes={setNotes}
          diagnoses={diagnoses}
          setDiagnoses={setDiagnoses}
          prescriptions={prescriptions}
          setPrescriptions={setPrescriptions}
          addendums={addendums}
          setAddendums={setAddendums}
        />
      )}

    </div>
  )
}