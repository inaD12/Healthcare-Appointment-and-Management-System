"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"
import { getDoctorById } from "@/features/doctors/services/doctorService"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Button } from "@/components/ui/button"

export default function DoctorDetailsPage() {
  const { id } = useParams()
  const [doctor, setDoctor] = useState<DoctorQueryViewModel | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState("")

  async function fetchDoctor() {
    try {
      const res = await getDoctorById(id as string)
      setDoctor(res.data.data)
    } catch (err: any) {
      if (err.response?.status === 404) {
        setError("Doctor not found.")
      } else if (err.response?.status === 403) {
        setError("You don’t have permission to view this doctor.")
      } else {
        setError("Failed to load doctor.")
      }
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (id) fetchDoctor()
  }, [id])

  if (loading) return <p className="p-8">Loading doctor...</p>

  if (error)
    return (
      <div className="p-8">
        <Card className="border-red-400 bg-red-50">
          <CardContent className="text-red-700">{error}</CardContent>
        </Card>
      </div>
    )

  if (!doctor) return null

  return (
    <div className="p-8 max-w-3xl mx-auto space-y-6">
      <Card>
        <CardContent className="space-y-3">
          <CardTitle className="text-2xl">
            {doctor.firstName} {doctor.lastName}
          </CardTitle>

          <CardDescription>{doctor.bio}</CardDescription>

          <p>
            <b>Specialities:</b> {doctor.specialities.join(", ")}
          </p>

          <p className="text-gray-500 text-sm">
            Timezone: {doctor.timeZoneId}
          </p>
        </CardContent>
      </Card>

      <Card>
        <CardContent className="flex justify-between items-center">
          <div>
            <p className="font-medium">Book an appointment</p>
            <p className="text-sm text-gray-500">
              Choose a time and schedule your visit
            </p>
          </div>
          <Button onClick={() => alert("TODO: Open booking flow")}>
            Book Appointment
          </Button>
        </CardContent>
      </Card>
    </div>
  )
}