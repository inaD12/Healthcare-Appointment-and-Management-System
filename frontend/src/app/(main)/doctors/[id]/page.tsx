"use client"

import { useState, useEffect } from "react"
import { useParams } from "next/navigation"
import { getDoctorById } from "@/features/doctors/services/doctorService"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"
import { createAppointment, getAppointmentsByDoctor } from "@/features/appointments/services/appointmentService"
import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { useDoctorCalendar } from "@/features/appointments/hooks/useDoctorCalendar"
import { CalendarGrid } from "@/components/calendar/CalendarGrid"
import { TimeSlots } from "@/components/calendar/TimeSlots"


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
  } catch (err: any) {
    if (err.response?.status !== 404) {
      console.error(err)
    }
  }
}

  useEffect(() => { if (id) fetchDoctor() }, [id])
  useEffect(() => { if (doctor) fetchAppointments(currentMonth.getMonth(), currentMonth.getFullYear()) }, [doctor, currentMonth])

  const { calendarDays, timeSlots } = useDoctorCalendar({
    doctor,
    appointments,
    currentMonth,
    duration,
    selectedDate
  })

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

      setBookingSuccess("Appointment booked successfully")
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

  const weekdays = ["Sun","Mon","Tue","Wed","Thu","Fri","Sat"]

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

      <div className="grid grid-cols-7 text-center font-medium">
        {weekdays.map(d => <div key={d}>{d}</div>)}
      </div>

      <CalendarGrid days={calendarDays} selectedDate={selectedDate} onSelect={setSelectedDate}></CalendarGrid>

      {selectedDate && (
        <div className="mt-4">
          <h3 className="font-medium mb-2">Available Slots for {selectedDate.toDateString()}</h3>
          <TimeSlots slots={timeSlots} selectedSlot={selectedSlot} onSelect={setSelectedSlot}></TimeSlots>
        </div>
      )}

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

    </div>
  )
}