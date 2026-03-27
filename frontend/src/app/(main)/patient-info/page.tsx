"use client"

import { useEffect, useState } from "react"
import { Card, CardContent, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { getPatientDashboard } from "@/features/patients/services/patientService"
import { Appointment, AppointmentStatus, PatientProfile } from "@/features/patients/types/patientTypes"
import { cancelAppointment } from "@/features/appointments/services/appointmentService"
import { useRouter } from "next/navigation"

const PAGE_SIZE = 5

export default function PatientDashboardPage() {
  useAuthGuard()
  const router = useRouter()

  const [patient, setPatient] = useState<PatientProfile | null>(null)
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [currentPage, setCurrentPage] = useState(1)
  const [loading, setLoading] = useState(true)

  async function fetchData() {
    try {
      const data = await getPatientDashboard()
      setPatient(data.profile)
      const sorted = data.appointments.sort((a, b) =>
        new Date(b.start).getTime() - new Date(a.start).getTime()
      )
      setAppointments(sorted)
    } catch (err) {
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  const totalPages = Math.ceil(appointments.length / PAGE_SIZE)
  const pagedAppointments = appointments.slice(
    (currentPage - 1) * PAGE_SIZE,
    currentPage * PAGE_SIZE
  )

  const handleCancel = async (id: string) => {
    try {
      await cancelAppointment(id)
      setAppointments(prev => prev.filter(a => a.id !== id))
    } catch (err) {
      console.error("Error canceling appointment:", err)
    }
  }

  const handleReschedule = (appointmentId: string, doctorId: string) => {
    router.push(`/doctors/${doctorId}?rescheduleId=${appointmentId}`)
  }

  const handleViewDetails = (appointmentId: string) => {
    router.push(`/appointment/${appointmentId}`)
  }

  if (loading) return <p className="p-8 text-center">Loading...</p>

  return (
    <div className="max-w-5xl mx-auto p-8 space-y-8">

      <Card className="p-6 shadow-lg">
        <CardTitle className="text-xl font-semibold">Patient Info</CardTitle>
        <CardContent className="mt-2 space-y-1">
          <p><strong>Name:</strong> {patient?.fullName}</p>
          <p><strong>Birth Date:</strong> {patient?.birthDate}</p>
          <p><strong>Allergies:</strong> {patient?.allergies.join(", ") || "None"}</p>
          <p><strong>Conditions:</strong> {patient?.conditions.join(", ") || "None"}</p>
        </CardContent>
      </Card>

      <Card className="p-6 shadow-lg">
        <CardTitle className="text-xl font-semibold">Appointments</CardTitle>
        <CardContent className="mt-3 space-y-4">
          {pagedAppointments.length === 0 ? (
            <p className="text-gray-500">No appointments found.</p>
          ) : (
           pagedAppointments.map(a => (
            <div
              key={a.id}
              className="border p-4 rounded-lg flex justify-between items-center hover:shadow cursor-pointer"
              onClick={() => handleViewDetails(a.id)}
            >
              <div>
                <p><strong>Status:</strong> {a.status}</p>
                <p>
                  <strong>Time:</strong>{" "}
                  {new Date(a.start).toLocaleString()} → {new Date(a.end).toLocaleString()}
                </p>
                <p><strong>Doctor:</strong> {a.doctorName || "Unknown"}</p>
              </div>
              <div className="flex gap-2">
                {a.status === AppointmentStatus.Scheduled && ( 
                  <>
                    <Button
                      variant="destructive"
                      size="sm"
                      onClick={(e) => {
                        e.stopPropagation()
                        handleCancel(a.id)
                      }}
                    >
                      Cancel
                    </Button>
                    <Button
                      variant="outline"
                      size="sm"
                      onClick={(e) => {
                        e.stopPropagation()
                        handleReschedule(a.id, a.doctorId)
                      }}
                    >
                      Reschedule
                    </Button>
                  </>
                )}
              </div>
            </div>
          ))
          )}

          {totalPages > 1 && (
            <div className="flex justify-center gap-2 mt-4">
              <Button
                size="sm"
                disabled={currentPage === 1}
                onClick={() => setCurrentPage(prev => prev - 1)}
              >
                Previous
              </Button>
              <span className="flex items-center px-2">
                Page {currentPage} of {totalPages}
              </span>
              <Button
                size="sm"
                disabled={currentPage === totalPages}
                onClick={() => setCurrentPage(prev => prev + 1)}
              >
                Next
              </Button>
            </div>
          )}
        </CardContent>
      </Card>

    </div>
  )
}