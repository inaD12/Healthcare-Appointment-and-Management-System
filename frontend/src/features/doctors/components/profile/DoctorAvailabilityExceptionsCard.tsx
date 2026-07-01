"use client"

import { useState } from "react"

import {
  DoctorQueryViewModel,
  DoctorAvailabilityExceptionDto
} from "@/features/doctors/types/doctorsTypes"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent
} from "@/components/ui/card"

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue
} from "@/components/ui/select"
import { doctorService } from "../../services/doctorService"

type Props = {
  doctor: DoctorQueryViewModel
  setDoctor: (d: DoctorQueryViewModel) => void
}

export default function DoctorAvailabilityExceptionsCard({
  doctor,
  setDoctor
}: Props) {

  const [newAvailability, setNewAvailability] =
    useState<DoctorAvailabilityExceptionDto>({
      start: "",
      end: "",
      reason: "",
      type: 1
    })

  const add = async () => {

    if (newAvailability.type === 1) {
      await doctorService.addExtraAvailability(newAvailability)
    } else {
      await doctorService.addUnavailability(newAvailability)
    }

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
  }

  const remove = async (availability: DoctorAvailabilityExceptionDto) => {

    if (availability.type === 1) {
      await doctorService.deleteExtraAvailability({
        start: availability.start,
        end: availability.end
      })
    } else {
      await doctorService.deleteUnavailability({
        start: availability.start,
        end: availability.end
      })
    }

    setDoctor({
      ...doctor,
      availabilityExceptions: doctor.availabilityExceptions.filter(
        a => a.start !== availability.start
      )
    })
  }

  return (
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

                  <Badge variant={a.type === 1 ? "destructive" : "default"}>
                    {a.type === 1
                      ? "Unavailable"
                      : "Extra Availability"}
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
                onClick={() => remove(a)}
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

          <Button onClick={add}>
            Add Exception
          </Button>

        </div>

      </CardContent>

    </Card>
  )
}