"use client"

import { useState } from "react"
import {
  EncounterDetails,
  EncounterStatus,
  Note,
  Diagnosis,
  Prescription,
  Addendum,
} from "@/features/patients/types/patientTypes"

import { patientService } from "@/features/patients/services/patientService"
import { useAuth } from "@/features/auth/hooks/useAuth"

import { Input } from "@/components/ui/input"
import { Textarea } from "@/components/ui/textarea"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Separator } from "@/components/ui/separator"

interface Props {
  encounter: EncounterDetails
  encounterId: string
  updateEncounter: (patch: Partial<EncounterDetails>) => void
}

export default function EncounterCard({
  encounter,
  encounterId,
  updateEncounter,
}: Props) {
  const { isPatient, isDoctor } = useAuth()

  const [newNote, setNewNote] = useState("")
  const [newDiagnosis, setNewDiagnosis] = useState({
    icdCode: "",
    description: "",
  })
  const [newPrescription, setNewPrescription] = useState({
    name: "",
    dosage: "",
    instructions: "",
  })
  const [newAddendum, setNewAddendum] = useState("")

  const isReadOnly =
    isPatient || encounter.status === EncounterStatus.Locked

  const canEdit =
    !isReadOnly && encounter.status === EncounterStatus.InProgress

  const canAddAddendum =
    !isReadOnly && encounter.status === EncounterStatus.Finalized

  const canLock =
    isDoctor &&
    encounter.status !== EncounterStatus.Locked

  const canFinalize =
    isDoctor &&
    encounter.status === EncounterStatus.InProgress

  const addNote = async () => {
    const res = await patientService.addNote(encounterId, {
      note: newNote,
    })

    const note: Note = {
      id: res.data.data.id,
      text: newNote,
      createdAt: new Date().toISOString(),
    }

    updateEncounter({
      notes: [...encounter.notes, note],
    })

    setNewNote("")
  }

  const addDiagnosis = async () => {
    const res = await patientService.addDiagnosis(encounterId, newDiagnosis)

    const diagnosis: Diagnosis = {
      ...newDiagnosis,
      id: res.data.data.id,
    }

    updateEncounter({
      diagnoses: [...encounter.diagnoses, diagnosis],
    })

    setNewDiagnosis({ icdCode: "", description: "" })
  }

  const addPrescription = async () => {
    const res = await patientService.prescribeMedication(
      encounterId,
      newPrescription
    )

    const prescription: Prescription = {
      ...newPrescription,
      id: res.data.data.id,
    }

    updateEncounter({
      prescriptions: [...encounter.prescriptions, prescription],
    })

    setNewPrescription({
      name: "",
      dosage: "",
      instructions: "",
    })
  }

  const addAddendum = async () => {
    const res = await patientService.addAddendum(encounterId, {
      note: newAddendum,
    })

    const addendum: Addendum = {
      id: res.data.data.id,
      text: newAddendum,
      createdAt: new Date().toISOString(),
    }

    updateEncounter({
      addendums: [...encounter.addendums, addendum],
    })

    setNewAddendum("")
  }

  const setStatus = async (status: EncounterStatus) => {
    if (status === EncounterStatus.Locked) {
      await patientService.lockEncounter(encounterId)
    }

    if (status === EncounterStatus.Finalized) {
      await patientService.finalizeEncounter(encounterId)
    }

    updateEncounter({ status })
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Encounter</CardTitle>
      </CardHeader>

      <CardContent className="space-y-6">

        <div>
          <h3 className="font-semibold mb-2">Notes</h3>

          {encounter.notes.map((n) => (
            <p key={n.id}>• {n.text}</p>
          ))}

          {canEdit && (
            <>
              <Textarea
                value={newNote}
                onChange={(e) => setNewNote(e.target.value)}
              />
              <Button onClick={addNote} className="mt-2">
                Add Note
              </Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-2">Diagnoses</h3>

          {encounter.diagnoses.map((d) => (
            <p key={d.id}>
              {d.icdCode} — {d.description}
            </p>
          ))}

          {canEdit && (
            <>
              <Input
                placeholder="ICD Code"
                value={newDiagnosis.icdCode}
                onChange={(e) =>
                  setNewDiagnosis({
                    ...newDiagnosis,
                    icdCode: e.target.value,
                  })
                }
              />

              <Input
                placeholder="Description"
                value={newDiagnosis.description}
                onChange={(e) =>
                  setNewDiagnosis({
                    ...newDiagnosis,
                    description: e.target.value,
                  })
                }
              />

              <Button onClick={addDiagnosis} className="mt-2">
                Add Diagnosis
              </Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-2">Prescriptions</h3>

          {encounter.prescriptions.map((p) => (
            <p key={p.id}>
              {p.name} — {p.dosage}
            </p>
          ))}

          {canEdit && (
            <>
              <Input
                placeholder="Medication"
                value={newPrescription.name}
                onChange={(e) =>
                  setNewPrescription({
                    ...newPrescription,
                    name: e.target.value,
                  })
                }
              />

              <Input
                placeholder="Dosage"
                value={newPrescription.dosage}
                onChange={(e) =>
                  setNewPrescription({
                    ...newPrescription,
                    dosage: e.target.value,
                  })
                }
              />

              <Input
                placeholder="Instructions"
                value={newPrescription.instructions}
                onChange={(e) =>
                  setNewPrescription({
                    ...newPrescription,
                    instructions: e.target.value,
                  })
                }
              />

              <Button onClick={addPrescription} className="mt-2">
                Prescribe
              </Button>
            </>
          )}
        </div>

        <Separator />

        <div>
          <h3 className="font-semibold mb-2">Addendums</h3>

          {encounter.addendums.map((a) => (
            <p key={a.id}>{a.text}</p>
          ))}

          {canAddAddendum && (
            <>
              <Textarea
                value={newAddendum}
                onChange={(e) => setNewAddendum(e.target.value)}
              />
              <Button onClick={addAddendum} className="mt-2">
                Add Addendum
              </Button>
            </>
          )}
        </div>

        <Separator />

        {!isReadOnly && (
          <div className="flex gap-3">
            {canLock && (
              <Button
                variant="secondary"
                onClick={() => setStatus(EncounterStatus.Locked)}
              >
                Lock
              </Button>
            )}

            {canFinalize && (
              <Button onClick={() => setStatus(EncounterStatus.Finalized)}>
                Finalize
              </Button>
            )}
          </div>
        )}

      </CardContent>
    </Card>
  )
}