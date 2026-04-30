"use client"

import { Appointment, AppointmentStatus } from "@/features/patients/types/patientsTypes"
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

const statusStyles: Record<AppointmentStatus, string> = {
  [AppointmentStatus.Scheduled]: "bg-blue-100 text-blue-700",
  [AppointmentStatus.Rescheduled]: "bg-yellow-100 text-yellow-700",
  [AppointmentStatus.Cancelled]: "bg-red-100 text-red-700",
  [AppointmentStatus.Completed]: "bg-green-100 text-green-700",
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
    return (
      <p className="text-sm text-muted-foreground py-6 text-center">
        {emptyText}
      </p>
    )
  }

  return (
    <div className="space-y-3">
      {appointments.map((a) => (
        <Card
          key={a.id}
          onClick={() => onSelectAppointment?.(a)}
          className={`p-4 border transition-all duration-150
            ${onSelectAppointment ? "cursor-pointer hover:shadow-md hover:border-gray-300" : ""}
          `}
        >
          <div className="flex items-start justify-between gap-4">

            <div className="space-y-1">
              <div className="flex items-center gap-2">
                <span
                  className={`text-xs font-medium px-2 py-0.5 rounded-full ${
                    statusStyles[a.status]
                  }`}
                >
                  {a.status}
                </span>

                <span className="text-sm font-medium text-gray-800">
                  Dr. {a.doctorName || "Unknown"}
                </span>
              </div>

              <p className="text-sm text-gray-600">
                {new Date(a.start).toLocaleString()} →{" "}
                {new Date(a.end).toLocaleString()}
              </p>
            </div>

            {onSelectAppointment && (
              <div className="text-xs text-gray-400 self-center">
                View →
              </div>
            )}
          </div>
        </Card>
      ))}

      {showLoadMore && (
        <div className="flex justify-center pt-4">
          <Button
            size="sm"
            variant="outline"
            onClick={() => onLoadMore?.()}
            disabled={loadingMore}
          >
            {loadingMore ? "Loading..." : "Load more"}
          </Button>
        </div>
      )}
    </div>
  )
}