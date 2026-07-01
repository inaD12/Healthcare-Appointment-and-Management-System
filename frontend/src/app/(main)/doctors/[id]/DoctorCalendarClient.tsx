"use client"

import { useState, useEffect } from "react"

import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"

import { useDoctorCalendar } from "@/features/appointments/hooks/useDoctorCalendar"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

import BookingConfirmBar from "@/features/doctors/components/BookingConfirmBar"
import CalendarSection from "@/features/doctors/components/CalendarSection"
import DoctorHeader from "@/features/doctors/components/DoctorHeader"
import DurationSelector from "@/features/doctors/components/DurationSelector"
import TimeSlotSection from "@/features/doctors/components/TimeSlotSection"

import { appointmentService } from "@/features/appointments/services/appointmentService"
import { ratingService } from "@/features/ratings/services/ratingService"
import { useRouter } from "next/navigation"

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
  const router = useRouter()

  const patientId = auth?.keycloak?.subject
  const isAdmin = auth?.isAdmin

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

      const res = await appointmentService.getAppointmentsByDoctor(
        doctor.userId,
        {
          startDate: startOfMonth.toISOString().split("T")[0],
          endDate: endOfMonth.toISOString().split("T")[0],
        }
      )

      setAppointments(res.data.data)
    } catch {}
  }

  async function fetchRatings(page = 1) {
    if (rescheduleId) return

    try {
      const res = await ratingService.getRatingsByDoctor(doctor.userId, {
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

  useEffect(() => {
    if (!bookingSuccess) return

    const t = setTimeout(() => {
      setBookingSuccess("")
    }, 3000)

    return () => clearTimeout(t)
  }, [bookingSuccess])

const adjustedAppointments = appointments.map((a) => {
  const start = new Date(a.start)
  const end = new Date(a.end)

  start.setHours(start.getHours() - 3)
  end.setHours(end.getHours() - 3)

  return {
    ...a,
    start: start.toISOString(),
    end: end.toISOString(),
  }
})

const { calendarDays, timeSlots } = useDoctorCalendar({
  doctor,
  appointments: adjustedAppointments,
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
    // add +3 hours
    const adjustedStartTime = new Date(selectedSlot)
    adjustedStartTime.setHours(adjustedStartTime.getHours() + 3)

    const payload = {
      scheduledStartTime: adjustedStartTime.toISOString(),
      duration,
    }

    if (rescheduleId) {
      if (isAdmin) {
        await appointmentService.rescheduleAppointmentByAdmin(rescheduleId, payload)
      } else {
        await appointmentService.rescheduleAppointment(rescheduleId, payload)
      }

      setBookingSuccess("Appointment rescheduled successfully")

      setTimeout(() => {
        router.back()
      }, 600)
    } else {
      await appointmentService.createAppointment({
        doctorUserId: doctor.userId,
        scheduledStartTime: adjustedStartTime.toISOString(),
        duration,
      })

      setBookingSuccess("Appointment booked successfully")
    }

    setSelectedSlot(null)
  } catch (err: any) {
    setBookingError(
      err.response?.status === 409
        ? "This time slot is already taken."
        : "Failed to process appointment."
    )
  } finally {
    setBookingLoading(false)
  }
}

  const weekdays = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

  return (
    <div className="space-y-4">
      {bookingSuccess && (
        <div className="mb-4 rounded-md bg-green-100 text-green-800 px-4 py-2">
          {bookingSuccess}
        </div>
      )}

      {bookingError && (
        <div className="mb-4 rounded-md bg-red-100 text-red-800 px-4 py-2">
          {bookingError}
        </div>
      )}

      <DoctorHeader
        doctor={doctor}
        ratings={rescheduleId ? undefined : ratings}
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
    </div>
  )
}