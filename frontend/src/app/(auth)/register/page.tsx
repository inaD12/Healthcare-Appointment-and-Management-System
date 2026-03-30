"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"
import { useForm } from "react-hook-form"
import { zodResolver } from "@hookform/resolvers/zod"

import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Separator } from "@/components/ui/separator"
import { Textarea } from "@/components/ui/textarea"

import {
  getAppointmentWithEncounters,
  patientService,
} from "@/features/patients/services/patientService"

import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"

import {
  Appointment,
  EncounterStatus,
  Note,
  Diagnosis,
  Prescription,
  Addendum,
  AddNoteSchema,
  AddDiagnosisSchema,
  AddAddendumSchema,
  PrescribeMedicationSchema,
} from "@/features/patients/types/patientTypes"

import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

export default function DoctorAppointmentPage() {
  useAuthGuard()

  const params = useParams()
  const { id } = params as { id: string }

  const [appointment, setAppointment] = useState<Appointment | null>(null)
  const [encounterId, setEncounterId] = useState<string | null>(null)
  const [encounterStatus, setEncounterStatus] =
    useState<EncounterStatus | null>(null)

  const [notes, setNotes] = useState<Note[]>([])
  const [diagnoses, setDiagnoses] = useState<Diagnosis[]>([])
  const [prescriptions, setPrescriptions] = useState<Prescription[]>([])
  const [addendums, setAddendums] = useState<Addendum[]>([])

  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const isInProgress = encounterStatus === EncounterStatus.InProgress
  const isFinalized = encounterStatus === EncounterStatus.Finalized
  const isLocked = encounterStatus === EncounterStatus.Locked

  const canEditMedical = isInProgress
  const canAddAddendum = isFinalized
  const showFinalize = isInProgress
  const showLock = !isLocked
  const canFinalizeOrLock = diagnoses.length > 0

  useEffect(() => {
    if (!id) return

    const fetchAppointment = async () => {
      try {
        const res = await getAppointmentWithEncounters(id)
        const app = mapAppointmentResponseToAppointment(res)

        if (!app) throw new Error("Appointment not found")

        setAppointment(app)

        const encounter = app.encounterDetails

        if (encounter) {
          setEncounterId(encounter.id)
          setEncounterStatus(encounter.status)
          setNotes(encounter.notes || [])
          setDiagnoses(encounter.diagnoses || [])
          setPrescriptions(encounter.prescriptions || [])
          setAddendums(encounter.addendums || [])
        }
      } catch {
        setError("Failed to load appointment")
      } finally {
        setLoading(false)
      }
    }

    fetchAppointment()
  }, [id])

  /*
  -------------------------
  NOTE FORM
  -------------------------
  */

  const {
    register: registerNote,
    handleSubmit: handleSubmitNote,
    reset: resetNote,
    formState: { errors: noteErrors, isSubmitting: noteSubmitting },
  } = useForm({
    resolver: zodResolver(AddNoteSchema),
  })

  const onAddNote = async (data: { note: string }) => {
    const res = await patientService.addNote(encounterId!, data)

    setNotes([
      ...notes,
      { id: res.data.data.id, text: data.note, createdAt: new Date().toISOString() },
    ])

    resetNote()
  }

  /*
  -------------------------
  DIAGNOSIS FORM
  -------------------------
  */

  const {
    register: registerDiagnosis,
    handleSubmit: handleSubmitDiagnosis,
    reset: resetDiagnosis,
    formState: { errors: diagnosisErrors, isSubmitting: diagnosisSubmitting },
  } = useForm({
    resolver: zodResolver(AddDiagnosisSchema),
  })

  const onAddDiagnosis = async (data: any) => {
    const res = await patientService.addDiagnosis(encounterId!, data)

    setDiagnoses([...diagnoses, { ...data, id: res.data.data.id }])

    resetDiagnosis()
  }

  /*
  -------------------------
  PRESCRIPTION FORM
  -------------------------
  */

  const {
    register: registerPrescription,
    handleSubmit: handleSubmitPrescription,
    reset: resetPrescription,
    formState: { errors: prescriptionErrors, isSubmitting: prescriptionSubmitting },
  } = useForm({
    resolver: zodResolver(PrescribeMedicationSchema),
  })

  const onAddPrescription = async (data: any) => {
    const res = await patientService.prescribeMedication(encounterId!, data)

    setPrescriptions([
      ...prescriptions,
      {
        id: res.data.data.id,
        medicationName: data.name,
        dosage: data.dosage,
        instructions: data.instructions,
      },
    ])

    resetPrescription()
  }

  /*
  -------------------------
  ADDENDUM FORM
  -------------------------
  */

  const {
    register: registerAddendum,
    handleSubmit: handleSubmitAddendum,
    reset: resetAddendum,
    formState: { errors: addendumErrors, isSubmitting: addendumSubmitting },
  } = useForm({
    resolver: zodResolver(AddAddendumSchema),
  })

  const onAddAddendum = async (data: { note: string }) => {
    const res = await patientService.addAddendum(encounterId!, data)

    setAddendums([
      ...addendums,
      { id: res.data.data.id, text: data.note, createdAt: new Date().toISOString() },
    ])

    resetAddendum()
  }

  /*
  -------------------------
  ENCOUNTER ACTIONS
  -------------------------
  */

  const handleStartEncounter = async () => {
    if (!appointment) return

    const res = await patientService.startEncounter({
      appointmentId: appointment.id,
    })

    setEncounterId(res.data.data.id)
    setEncounterStatus(EncounterStatus.InProgress)
  }

  const handleFinalizeEncounter = async () => {
    if (!encounterId || !canFinalizeOrLock) return

    await patientService.finalizeEncounter(encounterId)

    setEncounterStatus(EncounterStatus.Finalized)
  }

  const handleLockEncounter = async () => {
    if (!encounterId || !canFinalizeOrLock) return

    await patientService.lockEncounter(encounterId)

    setEncounterStatus(EncounterStatus.Locked)
  }

  /*
  -------------------------
  RENDER
  -------------------------
  */

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!appointment) return <p>No appointment found</p>

  return (
    <div className="max-w-4xl mx-auto p-6 space-y-6">
      <h1 className="text-3xl font-bold">Appointment</h1>

      <Card>
        <CardHeader>
          <CardTitle>Patient {appointment.patientId}</CardTitle>
        </CardHeader>
        <CardContent className="space-y-2">
          <Badge>{appointment.status}</Badge>
          <p>
            <strong>Start:</strong>{" "}
            {new Date(appointment.start).toLocaleString()}
          </p>
          <p>
            <strong>End:</strong> {new Date(appointment.end).toLocaleString()}
          </p>
        </CardContent>
      </Card>

      {!encounterId && (
        <Card>
          <CardContent className="p-6">
            <p className="mb-4">No encounter started.</p>
            <Button onClick={handleStartEncounter}>Start Encounter</Button>
          </CardContent>
        </Card>
      )}

      {encounterId && (
        <Card>
          <CardHeader>
            <CardTitle>Encounter</CardTitle>
          </CardHeader>

          <CardContent className="space-y-6">

            {/* NOTES */}
            <div>
              <h3 className="font-semibold mb-2">Notes</h3>

              {notes.map((n) => (
                <div key={n.id} className="border p-2 rounded mb-2">
                  {n.text}
                </div>
              ))}

              {canEditMedical && (
                <form onSubmit={handleSubmitNote(onAddNote)} className="space-y-2">
                  <Textarea placeholder="Add note" {...registerNote("note")} />
                  {noteErrors.note && (
                    <p className="text-red-600 text-sm">
                      {noteErrors.note.message}
                    </p>
                  )}
                  <Button type="submit" disabled={noteSubmitting}>
                    Add Note
                  </Button>
                </form>
              )}
            </div>

            <Separator />

            {/* DIAGNOSIS */}
            <div>
              <h3 className="font-semibold mb-2">Diagnoses</h3>

              {diagnoses.map((d) => (
                <div key={d.id} className="border p-2 rounded mb-2">
                  {d.icdCode} — {d.description}
                </div>
              ))}

              {canEditMedical && (
                <form
                  onSubmit={handleSubmitDiagnosis(onAddDiagnosis)}
                  className="space-y-2"
                >
                  <Input placeholder="ICD Code" {...registerDiagnosis("icdCode")} />
                  {diagnosisErrors.icdCode && (
                    <p className="text-red-600 text-sm">
                      {diagnosisErrors.icdCode.message}
                    </p>
                  )}

                  <Input
                    placeholder="Description"
                    {...registerDiagnosis("description")}
                  />
                  {diagnosisErrors.description && (
                    <p className="text-red-600 text-sm">
                      {diagnosisErrors.description.message}
                    </p>
                  )}

                  <Button type="submit" disabled={diagnosisSubmitting}>
                    Add Diagnosis
                  </Button>
                </form>
              )}
            </div>

            <Separator />

            {/* PRESCRIPTIONS */}
            <div>
              <h3 className="font-semibold mb-2">Prescriptions</h3>

              {prescriptions.map((p) => (
                <div key={p.id} className="border p-2 rounded mb-2">
                  <p>
                    <strong>{p.medicationName}</strong>
                  </p>
                  <p>Dosage: {p.dosage}</p>
                  <p>Instructions: {p.instructions}</p>
                </div>
              ))}

              {canEditMedical && (
                <form
                  onSubmit={handleSubmitPrescription(onAddPrescription)}
                  className="space-y-2"
                >
                  <Input
                    placeholder="Medication"
                    {...registerPrescription("name")}
                  />
                  {prescriptionErrors.name && (
                    <p className="text-red-600 text-sm">
                      {prescriptionErrors.name.message}
                    </p>
                  )}

                  <Input placeholder="Dosage" {...registerPrescription("dosage")} />
                  {prescriptionErrors.dosage && (
                    <p className="text-red-600 text-sm">
                      {prescriptionErrors.dosage.message}
                    </p>
                  )}

                  <Input
                    placeholder="Instructions"
                    {...registerPrescription("instructions")}
                  />
                  {prescriptionErrors.instructions && (
                    <p className="text-red-600 text-sm">
                      {prescriptionErrors.instructions.message}
                    </p>
                  )}

                  <Button type="submit" disabled={prescriptionSubmitting}>
                    Prescribe
                  </Button>
                </form>
              )}
            </div>

            <Separator />

            {/* ADDENDUM */}
            <div>
              <h3 className="font-semibold mb-2">Addendums</h3>

              {addendums.map((a) => (
                <div key={a.id} className="border p-2 rounded mb-2">
                  {a.text}
                </div>
              ))}

              {canAddAddendum && (
                <form
                  onSubmit={handleSubmitAddendum(onAddAddendum)}
                  className="space-y-2"
                >
                  <Textarea
                    placeholder="Add addendum"
                    {...registerAddendum("note")}
                  />

                  {addendumErrors.note && (
                    <p className="text-red-600 text-sm">
                      {addendumErrors.note.message}
                    </p>
                  )}

                  <Button type="submit" disabled={addendumSubmitting}>
                    Add Addendum
                  </Button>
                </form>
              )}
            </div>

            <Separator />

            <div className="flex gap-4">
              {showLock && (
                <Button
                  variant="secondary"
                  onClick={handleLockEncounter}
                  disabled={!canFinalizeOrLock}
                >
                  Lock Encounter
                </Button>
              )}

              {showFinalize && (
                <Button
                  onClick={handleFinalizeEncounter}
                  disabled={!canFinalizeOrLock}
                >
                  Finalize Encounter
                </Button>
              )}
            </div>

          </CardContent>
        </Card>
      )}
    </div>
  )
}