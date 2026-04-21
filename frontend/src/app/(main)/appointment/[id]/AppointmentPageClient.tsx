"use client"

import { useState } from "react"

import {
  AppointmentStatus,
  EncounterDetails,
} from "@/features/patients/types/patientTypes"

import AppointmentHeader from "@/components/doctor/appointment/AppointmentHeader"
import AppointmentInfoCard from "@/components/doctor/appointment/AppointmentInfoCard"
import AppointmentRatingCard from "@/features/ratings/components/AppointmentRatingCard"
import EncounterCard from "@/features/encounters/components/EncounterCard"
import { addRating, editRating, removeRating } from "@/features/ratings/services/ratingService"
import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { ROLES } from "@/features/users/types/userTypes"

export default function AppointmentPageClient({
  initialAppointment,
  initialRating,
}: any) {
  const [appointment] = useState(initialAppointment)
  const [rating, setRating] = useState(initialRating)

  const encounter = appointment?.encounterDetails ?? null

  useRequireRole(ROLES.PATIENT)
  
  const handleCreateRating = async (data: { score: number; comment: string }) => {
    const res = await addRating({
      AppointmentId: appointment.id,
      Score: data.score,
      Comment: data.comment,
    })

    setRating({
      id: res.data.data.id,
      appointmentId: appointment.id,
      doctorId: "",
      patientId: "",
      score: data.score,
      comment: data.comment,
      createdAt: new Date().toISOString(),
    })
  }

  const handleEditRating = async (data: { score: number; comment: string }) => {
    if (!rating?.id) return

    await editRating(rating.id, {
      Score: data.score,
      Comment: data.comment,
    })

    setRating({
      ...rating,
      score: data.score,
      comment: data.comment,
    })
  }

  const handleDeleteRating = async () => {
    if (!rating?.id) return

    await removeRating(rating.id)
    setRating(null)
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
          onCreate={handleCreateRating}
          onEdit={handleEditRating}
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

    </div>
  )
}