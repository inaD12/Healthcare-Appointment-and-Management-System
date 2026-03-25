"use client"

import { useEffect, useState } from "react"
import { Card, CardContent, CardTitle } from "@/components/ui/card"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { getPatientDashboard } from "@/features/patients/services/patientService"
import { Appointment, PatientProfile } from "@/features/patients/types/patientTypes"


export default function PatientDashboardPage() {
  const auth = useAuthGuard()

  const [patient, setPatient] = useState<PatientProfile | null>(null)
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [loading, setLoading] = useState(true)

  async function fetchData() {
    try {
      const data = await getPatientDashboard()
      setPatient(data)
      setAppointments(data.appointments)
    } catch (err) {
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  if (loading) return <p className="p-8">Loading...</p>

  return (
    <div className="max-w-4xl mx-auto p-8 space-y-6">

      <Card className="p-4">
        <CardTitle>Patient Info</CardTitle>
        <CardContent>
          <p><strong>Name:</strong> {patient?.fullName}</p>
          <p><strong>Birth Date:</strong> {patient?.birthDate}</p>
        </CardContent>
      </Card>

      <Card className="p-4">
        <CardTitle>Appointments</CardTitle>
        <CardContent className="space-y-3">
          {appointments.length === 0 ? (
            <p className="text-gray-500">No appointments found.</p>
          ) : (
            appointments.map(a => (
              <div
                key={a.id}
                className="border p-3 rounded-lg flex justify-between"
              >
                <div>
                  <p><strong>Status:</strong> {a.status}</p>
                  <p>
                    {new Date(a.start).toLocaleString()} →{" "}
                    {new Date(a.end).toLocaleString()}
                  </p>
                </div>
                <div className="text-sm text-gray-500">
                  <strong>Doctor:</strong> {a.doctorName || "Unknown"}
                </div>
              </div>
            ))
          )}
        </CardContent>
      </Card>

    </div>
  )
}