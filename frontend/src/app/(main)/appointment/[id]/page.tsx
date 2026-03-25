"use client"

import { useEffect, useState } from "react"
import { useRouter, useParams } from "next/navigation"
import {
    Addendum,
  AppointmentByIdResponse,
  Diagnosis,
  EncounterDetails,
  EncounterStatus,
  Note,
  Prescription,
} from "@/features/patients/types/patientTypes"
import { getAppointmentWithEncounters } from "@/features/patients/services/patientService"

export default function AppointmentPage() {
  const router = useRouter()
  const params = useParams()
  const { id } = params as { id: string }

  const [appointment, setAppointment] = useState<AppointmentByIdResponse | null>(null)
  const [loading, setLoading] = useState<boolean>(true)
  const [error, setError] = useState<string | null>(null)
  const [openEncounter, setOpenEncounter] = useState(false)

  useEffect(() => {
    if (!id) return

    const fetchAppointment = async () => {
      setLoading(true)
      try {
        const data = await getAppointmentWithEncounters(id)
        setAppointment(data)
      } catch (err) {
        console.error(err)
        setError("Failed to fetch appointment")
      } finally {
        setLoading(false)
      }
    }

    fetchAppointment()
  }, [id])

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!appointment || appointment.appointmentById.length === 0) return <p>No appointment found</p>

  const app = appointment.appointmentById[0]

  const encounter: EncounterDetails = app.encounterDetails && app.encounterDetails.length > 0
    ? app.encounterDetails[0]
    : {
        id: "example-encounter",
        startedAt: new Date().toISOString(),
        finalizedAt: null,
        status: EncounterStatus.InProgress,
        notes: [
          { id: "note-1", text: "Example note", createdAt: new Date().toISOString() } as Note
        ],
        diagnoses: [
          { id: "diag-1", icdCode: "A00", description: "Example diagnosis" } as Diagnosis
        ],
        prescriptions: [
          { id: "pres-1", medicationName: "ExampleMed", dosage: "10mg", instructions: "Once daily" } as Prescription
        ],
        addendums: [
          { id: "add-1", text: "Example addendum", createdAt: new Date().toISOString() } as Addendum
        ]
      }

  return (
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Appointment Details</h1>
      <div className="border rounded p-4 mb-6 shadow-sm">
        <p><strong>Doctor:</strong> {app.doctorName}</p>
        <p><strong>Status:</strong> {app.status}</p>
        <p><strong>Start:</strong> {new Date(app.start).toLocaleString()}</p>
        <p><strong>End:</strong> {new Date(app.end).toLocaleString()}</p>
      </div>

      <h2 className="text-xl font-semibold mb-2">Encounter</h2>

      {!app.encounterDetails || app.encounterDetails.length === 0 ? (
        <p className="text-gray-500">No encounter has been created yet. Showing example encounter for demo:</p>
      ) : null}

      <div className="border rounded mb-4 shadow-sm">
        <button
          className="w-full text-left px-4 py-2 bg-gray-100 hover:bg-gray-200 font-medium"
          onClick={() => setOpenEncounter(prev => !prev)}
        >
          Encounter {encounter.id} - Status: {encounter.status}
        </button>

        {openEncounter && (
          <div className="p-4 bg-white">
            <p><strong>Started At:</strong> {new Date(encounter.startedAt).toLocaleString()}</p>
            <p><strong>Finalized At:</strong> {encounter.finalizedAt ? new Date(encounter.finalizedAt).toLocaleString() : "N/A"}</p>

            <div className="mt-2">
              <h3 className="font-semibold">Notes:</h3>
              {encounter.notes.length === 0 ? <p>No notes</p> : (
                <ul className="list-disc list-inside">
                  {encounter.notes.map(note => (
                    <li key={note.id}>
                      {note.text} <span className="text-gray-500">({new Date(note.createdAt).toLocaleString()})</span>
                    </li>
                  ))}
                </ul>
              )}
            </div>

            <div className="mt-2">
              <h3 className="font-semibold">Diagnoses:</h3>
              {encounter.diagnoses.length === 0 ? <p>No diagnoses</p> : (
                <ul className="list-disc list-inside">
                  {encounter.diagnoses.map(d => (
                    <li key={d.id}>{d.icdCode} - {d.description}</li>
                  ))}
                </ul>
              )}
            </div>

            <div className="mt-2">
              <h3 className="font-semibold">Prescriptions:</h3>
              {encounter.prescriptions.length === 0 ? <p>No prescriptions</p> : (
                <ul className="list-disc list-inside">
                  {encounter.prescriptions.map(p => (
                    <li key={p.id}>
                      {p.medicationName} - {p.dosage} ({p.instructions})
                    </li>
                  ))}
                </ul>
              )}
            </div>

            <div className="mt-2">
              <h3 className="font-semibold">Addendums:</h3>
              {encounter.addendums.length === 0 ? <p>No addendums</p> : (
                <ul className="list-disc list-inside">
                  {encounter.addendums.map(a => (
                    <li key={a.id}>
                      {a.text} <span className="text-gray-500">({new Date(a.createdAt).toLocaleString()})</span>
                    </li>
                  ))}
                </ul>
              )}
            </div>
          </div>
        )}
      </div>
    </div>
  )
}