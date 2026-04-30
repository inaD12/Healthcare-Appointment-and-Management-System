"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"

import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"
import { AppointmentResponse } from "@/features/appointments/types/appointmentsTypes"

import DoctorSchedule from "@/features/doctors/components/DoctorSchedule"
import { PersonProfileCard } from "@/components/profile/PersonProfileCard"
import DoctorBioCard from "@/features/doctors/components/profile/DoctorBioCard"
import DoctorWorkDaysCard from "@/features/doctors/components/profile/DoctorWorkDaysCard"
import DoctorAvailabilityExceptionsCard from "@/features/doctors/components/profile/DoctorAvailabilityExceptionsCard"
import { useRequireRole } from "@/features/auth/hooks/useRequireRole"
import { ROLES } from "@/features/users/types/usersTypes"
import { appointmentService } from "@/features/appointments/services/appointmentService"


type Props = {
  doctor: DoctorQueryViewModel
  appointments: AppointmentResponse[]
}

export default function DoctorProfileClient({
  doctor: initialDoctor,
  appointments
}: Props) {

  useRequireRole(ROLES.DOCTOR)

  const router = useRouter()

  const [doctor, setDoctor] = useState(initialDoctor)

  return (
    <div className="space-y-8 max-w-6xl mx-auto p-6">

      <PersonProfileCard doctor={doctor} />

      <DoctorBioCard
        doctor={doctor}
        setDoctor={setDoctor}
      />

      <DoctorSchedule
        initialAppointments={appointments}
        fetchAppointments={appointmentService.getMyAppointments}
        onAppointmentClick={(appointment) =>
          router.push(`/doctors/appointment/${appointment.id}`)
        }
      />

      <DoctorWorkDaysCard
        doctor={doctor}
        setDoctor={setDoctor}
      />

      <DoctorAvailabilityExceptionsCard
        doctor={doctor}
        setDoctor={setDoctor}
      />

    </div>
  )
}