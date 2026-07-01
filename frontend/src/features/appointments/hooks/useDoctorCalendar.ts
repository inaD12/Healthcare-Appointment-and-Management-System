import { useMemo } from "react"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"

export type StatusType =
  | "past"
  | "empty"
  | "fullyBooked"
  | "partiallyBooked"

export type CalendarDay = {
  date: Date
  isCurrentMonth: boolean
  status: StatusType
}

export type TimeSlotItem = {
  start: string
  isBlocked: boolean
  isPast: boolean
}

type UseDoctorCalendarProps = {
  doctor: DoctorQueryViewModel | null
  appointments: BookingQueryResponse[]
  currentMonth: Date
  duration: number
  selectedDate: Date | null
}

function overlaps(
  start1: Date,
  end1: Date,
  start2: Date,
  end2: Date
) {
  return start1 < end2 && end1 > start2
}

export function useDoctorCalendar({
  doctor,
  appointments,
  currentMonth,
  duration,
  selectedDate,
}: UseDoctorCalendarProps) {
  /**
   * Group appointments by day
   */
  const appointmentsByDate = useMemo(() => {
    const map = new Map<string, BookingQueryResponse[]>()

    for (const appointment of appointments) {
      const key = new Date(appointment.start).toDateString()

      if (!map.has(key)) {
        map.set(key, [])
      }

      map.get(key)!.push(appointment)
    }

    return map
  }, [appointments])

  /**
   * Group availability exceptions by day
   */
  const exceptionsByDate = useMemo(() => {
    const map = new Map<
      string,
      DoctorQueryViewModel["availabilityExceptions"]
    >()

    if (!doctor) return map

    for (const exception of doctor.availabilityExceptions) {
      const start = new Date(exception.start)
      const end = new Date(exception.end)

      const current = new Date(start)
      current.setHours(0, 0, 0, 0)

      const last = new Date(end)
      last.setHours(0, 0, 0, 0)

      while (current <= last) {
        const key = current.toDateString()

        if (!map.has(key)) {
          map.set(key, [])
        }

        map.get(key)!.push(exception)

        current.setDate(current.getDate() + 1)
      }
    }

    return map
  }, [doctor])

  const todayStart = useMemo(() => {
    const d = new Date()
    d.setHours(0, 0, 0, 0)
    return d
  }, [])

  /**
   * Calendar Days
   */
  const calendarDays: CalendarDay[] = useMemo(() => {
    if (!doctor) return []

    const month = currentMonth.getMonth()
    const year = currentMonth.getFullYear()

    const firstDay = new Date(year, month, 1).getDay()
    const lastDate = new Date(year, month + 1, 0).getDate()
    const prevMonthLastDate = new Date(year, month, 0).getDate()

    const days: CalendarDay[] = []

    for (let i = firstDay - 1; i >= 0; i--) {
      days.push({
        date: new Date(year, month - 1, prevMonthLastDate - i),
        isCurrentMonth: false,
        status: "past",
      })
    }

    for (let day = 1; day <= lastDate; day++) {
      const date = new Date(year, month, day)

      const workDay = doctor.workDays.find(
        (d) => d.dayOfWeek === date.getDay()
      )

      if (date < todayStart) {
        days.push({
          date,
          isCurrentMonth: true,
          status: "past",
        })
        continue
      }

      if (!workDay) {
        days.push({
          date,
          isCurrentMonth: true,
          status: "fullyBooked",
        })
        continue
      }

      const dayAppointments =
        appointmentsByDate.get(date.toDateString()) ?? []

      const dayExceptions =
        exceptionsByDate.get(date.toDateString()) ?? []

      let totalSlots = 0
      let blockedSlots = 0

      for (const workTime of workDay.workTimes) {
        const [sh, sm] = workTime.start.split(":").map(Number)
        const [eh, em] = workTime.end.split(":").map(Number)

        let slotStart = new Date(date)
        slotStart.setHours(sh, sm, 0, 0)

        const workEnd = new Date(date)
        workEnd.setHours(eh, em, 0, 0)

        while (slotStart < workEnd) {
          const slotEnd = new Date(
            slotStart.getTime() + duration * 60000
          )

          if (slotEnd > workEnd) {
            break
          }

          totalSlots++

          const blockedByAppointment = dayAppointments.some((appointment) =>
            overlaps(
              slotStart,
              slotEnd,
              new Date(appointment.start),
              new Date(appointment.end)
            )
          )

          const blockedByException = dayExceptions.some((exception) =>
            overlaps(
              slotStart,
              slotEnd,
              new Date(exception.start),
              new Date(exception.end)
            )
          )

          if (blockedByAppointment || blockedByException) {
            blockedSlots++
          }

          slotStart = slotEnd
        }
      }

      let status: StatusType

      if (blockedSlots === 0) {
        status = "empty"
      } else if (blockedSlots < totalSlots) {
        status = "partiallyBooked"
      } else {
        status = "fullyBooked"
      }

      days.push({
        date,
        isCurrentMonth: true,
        status,
      })
    }

    while (days.length < 42) {
      const last = days[days.length - 1].date

      const next = new Date(last)
      next.setDate(last.getDate() + 1)

      days.push({
        date: next,
        isCurrentMonth: false,
        status: "past",
      })
    }

    return days
  }, [
    doctor,
    appointmentsByDate,
    exceptionsByDate,
    currentMonth,
    duration,
    todayStart,
  ])

  /**
   * Time Slots
   */
  const timeSlots: TimeSlotItem[] = useMemo(() => {
    if (!doctor || !selectedDate) {
      return []
    }

    const workDay = doctor.workDays.find(
      (d) => d.dayOfWeek === selectedDate.getDay()
    )

    if (!workDay) {
      return []
    }

    const now = new Date()

    const dayAppointments =
      appointmentsByDate.get(selectedDate.toDateString()) ?? []

    const dayExceptions =
      exceptionsByDate.get(selectedDate.toDateString()) ?? []

    const slots: TimeSlotItem[] = []

    for (const workTime of workDay.workTimes) {
      const [sh, sm] = workTime.start.split(":").map(Number)
      const [eh, em] = workTime.end.split(":").map(Number)

      let slotStart = new Date(selectedDate)
      slotStart.setHours(sh, sm, 0, 0)

      const workEnd = new Date(selectedDate)
      workEnd.setHours(eh, em, 0, 0)

      while (slotStart < workEnd) {
        const slotEnd = new Date(
          slotStart.getTime() + duration * 60000
        )

        if (slotEnd > workEnd) {
          break
        }

        const isPast = slotStart < now

        const blockedByAppointment = dayAppointments.some((appointment) =>
          overlaps(
            slotStart,
            slotEnd,
            new Date(appointment.start),
            new Date(appointment.end)
          )
        )

        const blockedByException = dayExceptions.some((exception) =>
          overlaps(
            slotStart,
            slotEnd,
            new Date(exception.start),
            new Date(exception.end)
          )
        )

        slots.push({
          start: slotStart.toISOString(),
          isPast,
          isBlocked:
            blockedByAppointment || blockedByException,
        })

        slotStart = slotEnd
      }
    }

    return slots
  }, [
    doctor,
    selectedDate,
    appointmentsByDate,
    exceptionsByDate,
    duration,
  ])

  return {
    calendarDays,
    appointmentsByDate,
    timeSlots,
  }
}