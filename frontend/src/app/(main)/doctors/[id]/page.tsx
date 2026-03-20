"use client"

import { useEffect, useMemo, useState } from "react"
import { useParams } from "next/navigation"
import { getDoctorById } from "@/features/doctors/services/doctorService"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"
import { createAppointment, getAppointmentsByDoctor } from "@/features/appointments/services/appointmentService"

export default function DoctorCalendarPage() {
  const { id } = useParams()
  const auth = useAuth()
  const patientId = auth.keycloak?.subject

  const [doctor, setDoctor] = useState<DoctorQueryViewModel | null>(null)
  const [appointments, setAppointments] = useState<BookingQueryResponse[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  const [currentMonth, setCurrentMonth] = useState(new Date())
  const [selectedDate, setSelectedDate] = useState<Date | null>(null)
  const [selectedSlot, setSelectedSlot] = useState<string | null>(null)
  const [duration, setDuration] = useState<15 | 30 | 60>(30)

  const [bookingLoading, setBookingLoading] = useState(false)
  const [bookingError, setBookingError] = useState("")
  const [bookingSuccess, setBookingSuccess] = useState("")

  // Fetch doctor info
  async function fetchDoctor() {
    try {
      const res = await getDoctorById(id as string)
      setDoctor(res.data.data)
    } catch (err: any) {
      setError(err.response?.status === 404 ? "Doctor not found." : "Failed to load doctor.")
    } finally {
      setLoading(false)
    }
  }

  async function fetchAppointments(month: number, year: number) {
    if (!doctor) return
    try {
      const startOfMonth = new Date(year, month, 1)
      const endOfMonth = new Date(year, month + 1, 0, 23, 59, 59)
      const res = await getAppointmentsByDoctor(doctor.userId, {
      startDate: startOfMonth.toISOString().split("T")[0],
      endDate: endOfMonth.toISOString().split("T")[0],
    })

    setAppointments(res.data.data)
    } catch (err) {
      console.error(err)
    }
  }

  useEffect(() => {
    if (id) fetchDoctor()
  }, [id])

  useEffect(() => {
    if (doctor) {
      fetchAppointments(currentMonth.getMonth(), currentMonth.getFullYear())
    }
  }, [doctor, currentMonth])

  const calendarDays = useMemo(() => {
    if (!doctor) return []

    const month = currentMonth.getMonth()
    const year = currentMonth.getFullYear()
    const firstDay = new Date(year, month, 1).getDay()
    const lastDate = new Date(year, month + 1, 0).getDate()
    const today = new Date()
    const todayStart = new Date()
    todayStart.setHours(0, 0, 0, 0)

    const days = []

    for (let i = 0; i < firstDay; i++) days.push(null)

    for (let i = 1; i <= lastDate; i++) {
      const date = new Date(year, month, i)
      const dayAppointments = appointments.filter(a => {
        const aDate = new Date(a.start)
        return aDate.toDateString() === date.toDateString()
      })

      const dayOfWeek = date.getDay()
      const workDay = doctor.workDays.find(d => d.dayOfWeek === dayOfWeek)

      let status: "past" | "fullyBooked" | "partiallyBooked" | "empty" = "empty"

      if (date.valueOf() < todayStart.valueOf()) status = "past"
      else if (!workDay) status = "fullyBooked"
      else {
        let totalSlots = 0
        for (const wt of workDay.workTimes) {
          const start = new Date(`${date.toDateString()} ${wt.start}`)
          const end = new Date(`${date.toDateString()} ${wt.end}`)
          totalSlots += Math.floor((end.getTime() - start.getTime()) / (duration * 60000))
        }

        if (dayAppointments.length === 0) status = "empty"
        else if (dayAppointments.length < totalSlots) status = "partiallyBooked"
        else status = "fullyBooked"
      }

      days.push({ date, status })
    }

    return days
}, [appointments, doctor, currentMonth, duration])

  const slots = useMemo(() => {
    if (!doctor || !selectedDate) return []
    const dayOfWeek = selectedDate.getDay()
    const workDay = doctor.workDays.find(d => d.dayOfWeek === dayOfWeek)
    if (!workDay) return []

    const now = new Date()
    const result: { start: string }[] = []

    for (const wt of workDay.workTimes) {
      let start = new Date(`${selectedDate.toDateString()} ${wt.start}`)
      const end = new Date(`${selectedDate.toDateString()} ${wt.end}`)

      while (start < end) {
        const slotEnd = new Date(start.getTime() + duration * 60000)
        if (slotEnd > end) break

        const isPast = start < now
        const isBlocked = appointments.some(a => {
          const aStart = new Date(a.start)
          const aEnd = new Date(a.end)  
          const duration = (end.getTime() - start.getTime()) / (1000 * 60)
          aEnd.setMinutes(aEnd.getMinutes() + duration)
          return start < aEnd && slotEnd > aStart
        })

        if (!isPast && !isBlocked) result.push({ start: start.toISOString() })
        start = slotEnd
      }
    }

    return result
  }, [appointments, doctor, selectedDate, duration])

  async function handleBooking() {
    if (!selectedSlot || !doctor || !patientId) {
      setBookingError("You must be logged in to book.")
      return
    }

    setBookingLoading(true)
    setBookingError("")
    setBookingSuccess("")

    try {
      await createAppointment({
        doctorUserId: doctor.userId,
        scheduledStartTime: selectedSlot,
        duration,
      })
      setBookingSuccess("Appointment booked successfully 🎉")
      setSelectedSlot(null)
      if (selectedDate) fetchAppointments(selectedDate.getMonth(), selectedDate.getFullYear())
    } catch (err: any) {
      setBookingError(err.response?.status === 409 ? "This time slot is already taken." : "Failed to book appointment.")
    } finally {
      setBookingLoading(false)
    }
  }

  if (loading) return <p className="p-8">Loading doctor...</p>
  if (error) return <p className="p-8 text-red-600">{error}</p>
  if (!doctor) return null

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">

      <Card>
        <CardContent>
          <CardTitle className="text-2xl flex justify-between">
            {doctor.firstName} {doctor.lastName}
            <span className="text-yellow-600 text-sm">
              ⭐ {doctor.averageRating?.toFixed(1) ?? "0.0"} ({doctor.ratingsCount ?? 0})
            </span>
          </CardTitle>
          <CardDescription>{doctor.bio}</CardDescription>
          <p><b>Specialities:</b> {doctor.specialities.join(", ")}</p>
        </CardContent>
      </Card>

      <Card className="p-4">
        <CardTitle>Select Duration</CardTitle>
        <CardContent className="flex gap-2 mt-3">
          {[15, 30, 60].map(d => (
            <Button
              key={d}
              size="sm"
              variant={duration === d ? "default" : "outline"}
              onClick={() => setDuration(d as 15 | 30 | 60)}
            >
              {d} min
            </Button>
          ))}
        </CardContent>
      </Card>

      <div className="flex justify-between items-center mb-2">
        <Button onClick={() => setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1))}>Previous</Button>
        <h2 className="text-lg font-medium">{currentMonth.toLocaleString("default", { month: "long", year: "numeric" })}</h2>
        <Button onClick={() => setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1))}>Next</Button>
      </div>

      <Card className="p-4">
        <CardTitle>Available Dates</CardTitle>
        <CardContent className="grid grid-cols-7 gap-2 mt-3">
          {calendarDays.map((day, idx) => {
            if (!day) return <div key={idx} />
            const isSelected = selectedDate?.toDateString() === day.date.toDateString()

            let className = ""
            switch (day.status) {
              case "past":
                className = "bg-gray-300 text-gray-600 cursor-not-allowed"
                break
              case "empty":
                className = "bg-green-500 text-white"
                break
              case "partiallyBooked":
                className = "bg-yellow-400 text-white"
                break
              case "fullyBooked":
                className = "bg-red-500 text-white"
                break
            }

            return (
              <Button
                key={day.date.toISOString()}
                size="sm"
                variant={isSelected ? "default" : "outline"}
                className={className}
                disabled={day.status === "past"}
                onClick={() => setSelectedDate(day.date)}
              >
                {day.date.getDate()}
              </Button>
            )
          })}
        </CardContent>
      </Card>

      {slots.length > 0 && selectedDate && (
        <Card className="p-4">
          <CardTitle>Available Slots for {selectedDate.toDateString()}</CardTitle>
          <CardContent className="flex flex-wrap gap-2 mt-3">
            {slots.map(slot => {
              const time = new Date(slot.start).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })
              const isSelected = selectedSlot === slot.start
              return (
                <Button
                  key={slot.start}
                  size="sm"
                  variant={isSelected ? "default" : "outline"}
                  onClick={() => setSelectedSlot(slot.start)}
                >
                  {time}
                </Button>
              )
            })}
          </CardContent>

          {selectedSlot && (
            <div className="mt-3 flex justify-between items-center">
              <p>Selected Time: {new Date(selectedSlot).toLocaleString()}</p>
              <Button onClick={handleBooking} disabled={bookingLoading}>
                {bookingLoading ? "Booking..." : "Confirm Booking"}
              </Button>
            </div>
          )}

          {(bookingError || bookingSuccess) && (
            <p className={`mt-2 text-sm ${bookingError ? "text-red-600" : "text-green-600"}`}>
              {bookingError || bookingSuccess}
            </p>
          )}
        </Card>
      )}
    </div>
  )
}