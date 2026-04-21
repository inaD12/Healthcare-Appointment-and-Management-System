"use client"

import { useState } from "react"
import {
  AddAddendumSchema,
  AddDiagnosisSchema,
  AddNoteSchema,
  EncounterDetails,
  EncounterStatus,
  PrescribeMedicationSchema,
} from "@/features/patients/types/patientTypes"

import {
  NoteIcon,
  DiagnosisIcon,
  PrescriptionIcon,
  AddendumIcon,
  LockIcon,
  CheckIcon,
  PlusIcon,
} from "./icons"

import { Section } from "./Section"
import { EmptyState } from "./EmptyState"
import { FieldError } from "./FieldError"

import { patientService } from "@/features/patients/services/patientService"
import { useAuth } from "@/features/auth/hooks/useAuth"
import { ZodSchema } from "zod"
import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"

interface Props {
  encounter: EncounterDetails
  encounterId: string
  updateEncounter: (patch: Partial<EncounterDetails>) => void
}


type FieldErrors<T> = Partial<Record<keyof T, string>>

function validate<T extends object>(schema: ZodSchema<T>, data: T): FieldErrors<T> {
  const result = schema.safeParse(data)
  if (result.success) return {}
  return Object.fromEntries(
    result.error.issues.map((issue) => [issue.path[0], issue.message])
  ) as FieldErrors<T>
}


const statusConfig: Record<EncounterStatus, { label: string; color: string; dot: string }> = {
  [EncounterStatus.InProgress]: {
    label: "In Progress",
    color: "bg-amber-50 text-amber-700 border border-amber-200",
    dot: "bg-amber-400",
  },
  [EncounterStatus.Finalized]: {
    label: "Finalized",
    color: "bg-emerald-50 text-emerald-700 border border-emerald-200",
    dot: "bg-emerald-400",
  },
  [EncounterStatus.Locked]: {
    label: "Locked",
    color: "bg-slate-100 text-slate-600 border border-slate-200",
    dot: "bg-slate-400",
  },
}


function FieldWrap({ children, error }: { children: React.ReactNode; error?: string }) {
  return (
    <div>
      <div className={error
        ? "[&_input]:border-red-300 [&_input]:focus:border-red-400 [&_textarea]:border-red-300 [&_textarea]:focus:border-red-400"
        : ""}>
        {children}
      </div>
      <FieldError message={error} />
    </div>
  )
}


