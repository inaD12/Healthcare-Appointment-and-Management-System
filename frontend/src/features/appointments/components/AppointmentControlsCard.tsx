"use client"

import { useRouter } from "next/navigation"
import { useState } from "react"
import { toast } from "sonner"

import { appointmentService } from "@/features/appointments/services/appointmentService"
import { AppointmentStatus } from "@/features/patients/types/patientsTypes"
import { useAuth } from "@/features/auth/hooks/useAuth"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

type Props = {
  appointmentId: string
  doctorUserId: string
  status: AppointmentStatus
  onStatusChange?: (status: AppointmentStatus) => void
}

export default function AppointmentControlsCard({
  appointmentId,
  doctorUserId,
  status,
  onStatusChange,
}: Props) {
  const router = useRouter()
  const auth = useAuth()
  const isAdmin = auth?.isAdmin

  const [loading, setLoading] = useState(false)

  const handleCancel = async () => {
    const confirmed = window.confirm(
      "Are you sure you want to cancel this appointment?"
    )

    if (!confirmed) return

    setLoading(true)

    try {
      if (isAdmin) {
        await appointmentService.cancelAppointmentByAdmin(appointmentId)
      } else {
        await appointmentService.cancelAppointment(appointmentId)
      }

      onStatusChange?.(AppointmentStatus.Cancelled)

      toast.success("Appointment cancelled successfully")
    } catch {
      toast.error("Failed to cancel appointment")
    } finally {
      setLoading(false)
    }
  }

  const handleReschedule = () => {
    router.push(`/doctors/${doctorUserId}?rescheduleId=${appointmentId}`)
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Appointment Controls</CardTitle>
      </CardHeader>

      <CardContent className="flex gap-3">
        {status !== AppointmentStatus.Cancelled && (
          <Button
            variant="destructive"
            onClick={handleCancel}
            disabled={loading}
          >
            Cancel
          </Button>
        )}

        {status === AppointmentStatus.Scheduled && (
          <Button variant="secondary" onClick={handleReschedule}>
            Reschedule
          </Button>
        )}
      </CardContent>
    </Card>
  )
}