"use client"

import { useEffect, useState } from "react"
import { Button } from "@/components/ui/button"
import { Card } from "@/components/ui/card"
import { ChevronLeft, ChevronRight } from "lucide-react"

import { getMyAppointments } from "@/features/appointments/services/appointmentService"
import { AppointmentResponse } from "@/features/appointments/types/appointmentsTypes"

type Props = {
  onAppointmentClick?: (appointment: AppointmentResponse) => void
}

export default function DoctorSchedule({ onAppointmentClick }: Props) {
  const [appointments, setAppointments] = useState<AppointmentResponse[]>([])
  const [weekStart, setWeekStart] = useState(getStartOfWeek(new Date()))

  function getStartOfWeek(date: Date) {
    const d = new Date(date)
    const day = d.getDay()
    d.setDate(d.getDate() - day)
    d.setHours(0,0,0,0)
    return d
  }

  const getEndOfWeek = (start: Date) => {
    const end = new Date(start)
    end.setDate(start.getDate() + 7)
    return end
  }

  const days = Array.from({ length: 7 }).map((_, i) => {
    const d = new Date(weekStart)
    d.setDate(weekStart.getDate() + i)
    return d
  })

  useEffect(() => {

    const fetchAppointments = async () => {
      try {

        const formatDateOnly = (date: Date) =>
            date.toISOString().split("T")[0]

            const res = await getMyAppointments({
            startDate: formatDateOnly(weekStart),
            endDate: formatDateOnly(getEndOfWeek(weekStart))
            })

        const data = res.data.data ?? res.data

        const sorted = data.sort(
          (a: AppointmentResponse, b: AppointmentResponse) =>
            new Date(a.duration.start).getTime() -
            new Date(b.duration.start).getTime()
        )

        setAppointments(sorted)

      } catch (err: any) {
        if (err.response?.status !== 404) {
              console.error(err)
            }
      }
    }

    fetchAppointments()

  }, [weekStart])

  const appointmentsByDay = (day: Date) =>
    appointments.filter(a => {
      const d = new Date(a.duration.start)
      return d.toDateString() === day.toDateString()
    })

  const nextWeek = () => {
    const d = new Date(weekStart)
    d.setDate(d.getDate() + 7)
    setWeekStart(d)
  }

  const prevWeek = () => {
    const d = new Date(weekStart)
    d.setDate(d.getDate() - 7)
    setWeekStart(d)
  }

  return (
    <Card className="p-6 space-y-6">


      <div className="flex items-center justify-between">

        <Button variant="outline" onClick={prevWeek}>
          <ChevronLeft className="w-4 h-4 mr-2" />
          Previous
        </Button>

        <h2 className="text-lg font-semibold">
          Week of {weekStart.toLocaleDateString()}
        </h2>

        <Button variant="outline" onClick={nextWeek}>
          Next
          <ChevronRight className="w-4 h-4 ml-2" />
        </Button>

      </div>


      <div className="grid grid-cols-7 gap-4">

        {days.map(day => (
          <div key={day.toISOString()} className="space-y-3">


            <div className="text-center font-medium border-b pb-1">
              <p className="text-sm text-muted-foreground">
                {day.toLocaleDateString(undefined, { weekday: "short" })}
              </p>

              <p className="text-sm">
                {day.getDate()}
              </p>
            </div>


            <div className="space-y-2">

              {appointmentsByDay(day).length === 0 && (
                <p className="text-xs text-muted-foreground text-center">
                  —
                </p>
              )}

              {appointmentsByDay(day).map(a => {

                const start = new Date(a.duration.start)

                return (
                  <button
                    key={a.id}
                    onClick={() => onAppointmentClick?.(a)}
                    className="w-full text-left rounded-md border p-2 text-xs hover:bg-muted transition"
                  >

                    <div className="font-medium">
                      {start.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}
                    </div>

                    <div className="text-muted-foreground">
                      {a.status}
                    </div>

                  </button>
                )
              })}

            </div>

          </div>
        ))}

      </div>

    </Card>
  )
}