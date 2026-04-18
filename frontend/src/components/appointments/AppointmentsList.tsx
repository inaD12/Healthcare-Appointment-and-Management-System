"use client"

import { Appointment } from "@/features/patients/types/patientTypes"
import { Card } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

type Props = {
  appointments: Appointment[]
  emptyText?: string

  showLoadMore?: boolean
  loadingMore?: boolean
  onLoadMore?: (after?: string | null) => Promise<any>

  onSelectAppointment?: (appointment: Appointment) => void
}

export function AppointmentList({
  appointments,
  emptyText = "No appointments found.",
  showLoadMore,
  loadingMore,
  onLoadMore,
  onSelectAppointment,
}: Props) {
  if (!appointments.length) {
    return <p className="text-muted-foreground">{emptyText}</p>
  }

  return (
    <div className="space-y-3">
      {appointments.map((a) => (
        <Card
          key={a.id}
          className={`p-4 transition ${
            onSelectAppointment ? "cursor-pointer hover:shadow-md" : ""
          }`}
          onClick={() => onSelectAppointment?.(a)}
        >
          <div className="space-y-1">
            <p><strong>Status:</strong> {a.status}</p>

            <p>
              <strong>Time:</strong>{" "}
              {new Date(a.start).toLocaleString()} →{" "}
              {new Date(a.end).toLocaleString()}
            </p>

            <p><strong>Doctor:</strong> {a.doctorName}</p>
          </div>
        </Card>
      ))}

      {showLoadMore && (
        <div className="flex justify-center pt-4">
          <Button
            size="sm"
            onClick={() => onLoadMore?.()}
            disabled={loadingMore}
          >
            {loadingMore ? "Loading..." : "Load More"}
          </Button>
        </div>
      )}
    </div>
  )
}