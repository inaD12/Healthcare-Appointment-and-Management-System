"use client"

import { DashboardEncounter } from "@/features/patients/types/patientTypes"
import Link from "next/link"
import { Stethoscope } from "lucide-react"
import { Card, CardHeader, CardTitle, CardContent } from "../ui/card"

export function EncounterUpdateCard({
  encounter,
}: {
  encounter: DashboardEncounter
}) {
  const update = getEncounterUpdate(encounter)

  return (
    <Link href={`/appointment/${encounter.appointmentId}`}>
      <Card className="hover:bg-muted/40 cursor-pointer transition">
        <CardHeader className="flex flex-row items-center justify-between pb-2">
          <CardTitle className="text-sm font-medium">
            Encounter Update
          </CardTitle>
          <Stethoscope className="h-4 w-4" />
        </CardHeader>

        <CardContent>
          <div className="text-lg font-semibold">
            {new Date(encounter.updatedAt).toLocaleString(undefined, {
              dateStyle: "medium",
              timeStyle: "short",
            })}
          </div>

          {update && (
            <p className="text-sm text-muted-foreground mt-1">
              {update.type}: {update.text}
            </p>
          )}
        </CardContent>
      </Card>
    </Link>
  )
}

function getEncounterUpdate(encounter: DashboardEncounter) {
  const items = [
    ...encounter.prescriptions.flatMap(p => [
      {
        type: "Prescription added",
        text: `${p.medicationName} ${p.dosage}`,
        date: p.createdAt,
      },
      ...(p.deletedAt
        ? [{
            type: "Prescription deleted",
            text: `${p.medicationName} ${p.dosage}`,
            date: p.deletedAt,
          }]
        : []),
    ]),

    ...encounter.notes.flatMap(n => [
      {
        type: "Doctor note added",
        text: n.text,
        date: n.createdAt,
      },
      ...(n.deletedAt
        ? [{
            type: "Doctor note deleted",
            text: n.text,
            date: n.deletedAt,
          }]
        : []),
    ]),

    ...encounter.addendums.flatMap(a => [
      {
        type: "Addendum added",
        text: a.text,
        date: a.createdAt,
      },
      ...(a.deletedAt
        ? [{
            type: "Addendum deleted",
            text: a.text,
            date: a.deletedAt,
          }]
        : []),
    ]),

    ...encounter.diagnoses.flatMap(d => [
      {
        type: "Diagnosis added",
        text: `${d.icdCode} ${d.description}`,
        date: d.createdAt,
      },
      ...(d.deletedAt
        ? [{
            type: "Diagnosis deleted",
            text: `${d.icdCode} ${d.description}`,
            date: d.deletedAt,
          }]
        : []),
    ]),
  ]

  if (items.length === 0) return null

  return items.sort(
    (a, b) => new Date(b.date).getTime() - new Date(a.date).getTime()
  )[0]
}