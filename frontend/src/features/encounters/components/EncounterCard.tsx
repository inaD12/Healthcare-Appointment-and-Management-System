"use client"

import { EncounterDetails } from "@/features/patients/types/patientTypes"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Separator } from "@/components/ui/separator"

export function EncounterCard({ encounter }: { encounter: EncounterDetails }) {

  return (
    <Card>

      <CardHeader>
        <CardTitle>Encounter</CardTitle>
      </CardHeader>

      <CardContent className="space-y-6">

        <div className="flex items-center gap-3">
          <Badge>{encounter.status}</Badge>
          <span className="text-sm text-muted-foreground">
            Started {new Date(encounter.startedAt).toLocaleString()}
          </span>
        </div>

        <Separator/>

        <div>
          <h3 className="font-semibold mb-2">Notes</h3>
          {encounter.notes.length === 0
            ? <p className="text-muted-foreground">No notes</p>
            : encounter.notes.map(n=>(
                <p key={n.id}>• {n.text}</p>
              ))
          }
        </div>

        <Separator/>

        <div>
          <h3 className="font-semibold mb-2">Diagnoses</h3>
          {encounter.diagnoses.map(d=>(
            <p key={d.id}>{d.icdCode} - {d.description}</p>
          ))}
        </div>

        <Separator/>

        <div>
          <h3 className="font-semibold mb-2">Prescriptions</h3>
          {encounter.prescriptions.map(p=>(
            <p key={p.id}>
              {p.medicationName} - {p.dosage} ({p.instructions})
            </p>
          ))}
        </div>

      </CardContent>
    </Card>
  )
}