import { useMemo} from "react"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { BookingQueryResponse } from "@/features/appointments/types/appointmentsTypes"

export type StatusType = "past" | "empty" | "fullyBooked" | "partiallyBooked"

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

export function useDoctorCalendar({
  doctor,
  appointments,
  currentMonth,
  duration,
  selectedDate,
}: UseDoctorCalendarProps) {
  const appointmentsByDate = useMemo(() => {
    const map = new Map<string, BookingQueryResponse[]>()
    for (const a of appointments) {
      const key = new Date(a.start).toDateString()
      if (!map.has(key)) map.set(key, [])
      map.get(key)!.push(a)
    }
    return map
  }, [appointments])

  const todayStart = useMemo(() => {
    const d = new Date()
    d.setHours(0, 0, 0, 0)
    return d
  }, [])

  const calendarDays: CalendarDay[] = useMemo(() => {
    if (!doctor) return []

    const month = currentMonth.getMonth()
    const year = currentMonth.getFullYear()
    const firstDay = new Date(year, month, 1).getDay()
    const lastDate = new Date(year, month + 1, 0).getDate()
    const prevMonthLastDate = new Date(year, month, 0).getDate()

    const days: CalendarDay[] = []

    for (let i = firstDay - 1; i >= 0; i--) {
      const date = new Date(year, month - 1, prevMonthLastDate - i)
      days.push({ date, isCurrentMonth: false, status: "past" })
    }

    for (let i = 1; i <= lastDate; i++) {
      const date = new Date(year, month, i)
      const dayAppointments = appointmentsByDate.get(date.toDateString()) || []
      const workDay = doctor.workDays.find(d => d.dayOfWeek === date.getDay())

      let status: StatusType = "empty"
      if (date < todayStart) status = "past"
      else if (!workDay) status = "fullyBooked"
      else {
        let totalSlots = 0
        for (const wt of workDay.workTimes) {
          const [sh, sm] = wt.start.split(":").map(Number)
          const [eh, em] = wt.end.split(":").map(Number)
          let start = new Date(date)
          start.setHours(sh, sm, 0, 0)
          const end = new Date(date)
          end.setHours(eh, em, 0, 0)
          while (start < end) {
            const slotEnd = new Date(start.getTime() + duration * 60000)
            if (slotEnd > end) break
            totalSlots++
            start = slotEnd
          }
        }
        if (dayAppointments.length === 0) status = "empty"
        else if (dayAppointments.length < totalSlots) status = "partiallyBooked"
        else status = "fullyBooked"
      }

      days.push({ date, isCurrentMonth: true, status })
    }

    while (days.length < 42) {
      const last = days[days.length - 1].date
      const next = new Date(last)
      next.setDate(last.getDate() + 1)
      days.push({ date: next, isCurrentMonth: false, status: "past" })
    }

    return days
  }, [doctor, appointmentsByDate, currentMonth, duration, todayStart])

  const timeSlots: TimeSlotItem[] = useMemo(() => {
    if (!doctor || !selectedDate) return []

    const dayOfWeek = selectedDate.getDay()
    const workDay = doctor.workDays.find(d => d.dayOfWeek === dayOfWeek)
    if (!workDay) return []

    const now = new Date()
    const dayAppointments = appointmentsByDate.get(selectedDate.toDateString()) || []
    const slots: TimeSlotItem[] = []

    for (const wt of workDay.workTimes) {
      const [sh, sm] = wt.start.split(":").map(Number)
      const [eh, em] = wt.end.split(":").map(Number)
      let start = new Date(selectedDate)
      start.setHours(sh, sm, 0, 0)
      const end = new Date(selectedDate)
      end.setHours(eh, em, 0, 0)

      while (start < end) {
        const slotEnd = new Date(start.getTime() + duration * 60000)
        if (slotEnd > end) break
        const isPast = start < now
        const isBlocked = dayAppointments.some(a => {
          const aStart = new Date(a.start)
          const aEnd = new Date(a.end)
          return start < aEnd && slotEnd > aStart
        })
        slots.push({ start: start.toISOString(), isBlocked, isPast })
        start = slotEnd
      }
    }

    return slots
  }, [appointmentsByDate, doctor, duration, selectedDate])

  return { calendarDays, appointmentsByDate, timeSlots }
}