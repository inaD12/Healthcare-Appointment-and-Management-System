"use client"

import { useEffect, useMemo, useState } from "react"

import {
  DoctorQueryViewModel,
  WorkDayDto
} from "@/features/doctors/types/doctorsTypes"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent
} from "@/components/ui/card"

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from "@/components/ui/table"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { doctorService } from "../../services/doctorService"

const days = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"]

const formatTime = (time?: string) => {
  if (!time) return ""
  return time.length >= 5 ? time.slice(0, 5) : time
}

type Props = {
  doctor: DoctorQueryViewModel
  setDoctor: (d: DoctorQueryViewModel) => void
}

export default function DoctorWorkDaysCard({
  doctor,
  setDoctor
}: Props) {
  const availableDays = useMemo(
    () =>
      days
        .map((label, index) => ({ label, index }))
        .filter(
          d => !doctor.workDays.some(w => w.dayOfWeek === d.index)
        ),
    [doctor.workDays]
  )

  const [newWorkDay, setNewWorkDay] = useState<WorkDayDto>({
    dayOfWeek: 0,
    workTimes: [{ start: "", end: "" }]
  })

  useEffect(() => {
    if (
      availableDays.length > 0 &&
      !availableDays.some(d => d.index === newWorkDay.dayOfWeek)
    ) {
      setNewWorkDay(prev => ({
        ...prev,
        dayOfWeek: availableDays[0].index
      }))
    }
  }, [availableDays])

  const canAdd =
    availableDays.length > 0 &&
    newWorkDay.workTimes[0].start !== "" &&
    newWorkDay.workTimes[0].end !== "" &&
    newWorkDay.workTimes[0].start < newWorkDay.workTimes[0].end

  const add = async () => {
    const { dayOfWeek, workTimes } = newWorkDay
    const { start, end } = workTimes[0]

    if (!start || !end) {
      alert("Please select both a start and end time.")
      return
    }

    if (start >= end) {
      alert("Start time must be earlier than end time.")
      return
    }

    if (doctor.workDays.some(w => w.dayOfWeek === dayOfWeek)) {
      alert("This day already has a work schedule.")
      return
    }

    await doctorService.addWorkDaySchedule(newWorkDay)

    // normalize BEFORE adding to UI (fixes inconsistency)
    const normalized: WorkDayDto = {
      ...newWorkDay,
      workTimes: newWorkDay.workTimes.map(t => ({
        start: formatTime(t.start),
        end: formatTime(t.end)
      }))
    }

    setDoctor({
      ...doctor,
      workDays: [...doctor.workDays, normalized]
    })

    if (availableDays.length > 1) {
      const nextDay = availableDays.find(
        d => d.index !== dayOfWeek
      )!.index

      setNewWorkDay({
        dayOfWeek: nextDay,
        workTimes: [{ start: "", end: "" }]
      })
    } else {
      setNewWorkDay({
        dayOfWeek: 0,
        workTimes: [{ start: "", end: "" }]
      })
    }
  }

  const remove = async (dayOfWeek: number) => {
    await doctorService.removeWorkDaySchedule({ dayOfWeek })

    setDoctor({
      ...doctor,
      workDays: doctor.workDays.filter(
        w => w.dayOfWeek !== dayOfWeek
      )
    })
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Work Days & Hours</CardTitle>
      </CardHeader>

      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Day</TableHead>
              <TableHead>Times</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>

          <TableBody>
            {doctor.workDays.map(day => (
              <TableRow key={day.dayOfWeek}>
                <TableCell>{days[day.dayOfWeek]}</TableCell>

                <TableCell>
                  {day.workTimes
                    .map(
                      t =>
                        `${formatTime(t.start)} - ${formatTime(t.end)}`
                    )
                    .join(", ")}
                </TableCell>

                <TableCell>
                  <Button
                    size="sm"
                    variant="destructive"
                    onClick={() => remove(day.dayOfWeek)}
                  >
                    Remove
                  </Button>
                </TableCell>
              </TableRow>
            ))}

            {doctor.workDays.length === 0 && (
              <TableRow>
                <TableCell
                  colSpan={3}
                  className="text-center text-muted-foreground"
                >
                  No work days configured.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>

        {availableDays.length > 0 ? (
          <div className="mt-6 flex flex-wrap items-end gap-4">
            <div>
              <label className="mb-1 block text-sm font-medium">
                Day
              </label>

              <select
                className="h-10 rounded-md border px-3"
                value={newWorkDay.dayOfWeek}
                onChange={e =>
                  setNewWorkDay({
                    ...newWorkDay,
                    dayOfWeek: Number(e.target.value)
                  })
                }
              >
                {availableDays.map(day => (
                  <option key={day.index} value={day.index}>
                    {day.label}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium">
                Start
              </label>

              <Input
                type="time"
                value={newWorkDay.workTimes[0].start}
                onChange={e =>
                  setNewWorkDay({
                    ...newWorkDay,
                    workTimes: [
                      {
                        ...newWorkDay.workTimes[0],
                        start: e.target.value
                      }
                    ]
                  })
                }
              />
            </div>

            <div>
              <label className="mb-1 block text-sm font-medium">
                End
              </label>

              <Input
                type="time"
                value={newWorkDay.workTimes[0].end}
                onChange={e =>
                  setNewWorkDay({
                    ...newWorkDay,
                    workTimes: [
                      {
                        ...newWorkDay.workTimes[0],
                        end: e.target.value
                      }
                    ]
                  })
                }
              />
            </div>

            <Button onClick={add} disabled={!canAdd}>
              Add Work Day
            </Button>
          </div>
        ) : (
          <p className="mt-6 text-sm text-muted-foreground">
            All seven days already have a work schedule.
          </p>
        )}
      </CardContent>
    </Card>
  )
}