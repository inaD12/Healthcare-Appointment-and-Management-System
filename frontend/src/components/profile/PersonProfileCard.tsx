"use client"

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctors"
import { PatientProfile } from "@/features/patients/types/patientTypes"
import { UserQueryResponse } from "@/features/users/types/userTypes"

type PersonProfile = {
  user?: UserQueryResponse | null
  patient?: PatientProfile | null
  doctor?: DoctorQueryViewModel | null
}


function Badge({ children }: { children: React.ReactNode }) {
  return (
    <span className="inline-flex items-center rounded-full border bg-muted px-2.5 py-1 text-xs font-medium">
      {children}
    </span>
  )
}

function StatusBadge({ ok, label }: { ok: boolean; label: string }) {
  return (
    <span
      className={`inline-flex items-center rounded-full border px-2.5 py-1 text-xs font-medium ${
        ok
          ? "bg-green-50 text-green-700 border-green-200"
          : "bg-gray-50 text-gray-600 border-gray-200"
      }`}
    >
      {label}
    </span>
  )
}

function SectionTitle({ children }: { children: React.ReactNode }) {
  return (
    <p className="text-[11px] uppercase tracking-wider text-muted-foreground">
      {children}
    </p>
  )
}


export function PersonProfileCard({
  user,
  patient,
  doctor,
}: PersonProfile) {
  const hasUser = !!user
  const hasPatient = !!patient?.id
  const hasDoctor = !!doctor?.id


  const fullName =
    user?.firstName && user?.lastName
        ? `${user.firstName} ${user.lastName}`
        : doctor?.firstName && doctor?.lastName
        ? `${doctor.firstName} ${doctor.lastName}`
        : patient?.fullName
        ? `${patient.fullName}`
        : "Unknown Person"

  const birthDate = user?.birthDate || patient?.birthDate

  const emailVerified = user?.emailVerified ?? false

  return (
    <Card className="rounded-2xl border shadow-sm hover:shadow-md transition">
      <CardHeader className="pb-2">
        <CardTitle className="text-base font-semibold">
          Profile
        </CardTitle>
      </CardHeader>

      <CardContent className="space-y-6">

        <div className="rounded-xl border bg-muted/30 p-5 space-y-3">

          <div className="flex items-start justify-between gap-4">

            <div>
              <p className="text-lg font-semibold text-gray-900">
                {fullName}
              </p>

              {user?.email && (
                <p className="text-sm text-muted-foreground">
                  {user.email}
                </p>
              )}

              {birthDate && (
                <p className="text-xs text-muted-foreground mt-1">
                  Birth date:{" "}
                  {new Date(birthDate).toLocaleDateString(undefined, {
                    year: "numeric",
                    month: "short",
                    day: "2-digit",
                  })}
                </p>
              )}
            </div>

            <div className="flex gap-2 flex-wrap justify-end">
              {user?.roles?.map((r) => (
                <Badge key={r}>{r}</Badge>
              ))}

              {hasUser && (
                <StatusBadge
                  ok={emailVerified}
                  label={emailVerified ? "Email Verified" : "Email Unverified"}
                />
              )}
            </div>

          </div>
        </div>

        {hasUser && (
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <SectionTitle>Contact</SectionTitle>
              <p className="text-sm text-gray-900">
                {user.phoneNumber || "—"}
              </p>
            </div>

            <div>
              <SectionTitle>Address</SectionTitle>
              <p className="text-sm text-gray-900">
                {user.address || "—"}
              </p>
            </div>
          </div>
        )}

        {hasPatient && (
          <div className="space-y-4 pt-2 border-t">
            <SectionTitle>Patient Information</SectionTitle>

            <div className="grid md:grid-cols-2 gap-6 text-sm">

              <div className="space-y-1">
                <p className="text-muted-foreground text-xs">Allergies</p>

                {patient.allergies?.length ? (
                  <div className="flex flex-wrap gap-2">
                    {patient.allergies.map((a) => (
                      <span
                        key={a.id}
                        className="rounded-full border px-2 py-0.5 text-xs"
                        title={a.reaction}
                      >
                        {a.substance}
                      </span>
                    ))}
                  </div>
                ) : (
                  <p className="text-gray-500">None</p>
                )}
              </div>

              <div className="space-y-1">
                <p className="text-muted-foreground text-xs">Conditions</p>

                {patient.conditions?.length ? (
                  <div className="flex flex-wrap gap-2">
                    {patient.conditions.map((c) => (
                      <span
                        key={c.id}
                        className="rounded-full border px-2 py-0.5 text-xs"
                      >
                        {c.name}
                      </span>
                    ))}
                  </div>
                ) : (
                  <p className="text-gray-500">None</p>
                )}
              </div>

            </div>
          </div>
        )}

        {hasDoctor && (
            <div className="space-y-5 pt-2 border-t">

                <div className="flex flex-wrap gap-2">
                <Badge>⭐ {doctor.averageRating.toFixed(1)} Rating</Badge>
                <Badge>{doctor.ratingsCount} Reviews</Badge>
                <Badge>{doctor.specialities.length} Specialities</Badge>
                </div>

                <div className="rounded-lg border bg-muted/20 p-4">
                <p className="text-sm text-gray-700 leading-relaxed">
                    {doctor.bio || "No biography provided."}
                </p>
                </div>

                <div className="space-y-2">
                <p className="text-[11px] uppercase tracking-wider text-muted-foreground">
                    Specialities
                </p>

                <div className="flex flex-wrap gap-2">
                    {doctor.specialities.length > 0 ? (
                    doctor.specialities.map((s, i) => (
                        <span
                        key={i}
                        className="text-xs px-2.5 py-1 rounded-full border bg-white text-gray-700"
                        >
                        {s}
                        </span>
                    ))
                    ) : (
                    <p className="text-sm text-muted-foreground">No specialities listed</p>
                    )}
                </div>
                </div>

            </div>
        )}

      </CardContent>
    </Card>
  )
}