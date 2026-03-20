"use client"

import { useEffect, useMemo, useState } from "react"
import { useParams } from "next/navigation"
import { getDoctorById } from "@/features/doctors/services/doctorService"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { createAppointment } from "@/features/appointments/services/appointmentService"
import { useAuth } from "@/features/auth/hooks/useAuth"

export default function DoctorDetailsPage() {
  const { id } = useParams()
  const auth = useAuth()
  const patientId = auth.keycloak?.subject

  const [doctor, setDoctor] = useState<DoctorQueryViewModel | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  const [selectedDate, setSelectedDate] = useState(new Date())
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
      if (err.response?.status === 404) {
        setError("Doctor not found.")
      } else if (err.response?.status === 403) {
        setError("You don’t have permission to view this doctor.")
      } else {
        setError("Failed to load doctor.")
      }
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (id) fetchDoctor()
  }, [id])

  useEffect(() => {
    setSelectedSlot(null)
  }, [selectedDate, duration])

  const slots = useMemo(() => {
    if (!doctor) return []

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

        const isBlocked = doctor.availabilityExceptions.some(ex => {
          const exStart = new Date(ex.start)
          const exEnd = new Date(ex.end)
          return start < exEnd && slotEnd > exStart
        })

        if (!isPast && !isBlocked) {
          result.push({ start: start.toISOString() })
        }

        start = slotEnd
      }
    }

    return result
  }, [doctor, selectedDate, duration])

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
    } catch (err: any) {
      if (err.response?.status === 409) {
        setBookingError("This time slot is already taken.")
      } else {
        setBookingError("Failed to book appointment.")
      }
    } finally {
      setBookingLoading(false)
    }
  }

  if (loading) return <p className="p-8">Loading doctor...</p>

  if (error)
    return (
      <div className="p-8">
        <Card className="border-red-400 bg-red-50">
          <CardContent className="text-red-700">{error}</CardContent>
        </Card>
      </div>
    )

  if (!doctor) return null

  return (
    <div className="p-8 max-w-4xl mx-auto space-y-6">

      <Card>
        <CardContent className="space-y-3">
          <CardTitle className="text-2xl flex justify-between items-center">
            {doctor.firstName} {doctor.lastName}

            <span className="text-yellow-600 text-sm">
              ⭐ {doctor.averageRating?.toFixed(1) ?? "0.0"} ({doctor.ratingsCount ?? 0})
            </span>
          </CardTitle>

          <CardDescription>{doctor.bio}</CardDescription>

          <p>
            <b>Specialities:</b> {doctor.specialities.join(", ")}
          </p>

          <p className="text-gray-500 text-sm">
            Timezone: {doctor.timeZoneId}
          </p>
        </CardContent>
      </Card>

      <Card className="p-4">
        <CardTitle>Select Duration</CardTitle>
        <CardContent className="flex gap-2 mt-3">
          {[15, 30, 60].map((d) => (
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

      <Card className="p-4">
        <CardTitle>Select Date</CardTitle>
        <CardContent className="mt-3">
          <Input
            type="date"
            value={selectedDate.toISOString().split("T")[0]}
            onChange={(e) => setSelectedDate(new Date(e.target.value))}
            className="w-60"
          />
        </CardContent>
      </Card>

      <Card className="p-4">
        <CardTitle>
          Available on {selectedDate.toDateString()}
        </CardTitle>

        <CardContent className="flex flex-wrap gap-2 mt-3">
          {slots.length === 0 ? (
            <p className="text-sm text-gray-500">
              No available slots for this day
            </p>
          ) : (
            slots.map((slot) => {
              const time = new Date(slot.start).toLocaleTimeString([], {
                hour: "2-digit",
                minute: "2-digit",
              })

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
            })
          )}
        </CardContent>
      </Card>

      {selectedSlot && (
        <Card className="p-4">
          <CardContent className="flex items-center justify-between">
            <div>
              <p className="font-medium">Selected Time</p>
              <p className="text-sm text-gray-500">
                {new Date(selectedSlot).toLocaleString()}
              </p>
            </div>

            <Button onClick={handleBooking} disabled={bookingLoading}>
              {bookingLoading ? "Booking..." : "Confirm Booking"}
            </Button>
          </CardContent>

          {(bookingError || bookingSuccess) && (
            <p
              className={`mt-2 text-sm ${
                bookingError ? "text-red-600" : "text-green-600"
              }`}
            >
              {bookingError || bookingSuccess}
            </p>
          )}
        </Card>
      )}
    </div>
  )
}