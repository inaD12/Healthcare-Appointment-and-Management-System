"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"

import {
  updateDoctorInfo,
  addWorkDaySchedule,
  removeWorkDaySchedule,
  addExtraAvailability,
  deleteExtraAvailability,
  addUnavailability,
  deleteUnavailability
} from "@/features/doctors/services/doctorService"

import {
  DoctorQueryViewModel,
  WorkDayDto,
  DoctorAvailabilityExceptionDto
} from "@/features/doctors/types/doctors"

import { AppointmentResponse } from "@/features/appointments/types/appointmentsTypes"

import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "@/components/ui/card"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Badge } from "@/components/ui/badge"

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow
} from "@/components/ui/table"

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from "@/components/ui/select"

import DoctorSchedule from "@/components/schedule/DoctorSchedule"
import { PersonProfileCard } from "@/components/profile/PersonProfileCard"
import { getMyAppointments } from "@/features/appointments/services/appointmentService"

type Props = {
  doctor: DoctorQueryViewModel
  appointments: AppointmentResponse[]
}

export default function DoctorProfileClient({
  doctor: initialDoctor,
  appointments
}: Props) {

  useAuthGuard()

  const router = useRouter()

  const [doctor, setDoctor] = useState(initialDoctor)

  const [editingBio, setEditingBio] = useState(false)
  const [bioInput, setBioInput] = useState(initialDoctor.bio)

  const [newWorkDay, setNewWorkDay] = useState<WorkDayDto>({
    dayOfWeek: 0,
    workTimes: [{ start: "", end: "" }]
  })

  const [newAvailability, setNewAvailability] =
    useState<DoctorAvailabilityExceptionDto>({
      start: "",
      end: "",
      reason: "",
      type: 1
    })

  const handleSaveBio = async () => {
    try {

      await updateDoctorInfo({ newBio: bioInput })

      setDoctor({
        ...doctor,
        bio: bioInput
      })

      setEditingBio(false)

    } catch (err) {
      console.error(err)
    }
  }

  const handleAddWorkDay = async () => {
    try {

      await addWorkDaySchedule(newWorkDay)

      setDoctor({
        ...doctor,
        workDays: [...doctor.workDays, newWorkDay]
      })

      setNewWorkDay({
        dayOfWeek: 0,
        workTimes: [{ start: "", end: "" }]
      })

    } catch (err) {
      console.error(err)
    }
  }

  const handleRemoveWorkDay = async (dayOfWeek: number) => {
    try {

      await removeWorkDaySchedule({ dayOfWeek })

      setDoctor({
        ...doctor,
        workDays: doctor.workDays.filter(
          w => w.dayOfWeek !== dayOfWeek
        )
      })

    } catch (err) {
      console.error(err)
    }
  }

  const handleAddAvailability = async () => {
    try {

      if (newAvailability.type === 1)
        await addExtraAvailability(newAvailability)
      else
        await addUnavailability(newAvailability)

      setDoctor({
        ...doctor,
        availabilityExceptions: [
          ...doctor.availabilityExceptions,
          newAvailability
        ]
      })

      setNewAvailability({
        start: "",
        end: "",
        reason: "",
        type: 1
      })

    } catch (err) {
      console.error(err)
    }
  }

  const handleRemoveAvailability = async (
    availability: DoctorAvailabilityExceptionDto
  ) => {
    try {

      if (availability.type === 1)
        await deleteExtraAvailability({
          start: availability.start,
          end: availability.end
        })
      else
        await deleteUnavailability({
          start: availability.start,
          end: availability.end
        })

      setDoctor({
        ...doctor,
        availabilityExceptions:
          doctor.availabilityExceptions.filter(
            a => a.start !== availability.start
          )
      })

    } catch (err) {
      console.error(err)
    }
  }

  return (
    <div className="space-y-8 max-w-6xl mx-auto p-6">

      <PersonProfileCard doctor={doctor} />

      <Card>

        <CardHeader>
          <CardTitle>Bio</CardTitle>
        </CardHeader>

        <CardContent>

          {!editingBio ? (

            <div className="flex justify-between items-start">

              <p>{doctor.bio}</p>

              <Button
                variant="outline"
                size="sm"
                onClick={() => setEditingBio(true)}
              >
                Edit
              </Button>

            </div>

          ) : (

            <div className="space-y-2">

              <Textarea
                value={bioInput}
                onChange={(e) => setBioInput(e.target.value)}
              />

              <div className="flex gap-2">

                <Button onClick={handleSaveBio}>
                  Save
                </Button>

                <Button
                  variant="secondary"
                  onClick={() => {
                    setEditingBio(false)
                    setBioInput(doctor.bio)
                  }}
                >
                  Cancel
                </Button>

              </div>

            </div>

          )}

        </CardContent>

      </Card>

      <DoctorSchedule
        initialAppointments={appointments}
        fetchAppointments={getMyAppointments}
        onAppointmentClick={(appointment) => {
          router.push(`/doctors/appointment/${appointment.id}`)
        }}
      />

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
                    {day.workTimes
                      .map(wt => `${wt.start}-${wt.end}`)
                      .join(", ")}
                  </TableCell>

                  <TableCell>

                    <Button
                      size="sm"
                      variant="destructive"
                      onClick={() =>
                        handleRemoveWorkDay(day.dayOfWeek)
                      }
                    >
                      Remove
                    </Button>

                  </TableCell>

                </TableRow>

              ))}

            </TableBody>

          </Table>

          <div className="flex gap-2 mt-2">

            <Select
              onValueChange={(v) =>
                setNewWorkDay({
                  ...newWorkDay,
                  dayOfWeek: Number(v)
                })
              }
            >

              <SelectTrigger className="w-[150px]">
                <SelectValue placeholder="Day" />
              </SelectTrigger>

              <SelectContent>

                {[
                  "Sunday",
                  "Monday",
                  "Tuesday",
                  "Wednesday",
                  "Thursday",
                  "Friday",
                  "Saturday"
                ].map((d,i)=>(
                  <SelectItem key={i} value={i.toString()}>
                    {d}
                  </SelectItem>
                ))}

              </SelectContent>

            </Select>

            <Input
              type="time"
              value={newWorkDay.workTimes[0].start}
              onChange={(e) =>
                setNewWorkDay({
                  ...newWorkDay,
                  workTimes: [{
                    ...newWorkDay.workTimes[0],
                    start: e.target.value
                  }]
                })
              }
            />

            <Input
              type="time"
              value={newWorkDay.workTimes[0].end}
              onChange={(e) =>
                setNewWorkDay({
                  ...newWorkDay,
                  workTimes: [{
                    ...newWorkDay.workTimes[0],
                    end: e.target.value
                  }]
                })
              }
            />

            <Button onClick={handleAddWorkDay}>
              Add
            </Button>

          </div>

        </CardContent>

      </Card>

      <Card>

        <CardHeader>
          <CardTitle>Availability Exceptions</CardTitle>
        </CardHeader>

        <CardContent className="space-y-6">

          <div className="grid gap-3">

            {doctor.availabilityExceptions.length === 0 && (
              <p className="text-sm text-muted-foreground">
                No availability exceptions configured.
              </p>
            )}

            {doctor.availabilityExceptions.map(a => (

              <div
                key={a.start}
                className="flex items-center justify-between rounded-lg border p-4 bg-muted/30"
              >

                <div className="space-y-1">

                  <div className="flex items-center gap-2">

                    <Badge variant={a.type === 1 ? "default" : "destructive"}>
                      {a.type === 1
                        ? "Extra Availability"
                        : "Unavailable"}
                    </Badge>

                  </div>

                  <p className="text-sm text-muted-foreground">
                    {new Date(a.start).toLocaleString()} →{" "}
                    {new Date(a.end).toLocaleString()}
                  </p>

                  {a.reason && (
                    <p className="text-sm">{a.reason}</p>
                  )}

                </div>

                <Button
                  size="sm"
                  variant="destructive"
                  onClick={() => handleRemoveAvailability(a)}
                >
                  Remove
                </Button>

              </div>

            ))}

          </div>

          <div className="border-t pt-6 space-y-3">

            <h4 className="font-medium text-sm">
              Add Availability Exception
            </h4>

            <div className="grid md:grid-cols-2 lg:grid-cols-4 gap-3">

              <Select
                value={newAvailability.type.toString()}
                onValueChange={(v) =>
                  setNewAvailability({
                    ...newAvailability,
                    type: Number(v)
                  })
                }
              >

                <SelectTrigger>
                  <SelectValue placeholder="Type" />
                </SelectTrigger>

                <SelectContent>

                  <SelectItem value="1">
                    Extra Availability
                  </SelectItem>

                  <SelectItem value="2">
                    Unavailability
                  </SelectItem>

                </SelectContent>

              </Select>

              <Input
                type="datetime-local"
                value={newAvailability.start}
                onChange={(e) =>
                  setNewAvailability({
                    ...newAvailability,
                    start: e.target.value
                  })
                }
              />

              <Input
                type="datetime-local"
                value={newAvailability.end}
                onChange={(e) =>
                  setNewAvailability({
                    ...newAvailability,
                    end: e.target.value
                  })
                }
              />

              <Input
                placeholder="Reason"
                value={newAvailability.reason}
                onChange={(e) =>
                  setNewAvailability({
                    ...newAvailability,
                    reason: e.target.value
                  })
                }
              />

            </div>

            <Button onClick={handleAddAvailability}>
              Add Exception
            </Button>

          </div>

        </CardContent>

      </Card>

    </div>
  )
}