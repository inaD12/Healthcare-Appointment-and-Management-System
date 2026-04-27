"use client"

import { useState } from "react"

import {
  addWorkDaySchedule,
  removeWorkDaySchedule
} from "@/features/doctors/services/doctorService"

import {
  DoctorQueryViewModel,
  WorkDayDto
} from "@/features/doctors/types/doctors"

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

type Props = {
  doctor: DoctorQueryViewModel
  setDoctor: (d: DoctorQueryViewModel) => void
}

export default function DoctorWorkDaysCard({
  doctor,
  setDoctor
}: Props) {

  const [newWorkDay, setNewWorkDay] = useState<WorkDayDto>({
    dayOfWeek: 0,
    workTimes: [{ start: "", end: "" }]
  })

  const add = async () => {

    await addWorkDaySchedule(newWorkDay)

    setDoctor({
      ...doctor,
      workDays: [...doctor.workDays, newWorkDay]
    })
  }

  const remove = async (dayOfWeek: number) => {

    await removeWorkDaySchedule({ dayOfWeek })

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

                <TableCell>
                  {["Sun","Mon","Tue","Wed","Thu","Fri","Sat"][day.dayOfWeek]}
                </TableCell>

                <TableCell>
                  {day.workTimes.map(
                    w => `${w.start}-${w.end}`
                  ).join(", ")}
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

          </TableBody>

        </Table>

      </CardContent>

    </Card>
  )
}