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
import BookingConfirmBar from "@/features/doctors/components/BookingConfirmBar"
import CalendarSection from "@/features/doctors/components/CalendarSection"
import DoctorHeader from "@/features/doctors/components/DoctorHeader"
import DurationSelector from "@/features/doctors/components/DurationSelector"
import TimeSlotSection from "@/features/doctors/components/TimeSlotSection"

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
    <>
    <DoctorHeader
        doctor={doctor}
        ratings={ratings}
        ratingsPage={ratingsPage}
        ratingsTotalPages={ratingsTotalPages}
        onPageChange={fetchRatings}
        />

    <CalendarSection
        currentMonth={currentMonth}
        setCurrentMonth={setCurrentMonth}
        weekdays={weekdays}
        calendarDays={calendarDays}
        selectedDate={selectedDate}
        setSelectedDate={setSelectedDate}
        rescheduleId={rescheduleId}
        />

    <DurationSelector duration={duration} setDuration={setDuration} />

    {selectedDate && (
    <TimeSlotSection
        selectedDate={selectedDate}
        timeSlots={timeSlots}
        selectedSlot={selectedSlot}
        setSelectedSlot={setSelectedSlot}
    />
    )}

    {selectedSlot && (
    <BookingConfirmBar
        selectedSlot={selectedSlot}
        bookingLoading={bookingLoading}
        onConfirm={handleBooking}
        rescheduleId={rescheduleId}
    />
    )}
</>
  )
}