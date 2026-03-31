"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"

import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Separator } from "@/components/ui/separator"
import { Textarea } from "@/components/ui/textarea"

import { getAppointmentWithEncounters, patientService } from "@/features/patients/services/patientService"
import { mapAppointmentResponseToAppointment } from "@/features/patients/mappers/appointmentMapper"
import { Appointment, EncounterStatus, Note, Diagnosis, Prescription, Addendum, AddAddendumSchema, AddDiagnosisSchema, AddNoteSchema, PrescribeMedicationSchema, AppointmentStatus } from "@/features/patients/types/patientTypes"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"
import { getRatingByAppointment } from "@/features/ratings/services/ratingService"

export default function DoctorAppointmentPage() {
  useAuthGuard()

  const params = useParams()
  const { id } = params as { id: string }

  const [appointment, setAppointment] = useState<Appointment | null>(null)
  const [encounterId, setEncounterId] = useState<string | null>(null)
  const [encounterStatus, setEncounterStatus] = useState<EncounterStatus | null>(null)
  const [notes, setNotes] = useState<Note[]>([])
  const [diagnoses, setDiagnoses] = useState<Diagnosis[]>([])
  const [prescriptions, setPrescriptions] = useState<Prescription[]>([])
  const [addendums, setAddendums] = useState<Addendum[]>([])

  const [rating, setRating] = useState<RatingQueryViewModel | null>(null)

  const [newNote, setNewNote] = useState("")
  const [newDiagnosis, setNewDiagnosis] = useState({ icdCode: "", description: "" })
  const [newPrescription, setNewPrescription] = useState({ medicationName: "", dosage: "", instructions: "" })
  const [newAddendum, setNewAddendum] = useState("")

  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [noteError, setNoteError] = useState<string | null>(null)
  const [diagnosisError, setDiagnosisError] = useState<{ icdCode?: string; description?: string }>({})
  const [prescriptionError, setPrescriptionError] = useState<{ medicationName?: string; dosage?: string; instructions?: string }>({})
  const [addendumError, setAddendumError] = useState<string | null>(null)

  const isInProgress = encounterStatus === EncounterStatus.InProgress
  const isFinalized = encounterStatus === EncounterStatus.Finalized
  const isLocked = encounterStatus === EncounterStatus.Locked

  const canEditMedical = isInProgress
  const canAddAddendum = isFinalized
  const showFinalize = isInProgress
  const showLock = !isLocked
  const canFinalizeOrLock = diagnoses.length > 0

  const canUseEncounter =
  appointment?.status === AppointmentStatus.Scheduled ||
  appointment?.status === AppointmentStatus.Completed

  useEffect(() => {
    if (!id) return
    const fetchAppointment = async () => {
      try {
        const res = await getAppointmentWithEncounters(id)
        const app = mapAppointmentResponseToAppointment(res)
        if (!app) throw new Error("Appointment not found")
        setAppointment(app)

        if (app.status === AppointmentStatus.Completed) {
          try {
            const ratingRes = await getRatingByAppointment(app.id)
            setRating(ratingRes.data.data)
          } catch (err: any) {
            if (err.response?.status !== 404) {
              console.error("Failed to fetch rating")
            }
          }
        }

        const encounter = app.encounterDetails
        if (encounter && (app.status === AppointmentStatus.Scheduled || app.status === AppointmentStatus.Completed)) {
          setEncounterId(encounter.id)
          setEncounterStatus(encounter.status)
          setNotes(encounter.notes || [])
          setDiagnoses(encounter.diagnoses || [])
          setPrescriptions(encounter.prescriptions || [])
          setAddendums(encounter.addendums || [])
        }
        
      } catch (err) {
        setError("Failed to load appointment")
      } finally {
        setLoading(false)
      }
    }
    fetchAppointment()
  }, [id])

 const parseZodErrors = (err: any) => {
  const formatted: Record<string, string> = {}

  if (err?.issues) {
    err.issues.forEach((issue: any) => {
      if (issue.path?.[0]) {
        formatted[issue.path[0]] = issue.message
      }
    })
  }

  return formatted
}

  const handleStartEncounter = async () => {
    if (!appointment || !canUseEncounter) return

    const res = await patientService.startEncounter({
      appointmentId: appointment.id
    })

    setEncounterId(res.data.data.id)
    setEncounterStatus(EncounterStatus.InProgress)
  }

  const handleAddNote = async () => {
    try {
      AddNoteSchema.parse({ note: newNote })
      setNoteError(null)
      const res = await patientService.addNote(encounterId!, { note: newNote })
      setNotes([...notes, { id: res.data.data.id, text: newNote, createdAt: new Date().toISOString() }])
      setNewNote("")
    } catch (err: any) {
      setNoteError(parseZodErrors(err).note || "Invalid note")
    }
  }

  const handleAddDiagnosis = async () => {
    try {
      AddDiagnosisSchema.parse(newDiagnosis)
      setDiagnosisError({})
      const res = await patientService.addDiagnosis(encounterId!, newDiagnosis)
      setDiagnoses([...diagnoses, { ...newDiagnosis, id: res.data.data.id }])
      setNewDiagnosis({ icdCode: "", description: "" })
    } catch (err: any) {
      setDiagnosisError(parseZodErrors(err))
    }
  }

  const handleAddPrescription = async () => {
    try {
      PrescribeMedicationSchema.parse({
        name: newPrescription.medicationName,
        dosage: newPrescription.dosage,
        instructions: newPrescription.instructions
      })
      setPrescriptionError({})
      const res = await patientService.prescribeMedication(encounterId!, {
        name: newPrescription.medicationName,
        dosage: newPrescription.dosage,
        instructions: newPrescription.instructions
      })
      setPrescriptions([...prescriptions, { ...newPrescription, id: res.data.data.id }])
      setNewPrescription({ medicationName: "", dosage: "", instructions: "" })
    } catch (err: any) {
      setPrescriptionError(parseZodErrors(err))
    }
  }

  const handleAddAddendum = async () => {
    try {
      AddAddendumSchema.parse({ note: newAddendum })
      setAddendumError(null)
      const res = await patientService.addAddendum(encounterId!, { note: newAddendum })
      setAddendums([...addendums, { id: res.data.data.id, text: newAddendum, createdAt: new Date().toISOString() }])
      setNewAddendum("")
    } catch (err: any) {
      setAddendumError(parseZodErrors(err).note || "Invalid addendum")
    }
  }

  const handleLockEncounter = async () => {
    if (!encounterId || !canFinalizeOrLock) return
    await patientService.lockEncounter(encounterId)
    setEncounterStatus(EncounterStatus.Locked)
  }

  const handleFinalizeEncounter = async () => {
    if (!encounterId || !canFinalizeOrLock) return
    await patientService.finalizeEncounter(encounterId)
    setEncounterStatus(EncounterStatus.Finalized)
  }

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!appointment) return <p>No appointment found</p>

  return (
    <div className="max-w-4xl mx-auto p-6 space-y-6">
      <h1 className="text-3xl font-bold">Appointment</h1>

      <Card>
        <CardHeader>
          <CardTitle>Patient Name: {appointment.patientName}</CardTitle>
        </CardHeader>
        <CardContent className="space-y-2">
          <Badge>{appointment.status}</Badge>
          <p><strong>Start:</strong> {new Date(appointment.start).toLocaleString()}</p>
          <p><strong>End:</strong> {new Date(appointment.end).toLocaleString()}</p>
        </CardContent>
      </Card>

      {rating && (
        <Card>
          <CardHeader>
            <CardTitle>Patient Rating</CardTitle>
          </CardHeader>

          <CardContent className="space-y-3">

            <div className="flex text-3xl text-yellow-400">
              {[1,2,3,4,5].map((star)=>(
                <span key={star}>
                  {star <= rating.score ? "★" : "☆"}
                </span>
              ))}
            </div>

            {rating.comment && (
              <p className="italic text-muted-foreground">
                "{rating.comment}"
              </p>
            )}

            <p className="text-sm text-muted-foreground">
              Submitted {new Date(rating.createdAt).toLocaleString()}
            </p>

          </CardContent>
        </Card>
      )}

      {canUseEncounter && !encounterId && (
        <Card>
          <CardContent className="p-6">
            <p className="mb-4">No encounter started.</p>
            <Button onClick={handleStartEncounter}>Start Encounter</Button>
          </CardContent>
        </Card>
      )}

      {canUseEncounter && encounterId && (
        <Card>
          <CardHeader>
            <CardTitle>Encounter</CardTitle>
          </CardHeader>
          <CardContent className="space-y-6">

            <div>
              <h3 className="font-semibold mb-2">Notes</h3>
              {notes.map(n => <div key={n.id} className="border p-2 rounded mb-2">{n.text}</div>)}
              {canEditMedical && (
                <>
                  <Textarea value={newNote} onChange={(e) => setNewNote(e.target.value)} placeholder="Add note"/>
                  {noteError && <p className="text-red-600">{noteError}</p>}
                  <Button className="mt-2" onClick={handleAddNote}>Add Note</Button>
                </>
              )}
            </div>

            <Separator />

            <div>
              <h3 className="font-semibold mb-2">Diagnoses</h3>
              {diagnoses.map(d => <div key={d.id} className="border p-2 rounded mb-2">{d.icdCode} — {d.description}</div>)}
              {canEditMedical && (
                <>
                  <div className="flex flex-col gap-1">
                    <Input placeholder="ICD Code" value={newDiagnosis.icdCode} onChange={(e) => setNewDiagnosis({ ...newDiagnosis, icdCode: e.target.value })}/>
                    {diagnosisError.icdCode && <p className="text-red-600 text-sm">{diagnosisError.icdCode}</p>}
                    <Input placeholder="Description" value={newDiagnosis.description} onChange={(e) => setNewDiagnosis({ ...newDiagnosis, description: e.target.value })}/>
                    {diagnosisError.description && <p className="text-red-600 text-sm">{diagnosisError.description}</p>}
                  </div>
                  <Button className="mt-2" onClick={handleAddDiagnosis}>Add Diagnosis</Button>
                </>
              )}
            </div>

            <Separator />

            <div>
              <h3 className="font-semibold mb-2">Prescriptions</h3>
              {prescriptions.map(p => (
                <div key={p.id} className="border p-2 rounded mb-2">
                  <p><strong>{p.medicationName}</strong></p>
                  <p>Dosage: {p.dosage}</p>
                  <p>Instructions: {p.instructions}</p>
                </div>
              ))}
              {canEditMedical && (
                <>
                  <div className="flex flex-col gap-1">
                    <Input placeholder="Medication" value={newPrescription.medicationName} onChange={(e) => setNewPrescription({ ...newPrescription, medicationName: e.target.value })}/>
                    {prescriptionError.medicationName && <p className="text-red-600 text-sm">{prescriptionError.medicationName}</p>}
                    <Input placeholder="Dosage" value={newPrescription.dosage} onChange={(e) => setNewPrescription({ ...newPrescription, dosage: e.target.value })}/>
                    {prescriptionError.dosage && <p className="text-red-600 text-sm">{prescriptionError.dosage}</p>}
                    <Input placeholder="Instructions" value={newPrescription.instructions} onChange={(e) => setNewPrescription({ ...newPrescription, instructions: e.target.value })}/>
                    {prescriptionError.instructions && <p className="text-red-600 text-sm">{prescriptionError.instructions}</p>}
                  </div>
                  <Button className="mt-2" onClick={handleAddPrescription}>Prescribe</Button>
                </>
              )}
            </div>

            <Separator />

            <div>
              <h3 className="font-semibold mb-2">Addendums</h3>
              {addendums.map(a => <div key={a.id} className="border p-2 rounded mb-2">{a.text}</div>)}
              {canAddAddendum && (
                <>
                  <Textarea value={newAddendum} onChange={(e) => setNewAddendum(e.target.value)} placeholder="Add addendum"/>
                  {addendumError && <p className="text-red-600">{addendumError}</p>}
                  <Button className="mt-2" onClick={handleAddAddendum}>Add Addendum</Button>
                </>
              )}
            </div>

            <Separator />

            <div className="flex gap-4">
              {showLock && <Button variant="secondary" onClick={handleLockEncounter} disabled={!canFinalizeOrLock} title={!canFinalizeOrLock ? "Add at least one diagnosis before locking" : ""}>Lock Encounter</Button>}
              {showFinalize && <Button onClick={handleFinalizeEncounter} disabled={!canFinalizeOrLock} title={!canFinalizeOrLock ? "Add at least one diagnosis before finalizing" : ""}>Finalize Encounter</Button>}
            </div>

          </CardContent>
        </Card>
      )}
    </div>
  )
}