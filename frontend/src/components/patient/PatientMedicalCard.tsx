"use client"

import { useState } from "react"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { AddAllergySchema, AddChronicConditionSchema, Allergy, Condition } from "@/features/patients/types/patientTypes"

type Props = {
  patientId: string
  allergies: Allergy[]
  conditions: Condition[]

  onAddAllergy: (
    patientId: string,
    data: { Substance: string; Reaction: string }
  ) => Promise<{ id: string }>

  onRemoveAllergy: (
    patientId: string,
    data: { AllergyId: string }
  ) => Promise<void>

  onAddCondition: (
    patientId: string,
    data: { Name: string }
  ) => Promise<{ id: string }>

  onRemoveCondition: (
    patientId: string,
    data: { ConditionId: string }
  ) => Promise<void>
}

export function PatientMedicalCard({
  patientId,
  allergies: initialAllergies,
  conditions: initialConditions,
  onAddAllergy,
  onRemoveAllergy,
  onAddCondition,
  onRemoveCondition,
}: Props) {
  const [errors, setErrors] = useState<{
    substance?: string
    reaction?: string
    condition?: string
  }>({})

  const [allergies, setAllergies] = useState(initialAllergies)
  const [conditions, setConditions] = useState(initialConditions)

  const [newSubstance, setNewSubstance] = useState("")
  const [newReaction, setNewReaction] = useState("")
  const [newCondition, setNewCondition] = useState("")

  const [loading, setLoading] = useState(false)

  const handleAddAllergy = async () => {
    const result = AddAllergySchema.safeParse({
      Substance: newSubstance,
      Reaction: newReaction,
    })

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors

      setErrors({
        substance: fieldErrors.Substance?.[0],
        reaction: fieldErrors.Reaction?.[0],
      })

      return
    }

    setErrors({})
    setLoading(true)

    const res = await onAddAllergy(patientId, result.data)

    setAllergies((prev) => [
      ...prev,
      {
        id: res.id,
        substance: newSubstance,
        reaction: newReaction,
      },
    ])

    setNewSubstance("")
    setNewReaction("")
    setLoading(false)
  }

  const handleRemoveAllergy = async (id: string) => {
    setLoading(true)

    await onRemoveAllergy(patientId, { AllergyId: id })

    setAllergies((prev) => prev.filter((a) => a.id !== id))
    setLoading(false)
  }

  const handleAddCondition = async () => {
    const result = AddChronicConditionSchema.safeParse({
      Name: newCondition,
    })

    if (!result.success) {
      const fieldErrors = result.error.flatten().fieldErrors

      setErrors({
        condition: fieldErrors.Name?.[0],
      })

      return
    }

    setErrors({})
    setLoading(true)

    const res = await onAddCondition(patientId, result.data)

    setConditions((prev) => [
      ...prev,
      {
        id: res.id,
        name: newCondition,
      },
    ])

    setNewCondition("")
    setLoading(false)
  }

  const handleRemoveCondition = async (id: string) => {
    setLoading(true)

    await onRemoveCondition(patientId, { ConditionId: id })

    setConditions((prev) => prev.filter((c) => c.id !== id))
    setLoading(false)
  }

  return (
    <Card className="border rounded-xl shadow-sm">
      <CardHeader>
        <CardTitle className="text-lg">Medical Information</CardTitle>
      </CardHeader>

      <CardContent className="space-y-8">

        <section className="space-y-3">
          <h3 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
            Allergies
          </h3>

          {allergies.length === 0 && (
            <p className="text-sm text-muted-foreground">No allergies recorded</p>
          )}

          <div className="flex flex-wrap gap-2">
            {allergies.map((a) => (
              <div
                key={a.id}
                className="group flex items-center gap-2 px-3 py-1.5 rounded-full border bg-muted/40 text-sm"
              >
                <span className="font-medium">{a.substance}</span>
                <span className="text-muted-foreground text-xs">
                  ({a.reaction})
                </span>

                <button
                  onClick={() => handleRemoveAllergy(a.id)}
                  className="ml-1 opacity-0 group-hover:opacity-100 transition text-red-500"
                >
                  ✕
                </button>
              </div>
            ))}
          </div>

          <div className="flex gap-2">
            <div className="flex flex-col w-full">
              <Input
                placeholder="Substance"
                value={newSubstance}
                onChange={(e) => setNewSubstance(e.target.value)}
                disabled={loading}
              />
              {errors.substance && (
                <p className="text-red-500 text-sm mt-1">
                  {errors.substance}
                </p>
              )}
            </div>

            <div className="flex flex-col w-full">
              <Input
                placeholder="Reaction"
                value={newReaction}
                onChange={(e) => setNewReaction(e.target.value)}
                disabled={loading}
              />
              {errors.reaction && (
                <p className="text-red-500 text-sm mt-1">
                  {errors.reaction}
                </p>
              )}
            </div>

            <Button onClick={handleAddAllergy} disabled={loading}>
              Add
            </Button>
          </div>
        </section>

        <section className="space-y-3">
          <h3 className="text-sm font-semibold text-muted-foreground uppercase tracking-wide">
            Chronic Conditions
          </h3>

          {conditions.length === 0 && (
            <p className="text-sm text-muted-foreground">No conditions recorded</p>
          )}

          <div className="flex flex-wrap gap-2">
            {conditions.map((c) => (
              <div
                key={c.id}
                className="group flex items-center gap-2 px-3 py-1.5 rounded-full border bg-muted/40 text-sm"
              >
                <span className="font-medium">{c.name}</span>

                <button
                  onClick={() => handleRemoveCondition(c.id)}
                  className="ml-1 opacity-0 group-hover:opacity-100 transition text-red-500"
                >
                  ✕
                </button>
              </div>
            ))}
          </div>

          <div className="flex gap-2">
            <div className="flex flex-col w-full">
              <Input
                placeholder="Condition name"
                value={newCondition}
                onChange={(e) => setNewCondition(e.target.value)}
                disabled={loading}
              />
              {errors.condition && (
                <p className="text-red-500 text-sm mt-1">
                  {errors.condition}
                </p>
              )}
            </div>

            <Button onClick={handleAddCondition} disabled={loading}>
              Add
            </Button>
          </div>
        </section>

      </CardContent>
    </Card>
  )
}