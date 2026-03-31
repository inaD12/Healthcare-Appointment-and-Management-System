"use client"

import { useState } from "react"
import { 
  EncounterStatus, Note, Diagnosis, Prescription, Addendum,
  AddNoteSchema, AddDiagnosisSchema, PrescribeMedicationSchema, AddAddendumSchema
} from "@/features/patients/types/patientTypes"
import { patientService } from "@/features/patients/services/patientService"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"

interface EncounterCardProps {
  encounterId: string
  encounterStatus: EncounterStatus | null
  setEncounterStatus: (status: EncounterStatus) => void
  notes: Note[]
  setNotes: (notes: Note[]) => void
  diagnoses: Diagnosis[]
  setDiagnoses: (diagnoses: Diagnosis[]) => void
  prescriptions: Prescription[]
  setPrescriptions: (prescriptions: Prescription[]) => void
  addendums: Addendum[]
  setAddendums: (addendums: Addendum[]) => void
}

export default function EncounterCard({
  encounterId,
  encounterStatus,
  setEncounterStatus,
  notes,
  setNotes,
  diagnoses,
  setDiagnoses,
  prescriptions,
  setPrescriptions,
  addendums,
  setAddendums
}: EncounterCardProps) {

  const [newNote, setNewNote] = useState("")
  const [noteError, setNoteError] = useState<string | null>(null)

  const [newDiagnosis, setNewDiagnosis] = useState({ icdCode: "", description: "" })
  const [diagnosisError, setDiagnosisError] = useState<{ icdCode?: string; description?: string }>({})

  const [newPrescription, setNewPrescription] = useState({ medicationName: "", dosage: "", instructions: "" })
  const [prescriptionError, setPrescriptionError] = useState<{ medicationName?: string; dosage?: string; instructions?: string }>({})

  const [newAddendum, setNewAddendum] = useState("")
  const [addendumError, setAddendumError] = useState<string | null>(null)

  const isInProgress = encounterStatus === EncounterStatus.InProgress
  const isFinalized = encounterStatus === EncounterStatus.Finalized
  const isLocked = encounterStatus === EncounterStatus.Locked

  const canEditMedical = isInProgress
  const canAddAddendum = isFinalized
  const showFinalize = isInProgress
  const showLock = !isLocked
  const canFinalizeOrLock = diagnoses.length > 0

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

  const handleAddNote = async () => {
    try {
      AddNoteSchema.parse({ note: newNote })
      setNoteError(null)
      const res = await patientService.addNote(encounterId, { note: newNote })
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
      const res = await patientService.addDiagnosis(encounterId, newDiagnosis)
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
      const res = await patientService.prescribeMedication(encounterId, {
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
      const res = await patientService.addAddendum(encounterId, { note: newAddendum })
      setAddendums([...addendums, { id: res.data.data.id, text: newAddendum, createdAt: new Date().toISOString() }])
      setNewAddendum("")
    } catch (err: any) {
      setAddendumError(parseZodErrors(err).note || "Invalid addendum")
    }
  }

  const handleLockEncounter = async () => {
    if (!canFinalizeOrLock) return
    await patientService.lockEncounter(encounterId)
    setEncounterStatus(EncounterStatus.Locked)
  }

  const handleFinalizeEncounter = async () => {
    if (!canFinalizeOrLock) return
    await patientService.finalizeEncounter(encounterId)
    setEncounterStatus(EncounterStatus.Finalized)
  }

  return (
    <Card className="shadow-sm">
      <CardHeader>
        <CardTitle>Encounter</CardTitle>
      </CardHeader>
      <CardContent className="space-y-6">

        <div>
          <h3 className="font-semibold mb-3">Notes</h3>
          {notes.map(n => (
            <div key={n.id} className="border rounded-md p-3 mb-2 bg-muted/20">{n.text}</div>
          ))}
          {canEditMedical && (
            <>
              <Textarea value={newNote} onChange={e => setNewNote(e.target.value)} placeholder="Add clinical note"/>
              {noteError && <p className="text-red-600">{noteError}</p>}
              <Button className="mt-2" onClick={handleAddNote}>Add Note</Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-3">Diagnoses</h3>
          {diagnoses.map(d => (
            <div key={d.id} className="border rounded-md p-3 mb-2 bg-muted/20">
              <strong>{d.icdCode}</strong> — {d.description}
            </div>
          ))}
          {canEditMedical && (
            <>
              <div className="flex flex-col gap-2">
                <Input placeholder="ICD Code" value={newDiagnosis.icdCode} onChange={e => setNewDiagnosis({ ...newDiagnosis, icdCode: e.target.value })}/>
                {diagnosisError.icdCode && <p className="text-red-600 text-sm">{diagnosisError.icdCode}</p>}
                <Input placeholder="Description" value={newDiagnosis.description} onChange={e => setNewDiagnosis({ ...newDiagnosis, description: e.target.value })}/>
                {diagnosisError.description && <p className="text-red-600 text-sm">{diagnosisError.description}</p>}
              </div>
              <Button className="mt-2" onClick={handleAddDiagnosis}>Add Diagnosis</Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-3">Prescriptions</h3>
          {prescriptions.map(p => (
            <div key={p.id} className="border rounded-md p-3 mb-2 bg-muted/20">
              <p className="font-medium">{p.medicationName}</p>
              <p className="text-sm">Dosage: {p.dosage}</p>
              <p className="text-sm">Instructions: {p.instructions}</p>
            </div>
          ))}
          {canEditMedical && (
            <>
              <div className="flex flex-col gap-2">
                <Input placeholder="Medication" value={newPrescription.medicationName} onChange={e => setNewPrescription({ ...newPrescription, medicationName: e.target.value })}/>
                {prescriptionError.medicationName && <p className="text-red-600 text-sm">{prescriptionError.medicationName}</p>}
                <Input placeholder="Dosage" value={newPrescription.dosage} onChange={e => setNewPrescription({ ...newPrescription, dosage: e.target.value })}/>
                {prescriptionError.dosage && <p className="text-red-600 text-sm">{prescriptionError.dosage}</p>}
                <Input placeholder="Instructions" value={newPrescription.instructions} onChange={e => setNewPrescription({ ...newPrescription, instructions: e.target.value })}/>
                {prescriptionError.instructions && <p className="text-red-600 text-sm">{prescriptionError.instructions}</p>}
              </div>
              <Button className="mt-2" onClick={handleAddPrescription}>Prescribe</Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-3">Addendums</h3>
          {addendums.map(a => (
            <div key={a.id} className="border rounded-md p-3 mb-2 bg-muted/20">{a.text}</div>
          ))}
          {canAddAddendum && (
            <>
              <Textarea value={newAddendum} onChange={e => setNewAddendum(e.target.value)} placeholder="Add addendum"/>
              {addendumError && <p className="text-red-600">{addendumError}</p>}
              <Button className="mt-2" onClick={handleAddAddendum}>Add Addendum</Button>
            </>
          )}
        </div>

        <Separator />

        <div className="flex gap-4">
          {showLock && <Button variant="secondary" onClick={handleLockEncounter} disabled={!canFinalizeOrLock}>Lock Encounter</Button>}
          {showFinalize && <Button onClick={handleFinalizeEncounter} disabled={!canFinalizeOrLock}>Finalize Encounter</Button>}
        </div>

      </CardContent>
    </Card>
  )
}