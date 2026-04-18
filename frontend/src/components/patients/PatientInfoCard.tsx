"use client"

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { PatientProfile } from "@/features/patients/types/patientTypes"

type Props = {
  patient: PatientProfile | null
}

function calculateAge(birthDate?: string) {
  if (!birthDate) return null

  const birth = new Date(birthDate)
  const today = new Date()

  let age = today.getFullYear() - birth.getFullYear()
  const m = today.getMonth() - birth.getMonth()

  if (m < 0 || (m === 0 && today.getDate() < birth.getDate())) {
    age--
  }

  return age
}

function formatDate(date?: string) {
  if (!date) return "Unknown"
  return new Date(date).toLocaleDateString(undefined, {
    year: "numeric",
    month: "short",
    day: "2-digit",
  })
}

function Tag({ children }: { children: React.ReactNode }) {
  return (
    <span className="inline-flex items-center rounded-full border bg-muted px-2.5 py-1 text-xs font-medium text-gray-700">
      {children}
    </span>
  )
}

function Field({
  label,
  value,
}: {
  label: string
  value: React.ReactNode
}) {
  return (
    <div className="space-y-2">
      <p className="text-[11px] uppercase tracking-wider text-muted-foreground">
        {label}
      </p>
      <div className="text-sm text-gray-900">{value}</div>
    </div>
  )
}

export function PatientInfoCard({ patient }: Props) {
  if (!patient) return null

  const age = calculateAge(patient.birthDate)

  return (
    <Card className="rounded-2xl border shadow-sm hover:shadow-md transition">
      <CardHeader className="pb-2">
        <CardTitle className="text-base font-semibold">
          Patient Profile
        </CardTitle>
      </CardHeader>

      <CardContent className="space-y-6">
        <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4 rounded-xl border bg-muted/30 p-5">
          
          <div className="space-y-1">
            <p className="text-lg font-semibold text-gray-900">
              {patient.fullName}
            </p>
            <p className="text-sm text-muted-foreground">
              {age !== null ? `${age} years old` : "Age unknown"}
            </p>
          </div>

          <div className="md:text-right space-y-1">
            <p className="text-[11px] uppercase tracking-wider text-muted-foreground">
              Date of Birth
            </p>
            <p className="text-sm font-medium text-gray-900">
              {formatDate(patient.birthDate)}
            </p>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          
          <div className="space-y-6">
            <Field
              label="Allergies"
              value={
                patient.allergiesList?.length ? (
                  <div className="flex flex-wrap gap-2">
                    {patient.allergiesList.map((a, i) => (
                      <Tag key={i}>{a}</Tag>
                    ))}
                  </div>
                ) : (
                  <span className="text-muted-foreground">None reported</span>
                )
              }
            />
          </div>

          <div className="space-y-6">
            <Field
              label="Conditions"
              value={
                patient.conditionsList?.length ? (
                  <div className="flex flex-wrap gap-2">
                    {patient.conditionsList.map((c, i) => (
                      <Tag key={i}>{c}</Tag>
                    ))}
                  </div>
                ) : (
                  <span className="text-muted-foreground">None reported</span>
                )
              }
            />
          </div>
        </div>
      </CardContent>
    </Card>
  )
}