export default function EncounterCard({ encounter, encounterId, updateEncounter }: Props) {
  const { isPatient, isDoctor, isAdmin } = useAuth()

  const [newNote, setNewNote] = useState("")
  const [newDiagnosis, setNewDiagnosis] = useState({ icdCode: "", description: "" })
  const [newPrescription, setNewPrescription] = useState({ name: "", dosage: "", instructions: "" })
  const [newAddendum, setNewAddendum] = useState("")

  const [noteErrors, setNoteErrors] = useState<FieldErrors<{ note: string }>>({})
  const [diagnosisErrors, setDiagnosisErrors] = useState<FieldErrors<{ icdCode: string; description: string }>>({})
  const [prescriptionErrors, setPrescriptionErrors] = useState<FieldErrors<{ name: string; dosage: string; instructions: string }>>({})
  const [addendumErrors, setAddendumErrors] = useState<FieldErrors<{ note: string }>>({})

  const [loading, setLoading] = useState<string | null>(null)

  const isReadOnly = isPatient || isAdmin || encounter.status === EncounterStatus.Locked
  const canEdit = !isReadOnly && encounter.status === EncounterStatus.InProgress
  const canAddAddendum = isDoctor && encounter.status === EncounterStatus.Finalized
  const canLock = isDoctor && encounter.status !== EncounterStatus.Locked
  const canFinalize = isDoctor && encounter.status === EncounterStatus.InProgress
  const status = statusConfig[encounter.status]

  const withLoading = async (key: string, fn: () => Promise<void>) => {
    setLoading(key)
    try { await fn() } finally { setLoading(null) }
  }


  const addNote = async () => {
    const errors = validate(AddNoteSchema, { note: newNote })
    setNoteErrors(errors)
    if (Object.keys(errors).length > 0) return
    await withLoading("note", async () => {
      const res = await patientService.addNote(encounterId, { note: newNote })
      updateEncounter({ notes: [...encounter.notes, { id: res.data.data.id, text: newNote, createdAt: new Date().toISOString() }] })
      setNewNote("")
      setNoteErrors({})
    })
  }

  const addDiagnosis = async () => {
    const errors = validate(AddDiagnosisSchema, newDiagnosis)
    setDiagnosisErrors(errors)
    if (Object.keys(errors).length > 0) return
    await withLoading("diagnosis", async () => {
      const res = await patientService.addDiagnosis(encounterId, newDiagnosis)
      updateEncounter({ diagnoses: [...encounter.diagnoses, { ...newDiagnosis, id: res.data.data.id }] })
      setNewDiagnosis({ icdCode: "", description: "" })
      setDiagnosisErrors({})
    })
  }

  const addPrescription = async () => {
    const errors = validate(PrescribeMedicationSchema, newPrescription)
    setPrescriptionErrors(errors)
    if (Object.keys(errors).length > 0) return
    await withLoading("prescription", async () => {
      const res = await patientService.prescribeMedication(encounterId, newPrescription)
      updateEncounter({ prescriptions: [...encounter.prescriptions, { ...newPrescription, id: res.data.data.id }] })
      setNewPrescription({ name: "", dosage: "", instructions: "" })
      setPrescriptionErrors({})
    })
  }

  const addAddendum = async () => {
    const errors = validate(AddAddendumSchema, { note: newAddendum })
    setAddendumErrors(errors)
    if (Object.keys(errors).length > 0) return
    await withLoading("addendum", async () => {
      const res = await patientService.addAddendum(encounterId, { note: newAddendum })
      updateEncounter({ addendums: [...encounter.addendums, { id: res.data.data.id, text: newAddendum, createdAt: new Date().toISOString() }] })
      setNewAddendum("")
      setAddendumErrors({})
    })
  }

  const setStatus = async (status: EncounterStatus) => {
    await withLoading("status", async () => {
      if (status === EncounterStatus.Locked) await patientService.lockEncounter(encounterId)
      if (status === EncounterStatus.Finalized) await patientService.finalizeEncounter(encounterId)
      updateEncounter({ status })
    })
  }


  return (
    <Card className="border-0 shadow-sm ring-1 ring-slate-200 overflow-hidden bg-white">
      <CardHeader className="px-6 py-5 bg-white border-b border-slate-100">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-1.5 h-6 rounded-full bg-blue-500" />
            <CardTitle className="text-base font-semibold text-slate-900 tracking-tight">
              Encounter Record
            </CardTitle>
          </div>
          <span className={`inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full text-xs font-medium ${status.color}`}>
            <span className={`w-1.5 h-1.5 rounded-full ${status.dot}`} />
            {status.label}
          </span>
        </div>
      </CardHeader>

      <CardContent className="px-6 py-6 space-y-8">

        <Section icon={<NoteIcon />} title="Notes" count={encounter.notes.length}>
          {encounter.notes.length === 0 && !canEdit && <EmptyState label="notes" />}
          {encounter.notes.length > 0 && (
            <div className="space-y-2 mb-4">
              {encounter.notes.map((n) => (
                <div key={n.id} className="flex gap-3 p-3 rounded-lg bg-slate-50 border border-slate-100">
                  <span className="w-1 h-1 rounded-full bg-blue-400 shrink-0 mt-2" />
                  <p className="text-sm text-slate-700 leading-relaxed">{n.text}</p>
                </div>
              ))}
            </div>
          )}
          {canEdit && (
            <div className="space-y-2">
              <FieldWrap error={noteErrors.note}>
                <Textarea
                  value={newNote}
                  onChange={(e) => { setNewNote(e.target.value); if (noteErrors.note) setNoteErrors({}) }}
                  placeholder="Enter clinical note..."
                  rows={3}
                  className="text-sm resize-none border-slate-200 focus:border-blue-400 focus:ring-blue-100 rounded-lg placeholder:text-slate-400"
                />
              </FieldWrap>
              <div className="flex justify-end">
                <Button onClick={addNote} disabled={loading === "note"} size="sm"
                  className="gap-1.5 bg-blue-600 hover:bg-blue-700 text-white text-xs font-medium px-3.5">
                  <PlusIcon /> Add Note
                </Button>
              </div>
            </div>
          )}
        </Section>

        <div className="h-px bg-slate-100" />

        <Section icon={<DiagnosisIcon />} title="Diagnoses" count={encounter.diagnoses.length}>
          {encounter.diagnoses.length === 0 && !canEdit && <EmptyState label="diagnoses" />}
          {encounter.diagnoses.length > 0 && (
            <div className="space-y-2 mb-4">
              {encounter.diagnoses.map((d) => (
                <div key={d.id} className="flex items-start gap-3 p-3 rounded-lg bg-slate-50 border border-slate-100">
                  <span className="shrink-0 px-1.5 py-0.5 rounded bg-blue-100 text-blue-700 text-xs font-mono font-semibold">
                    {d.icdCode}
                  </span>
                  <p className="text-sm text-slate-700 leading-relaxed">{d.description}</p>
                </div>
              ))}
            </div>
          )}
          {canEdit && (
            <div className="space-y-2">
              <div className="grid grid-cols-[140px_1fr] gap-2 items-start">
                <FieldWrap error={diagnosisErrors.icdCode}>
                  <Input
                    placeholder="ICD-10 Code"
                    value={newDiagnosis.icdCode}
                    onChange={(e) => {
                      setNewDiagnosis({ ...newDiagnosis, icdCode: e.target.value })
                      if (diagnosisErrors.icdCode) setDiagnosisErrors((p) => ({ ...p, icdCode: undefined }))
                    }}
                    className="text-sm border-slate-200 focus:border-blue-400 focus:ring-blue-100 font-mono placeholder:font-sans placeholder:text-slate-400"
                  />
                </FieldWrap>
                <FieldWrap error={diagnosisErrors.description}>
                  <Input
                    placeholder="Description"
                    value={newDiagnosis.description}
                    onChange={(e) => {
                      setNewDiagnosis({ ...newDiagnosis, description: e.target.value })
                      if (diagnosisErrors.description) setDiagnosisErrors((p) => ({ ...p, description: undefined }))
                    }}
                    className="text-sm border-slate-200 focus:border-blue-400 focus:ring-blue-100 placeholder:text-slate-400"
                  />
                </FieldWrap>
              </div>
              <div className="flex justify-end">
                <Button onClick={addDiagnosis} disabled={loading === "diagnosis"} size="sm"
                  className="gap-1.5 bg-blue-600 hover:bg-blue-700 text-white text-xs font-medium px-3.5">
                  <PlusIcon /> Add Diagnosis
                </Button>
              </div>
            </div>
          )}
        </Section>

        <div className="h-px bg-slate-100" />

        <Section icon={<PrescriptionIcon />} title="Prescriptions" count={encounter.prescriptions.length}>
          {encounter.prescriptions.length === 0 && !canEdit && <EmptyState label="prescriptions" />}
          {encounter.prescriptions.length > 0 && (
            <div className="space-y-2 mb-4">
              {encounter.prescriptions.map((p) => (
                <div key={p.id} className="p-3 rounded-lg bg-slate-50 border border-slate-100">
                  <div className="flex items-center justify-between mb-1">
                    <span className="text-sm font-semibold text-slate-800">{p.name}</span>
                    <span className="text-xs font-medium text-slate-500 bg-white border border-slate-200 px-2 py-0.5 rounded-full">
                      {p.dosage}
                    </span>
                  </div>
                  {p.instructions && <p className="text-xs text-slate-500 leading-relaxed">{p.instructions}</p>}
                </div>
              ))}
            </div>
          )}
          {canEdit && (
            <div className="space-y-2">
              <div className="grid grid-cols-2 gap-2 items-start">
                <FieldWrap error={prescriptionErrors.name}>
                  <Input
                    placeholder="Medication name"
                    value={newPrescription.name}
                    onChange={(e) => {
                      setNewPrescription({ ...newPrescription, name: e.target.value })
                      if (prescriptionErrors.name) setPrescriptionErrors((p) => ({ ...p, name: undefined }))
                    }}
                    className="text-sm border-slate-200 focus:border-blue-400 focus:ring-blue-100 placeholder:text-slate-400"
                  />
                </FieldWrap>
                <FieldWrap error={prescriptionErrors.dosage}>
                  <Input
                    placeholder="Dosage (e.g. 500mg)"
                    value={newPrescription.dosage}
                    onChange={(e) => {
                      setNewPrescription({ ...newPrescription, dosage: e.target.value })
                      if (prescriptionErrors.dosage) setPrescriptionErrors((p) => ({ ...p, dosage: undefined }))
                    }}
                    className="text-sm border-slate-200 focus:border-blue-400 focus:ring-blue-100 placeholder:text-slate-400"
                  />
                </FieldWrap>
              </div>
              <FieldWrap error={prescriptionErrors.instructions}>
                <Input
                  placeholder="Instructions (e.g. Take twice daily with food)"
                  value={newPrescription.instructions}
                  onChange={(e) => {
                    setNewPrescription({ ...newPrescription, instructions: e.target.value })
                    if (prescriptionErrors.instructions) setPrescriptionErrors((p) => ({ ...p, instructions: undefined }))
                  }}
                  className="text-sm border-slate-200 focus:border-blue-400 focus:ring-blue-100 placeholder:text-slate-400"
                />
              </FieldWrap>
              <div className="flex justify-end">
                <Button onClick={addPrescription} disabled={loading === "prescription"} size="sm"
                  className="gap-1.5 bg-blue-600 hover:bg-blue-700 text-white text-xs font-medium px-3.5">
                  <PlusIcon /> Prescribe
                </Button>
              </div>
            </div>
          )}
        </Section>

        {(encounter.addendums.length > 0 || canAddAddendum) && (
          <>
            <div className="h-px bg-slate-100" />
            <Section icon={<AddendumIcon />} title="Addendums" count={encounter.addendums.length}>
              {encounter.addendums.length === 0 && !canAddAddendum && <EmptyState label="addendums" />}
              {encounter.addendums.length > 0 && (
                <div className="space-y-2 mb-4">
                  {encounter.addendums.map((a) => (
                    <div key={a.id} className="flex gap-3 p-3 rounded-lg bg-amber-50 border border-amber-100">
                      <span className="text-amber-500 shrink-0 mt-0.5"><AddendumIcon /></span>
                      <p className="text-sm text-slate-700 leading-relaxed">{a.text}</p>
                    </div>
                  ))}
                </div>
              )}
              {canAddAddendum && (
                <div className="space-y-2">
                  <FieldWrap error={addendumErrors.note}>
                    <Textarea
                      value={newAddendum}
                      onChange={(e) => { setNewAddendum(e.target.value); if (addendumErrors.note) setAddendumErrors({}) }}
                      placeholder="Enter addendum to finalized encounter..."
                      rows={3}
                      className="text-sm resize-none border-amber-200 focus:border-amber-400 focus:ring-amber-100 rounded-lg placeholder:text-slate-400"
                    />
                  </FieldWrap>
                  <div className="flex justify-end">
                    <Button onClick={addAddendum} disabled={loading === "addendum"} size="sm" variant="outline"
                      className="gap-1.5 border-amber-300 text-amber-700 hover:bg-amber-50 text-xs font-medium px-3.5">
                      <PlusIcon /> Add Addendum
                    </Button>
                  </div>
                </div>
              )}
            </Section>
          </>
        )}

        {!isReadOnly && (canLock || canFinalize) && (
          <>
            <div className="h-px bg-slate-100" />
            <div className="flex items-center justify-between pt-1">
              <p className="text-xs text-slate-400">
                {canFinalize ? "Ready to close this encounter?" : "Lock to prevent further edits."}
              </p>
              <div className="flex gap-2">
                {canLock && (
                  <Button variant="outline" size="sm" onClick={() => setStatus(EncounterStatus.Locked)}
                    disabled={loading === "status"}
                    className="gap-1.5 text-xs border-slate-300 text-slate-600 hover:bg-slate-50 font-medium">
                    <LockIcon /> Lock
                  </Button>
                )}
                {canFinalize && (
                  <Button size="sm" onClick={() => setStatus(EncounterStatus.Finalized)}
                    disabled={loading === "status"}
                    className="gap-1.5 text-xs bg-emerald-600 hover:bg-emerald-700 text-white font-medium">
                    <CheckIcon /> Finalize Encounter
                  </Button>
                )}
              </div>
            </div>
          </>
        )}

      </CardContent>
    </Card>
  )
}