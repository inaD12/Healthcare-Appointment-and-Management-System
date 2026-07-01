"use client"

import { useEffect, useState } from "react"

import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent
} from "@/components/ui/card"

import { Button } from "@/components/ui/button"
import { Textarea } from "@/components/ui/textarea"
import { doctorService } from "../../services/doctorService"

type Props = {
  doctor: DoctorQueryViewModel
  setDoctor: (d: DoctorQueryViewModel) => void
}


export default function DoctorBioCard({
  doctor,
  setDoctor
}: Props) {

  const [editing, setEditing] = useState(false)
  const [bio, setBio] = useState(doctor.bio ?? "")

  const save = async () => {

    await doctorService.updateDoctorInfo({ newBio: bio })

    setDoctor({
      ...doctor,
      bio
    })

    setEditing(false)
  }

  useEffect(() => {
    setBio(doctor.bio ?? "")
  }, [doctor.bio])

  return (
    <Card>

      <CardHeader>
        <CardTitle>Bio</CardTitle>
      </CardHeader>

      <CardContent>

        {!editing ? (

          <div className="flex justify-between items-start">

            <p>{doctor.bio}</p>

            <Button
              variant="outline"
              size="sm"
              onClick={() => setEditing(true)}
            >
              Edit
            </Button>

          </div>

        ) : (

          <div className="space-y-2">

            <Textarea
              value={bio}
              onChange={(e) => setBio(e.target.value)}
            />

            <div className="flex gap-2">

              <Button onClick={save}>
                Save
              </Button>

              <Button
                variant="secondary"
                onClick={() => {
                  setEditing(false)
                  setBio(doctor.bio)
                }}
              >
                Cancel
              </Button>

            </div>

          </div>

        )}

      </CardContent>

    </Card>
  )
}