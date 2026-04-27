"use client"

import { useState, useEffect } from "react"
import { useSearchParams } from "next/navigation"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"
import {
  createAppointment,
  getAppointmentsByDoctor,
  rescheduleAppointment
} from "@/features/appointments/services/appointmentService"

import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { useDoctorCalendar } from "@/features/appointments/hooks/useDoctorCalendar"
import { CalendarGrid } from "@/components/calendar/CalendarGrid"
import { TimeSlots } from "@/components/calendar/TimeSlots"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { getRatingsByDoctor } from "@/features/ratings/services/ratingService"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"
import { DoctorRatings } from "@/components/ratings/DoctorRatings"

type Props = {
  doctor: DoctorQueryViewModel
  initialAppointments: BookingQueryResponse[]
  initialRatings: RatingQueryViewModel[]
  initialRatingsTotalPages: number
  rescheduleId: string | null
}

export default function DoctorCalendarClient({
  doctor,
  initialAppointments,
  initialRatings,
  initialRatingsTotalPages,
  rescheduleId,
}: Props) {
  const auth = useAuthGuard()
  const patientId = auth?.keycloak?.subject

  const [appointments, setAppointments] = useState(initialAppointments)
  const [ratings, setRatings] = useState(initialRatings)
  const [ratingsPage, setRatingsPage] = useState(1)
  const [ratingsTotalPages, setRatingsTotalPages] = useState(initialRatingsTotalPages)

  const [currentMonth, setCurrentMonth] = useState(new Date())
  const [selectedDate, setSelectedDate] = useState<Date | null>(null)
  const [selectedSlot, setSelectedSlot] = useState<string | null>(null)
  const [duration, setDuration] = useState<15 | 30 | 60>(30)

  const [bookingLoading, setBookingLoading] = useState(false)
  const [bookingError, setBookingError] = useState("")
  const [bookingSuccess, setBookingSuccess] = useState("")

  async function fetchAppointments(month: number, year: number) {
    try {
      const startOfMonth = new Date(year, month, 1)
      const endOfMonth = new Date(year, month + 1, 0, 23, 59, 59)

      const res = await getAppointmentsByDoctor(doctor.userId, {
        startDate: startOfMonth.toISOString().split("T")[0],
        endDate: endOfMonth.toISOString().split("T")[0],
      })

      setAppointments(res.data.data)
    } catch {}
  }

  async function fetchRatings(page = 1) {
    if (rescheduleId) return
    try {
      const res = await getRatingsByDoctor(doctor.userId, {
        PatientId: "",
        AppointmentId: "",
        MinScore: null,
        MaxScore: null,
        SortOrder: "DESC",
        SortPropertyName: "CreatedAt",
        Page: page,
        PageSize: 4,
      })

      const fetchedRatings = res?.data?.data?.items ?? []
      setRatings(fetchedRatings)
      setRatingsPage(page)
      setRatingsTotalPages(
        Math.ceil((res?.data?.data?.totalCount ?? fetchedRatings.length) / 4)
      )
    } catch {}
  }

  useEffect(() => {
    fetchAppointments(currentMonth.getMonth(), currentMonth.getFullYear())
    fetchRatings()
  }, [currentMonth])

  const { calendarDays, timeSlots } = useDoctorCalendar({
    doctor,
    appointments,
    currentMonth,
    duration,
    selectedDate,
  })

  async function handleBooking() {
    if (!selectedSlot || !patientId) {
      setBookingError("You must select a time slot.")
      return
    }

    setBookingLoading(true)
    setBookingError("")
    setBookingSuccess("")

    try {
      if (rescheduleId) {
        await rescheduleAppointment(rescheduleId, {
          scheduledStartTime: selectedSlot,
          duration,
        })
        setBookingSuccess("Appointment rescheduled successfully")
      } else {
        await createAppointment({
          doctorUserId: doctor.userId,
          scheduledStartTime: selectedSlot,
          duration,
        })
        setBookingSuccess("Appointment booked successfully")
      }

      setSelectedSlot(null)
    } catch (err: any) {
      setBookingError(
        err.response?.status === 409
          ? "This time slot is already taken."
          : "Failed to book appointment."
      )
    } finally {
      setBookingLoading(false)
    }
  }

  const weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

  return (
    <div className="p-8 max-w-5xl mx-auto space-y-6">

      {!rescheduleId && (
        <>
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

          <DoctorRatings
            ratings={ratings}
            page={ratingsPage}
            totalPages={ratingsTotalPages}
            onPageChange={fetchRatings}
          />
        </>
      )}

      <Card className="p-4">
        <CardContent>
          <h2 className="text-xl font-semibold mb-4">
            {rescheduleId ? "Reschedule Appointment" : "Book an Appointment"}
          </h2>

          <div className="flex justify-between items-center mb-2">
            <Button onClick={() =>
              setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() - 1, 1))
            }>
              Previous
            </Button>

            <h3 className="text-lg font-medium">
              {currentMonth.toLocaleString("default", { month: "long", year: "numeric" })}
            </h3>

            <Button onClick={() =>
              setCurrentMonth(new Date(currentMonth.getFullYear(), currentMonth.getMonth() + 1, 1))
            }>
              Next
            </Button>
          </div>

          <div className="grid grid-cols-7 text-center font-medium mb-2">
            {weekdays.map(d => <div key={d}>{d}</div>)}
          </div>

          <CalendarGrid
            days={calendarDays}
            selectedDate={selectedDate}
            onSelect={setSelectedDate}
          />
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

      {selectedDate && (
        <Card className="p-4">
          <CardContent>
            <h3 className="font-medium mb-2">
              Available Slots for {selectedDate.toDateString()}
            </h3>
            <TimeSlots
              slots={timeSlots}
              selectedSlot={selectedSlot}
              onSelect={setSelectedSlot}
            />
          </CardContent>
        </Card>
      )}

      {selectedSlot && (
        <Card className="p-3">
          <CardContent>
            <div className="mb-2 flex justify-between items-center">
              <p>Selected Time: {new Date(selectedSlot).toLocaleString()}</p>
              <Button onClick={handleBooking} disabled={bookingLoading}>
                {bookingLoading ? "Booking..." : rescheduleId ? "Confirm Reschedule" : "Confirm Booking"}
              </Button>
            </div>
          </CardContent>
        </Card>
      )}

      {(bookingError || bookingSuccess) && (
        <p className={`mt-2 text-sm ${bookingError ? "text-red-600" : "text-green-600"}`}>
          {bookingError || bookingSuccess}
        </p>
      )}
    </div>
  )
}