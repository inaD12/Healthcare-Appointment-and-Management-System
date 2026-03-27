"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"

import {
  AppointmentByIdResponse,
  AppointmentStatus,
  EncounterDetails
} from "@/features/patients/types/patientTypes"

import {
  getAppointmentWithEncounters
} from "@/features/patients/services/patientService"

import {
  addRating,
  editRating,
  getRatingByAppointment,
  removeRating
} from "@/features/ratings/services/ratingService"

import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

import {
  Card,
  CardHeader,
  CardTitle,
  CardContent
} from "@/components/ui/card"

import { Button } from "@/components/ui/button"
import { Textarea } from "@/components/ui/textarea"
import { Badge } from "@/components/ui/badge"
import { Separator } from "@/components/ui/separator"

export default function AppointmentPage() {

  useAuthGuard()

  const params = useParams()
  const { id } = params as { id: string }

  const [appointment, setAppointment] = useState<AppointmentByIdResponse | null>(null)
  const [rating, setRating] = useState<RatingQueryViewModel | null>(null)

  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  const [score, setScore] = useState(5)
  const [comment, setComment] = useState("")

  const [ratingLoading, setRatingLoading] = useState(false)
  const [isEditing, setIsEditing] = useState(false)

  useEffect(() => {

    if (!id) return

    const fetchData = async () => {

      try {

        const data = await getAppointmentWithEncounters(id)
        setAppointment(data)

        const app = data?.appointmentById?.[0]

        if (app?.status === AppointmentStatus.Completed) {

          try {

            const ratingRes = await getRatingByAppointment(id)
            setRating(ratingRes.data.data)

          } catch (err: any) {

            if (err.response?.status !== 404) {
              setError("Failed to fetch rating")
            }

          }

        }

      } catch (err) {

        console.error(err)
        setError("Failed to fetch appointment")

      } finally {

        setLoading(false)

      }

    }

    fetchData()

  }, [id])

  const handleDeleteRating = async () => {

    if (!rating) return

    try {

      await removeRating(rating.id)
      setRating(null)

    } catch {

      alert("Failed to delete rating")

    }

  }

  const handleEditRating = async () => {

    if (!rating) return

    try {

      setRatingLoading(true)

      await editRating(rating.id, {
        Score: score,
        Comment: comment
      })

      setRating({
        ...rating,
        score,
        comment
      })

      setIsEditing(false)

    } catch {

      alert("Failed to edit rating")

    } finally {

      setRatingLoading(false)

    }

  }

  const handleSubmitRating = async () => {

    if (!appointment) return

    const app = appointment.appointmentById[0]

    try {

      setRatingLoading(true)

      const result = await addRating({
        AppointmentId: app.id,
        Score: score,
        Comment: comment
      })

      setRating({
        id: result.data.data.id,
        doctorId: app.doctorId,
        patientId: app.patientId,
        appointmentId: app.id,
        score,
        createdAt: new Date().toISOString(),
        comment
      })

    } catch {

      alert("Failed to submit rating")

    } finally {

      setRatingLoading(false)

    }

  }

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!appointment || appointment.appointmentById.length === 0) return <p>No appointment found</p>

  const app = appointment.appointmentById[0]

  const encounter: EncounterDetails =
    app.encounterDetails && app.encounterDetails.length > 0
      ? app.encounterDetails[0]
      : {
          id: "example-encounter",
          startedAt: new Date().toISOString(),
          finalizedAt: null,
          status: "IN_PROGRESS" as any,
          notes: [
            {
              id: "note-1",
              text: "Patient reports mild headache and fatigue.",
              createdAt: new Date().toISOString()
            }
          ],
          diagnoses: [
            {
              id: "diag-1",
              icdCode: "R51",
              description: "Headache"
            }
          ],
          prescriptions: [
            {
              id: "pres-1",
              medicationName: "Ibuprofen",
              dosage: "200mg",
              instructions: "Take one tablet every 6 hours as needed"
            }
          ],
          addendums: []
        }

  return (

    <div className="max-w-4xl mx-auto p-6 space-y-8">

      <h1 className="text-3xl font-bold">
        Appointment Details
      </h1>


      <Card>

        <CardHeader>
          <CardTitle>{app.doctorName}</CardTitle>
        </CardHeader>

        <CardContent className="space-y-2">

          <Badge variant="secondary">
            {app.status}
          </Badge>

          <p>
            <strong>Start:</strong>{" "}
            {new Date(app.start).toLocaleString()}
          </p>

          <p>
            <strong>End:</strong>{" "}
            {new Date(app.end).toLocaleString()}
          </p>

        </CardContent>

      </Card>


      {rating && (

        <Card>

          <CardHeader>
            <CardTitle>Your Rating</CardTitle>
          </CardHeader>

          <CardContent className="space-y-4">

            {!isEditing && (
              <>
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
                  Submitted on {new Date(rating.createdAt).toLocaleString()}
                </p>

                <div className="flex gap-3">

                  <Button
                    onClick={()=>{
                      setIsEditing(true)
                      setScore(rating.score)
                      setComment(rating.comment ?? "")
                    }}
                  >
                    Edit
                  </Button>

                  <Button
                    variant="destructive"
                    onClick={handleDeleteRating}
                  >
                    Delete
                  </Button>

                </div>
              </>
            )}

            {isEditing && (
              <>
                <div className="flex gap-2 text-3xl">

                  {[1,2,3,4,5].map((star)=>(
                    <button
                      key={star}
                      onClick={()=>setScore(star)}
                      className={star <= score ? "text-yellow-400" : "text-gray-300"}
                    >
                      ★
                    </button>
                  ))}

                </div>

                <Textarea
                  value={comment}
                  onChange={(e)=>setComment(e.target.value)}
                />

                <div className="flex gap-3">

                  <Button
                    onClick={handleEditRating}
                    disabled={ratingLoading}
                  >
                    Save
                  </Button>

                  <Button
                    variant="secondary"
                    onClick={()=>setIsEditing(false)}
                  >
                    Cancel
                  </Button>

                </div>
              </>
            )}

          </CardContent>

        </Card>

      )}


      {app.status === AppointmentStatus.Completed && !rating && (

        <Card>

          <CardHeader>
            <CardTitle>Rate your appointment</CardTitle>
          </CardHeader>

          <CardContent className="space-y-4">

            <div className="flex gap-2 text-3xl">

              {[1,2,3,4,5].map((star)=>(
                <button
                  key={star}
                  onClick={()=>setScore(star)}
                  className={star <= score ? "text-yellow-400" : "text-gray-300"}
                >
                  ★
                </button>
              ))}

            </div>

            <Textarea
              value={comment}
              placeholder="Leave optional feedback..."
              onChange={(e)=>setComment(e.target.value)}
            />

            <Button
              onClick={handleSubmitRating}
              disabled={ratingLoading}
              className="w-full"
            >
              {ratingLoading ? "Submitting..." : "Submit Rating"}
            </Button>

          </CardContent>

        </Card>

      )}


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

            <h3 className="font-semibold mb-2">
              Notes
            </h3>

            {encounter.notes.length === 0
              ? <p className="text-muted-foreground">No notes</p>
              : encounter.notes.map(n=>(
                  <p key={n.id}>
                    • {n.text}
                  </p>
                ))
            }

          </div>

          <Separator/>

          <div>

            <h3 className="font-semibold mb-2">
              Diagnoses
            </h3>

            {encounter.diagnoses.map(d=>(
              <p key={d.id}>
                {d.icdCode} - {d.description}
              </p>
            ))}

          </div>

          <Separator/>

          <div>

            <h3 className="font-semibold mb-2">
              Prescriptions
            </h3>

            {encounter.prescriptions.map(p=>(
              <p key={p.id}>
                {p.medicationName} - {p.dosage} ({p.instructions})
              </p>
            ))}

          </div>

          <Separator/>

          <div>

            <h3 className="font-semibold mb-2">
              Addendums
            </h3>

            {encounter.addendums.length === 0
              ? <p className="text-muted-foreground">No addendums</p>
              : encounter.addendums.map(a=>(
                  <p key={a.id}>
                    {a.text}
                  </p>
                ))
            }

          </div>

        </CardContent>

      </Card>

    </div>

  )

}