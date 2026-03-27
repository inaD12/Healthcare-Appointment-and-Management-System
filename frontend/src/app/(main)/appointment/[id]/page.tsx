"use client"

import { useEffect, useState } from "react"
import { useParams } from "next/navigation"
import {
  AppointmentByIdResponse,
  AppointmentStatus,
  EncounterDetails
} from "@/features/patients/types/patientTypes"
import { getAppointmentWithEncounters } from "@/features/patients/services/patientService"
import { addRating, getRatingByAppointment } from "@/features/ratings/services/ratingService"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"

export default function AppointmentPage() {
  const params = useParams()
  const { id } = params as { id: string }

  const [appointment, setAppointment] = useState<AppointmentByIdResponse | null>(null)
  const [rating, setRating] = useState<RatingQueryViewModel | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [score, setScore] = useState<number>(5)
  const [comment, setComment] = useState<string>("")
  const [ratingLoading, setRatingLoading] = useState(false)
  const [ratingSuccess, setRatingSuccess] = useState(false)

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
              console.error(err)
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

  const handleSubmitRating = async () => {
    if (!app) return

    try {
      setRatingLoading(true)

      await addRating({
        AppointmentId: app.id,
        Score: score,
        Comment: comment
      })

      setRatingSuccess(true)
      setRating({
        id: "temp",
        doctorId: app.doctorId,
        patientId: app.patientId,
        appointmentId: app.id,
        score: score,
        createdAt: new Date().toISOString(),
        comment: comment
      })
    } catch (err) {
      console.error(err)
      alert("Failed to submit rating")
    } finally {
      setRatingLoading(false)
    }
  }

  if (loading) return <p>Loading...</p>
  if (error) return <p className="text-red-600">{error}</p>
  if (!appointment || appointment.appointmentById.length === 0) return <p>No appointment found</p>

  const app = appointment.appointmentById[0]

const encounter: EncounterDetails = app.encounterDetails && app.encounterDetails.length > 0
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
    <div className="max-w-4xl mx-auto p-4">
      <h1 className="text-2xl font-bold mb-4">Appointment Details</h1>

      <div className="border rounded p-4 mb-6 shadow-sm">
        <p><strong>Doctor:</strong> {app.doctorName}</p>
        <p><strong>Status:</strong> {app.status}</p>
        <p><strong>Start:</strong> {new Date(app.start).toLocaleString()}</p>
        <p><strong>End:</strong> {new Date(app.end).toLocaleString()}</p>
      </div>

      {rating && (
        <div className="mt-8 rounded-2xl border bg-gradient-to-br from-green-50 to-emerald-50 p-6 shadow-lg">
          
          <h2 className="text-xl font-bold text-gray-800 mb-4">
            Your Rating
          </h2>

          <div className="flex text-3xl text-yellow-400 mb-3">
            {[1,2,3,4,5].map((star) => (
              <span key={star}>
                {star <= rating.score ? "★" : "☆"}
              </span>
            ))}
          </div>

          {rating.comment && (
            <p className="text-gray-700 mb-2">
              "{rating.comment}"
            </p>
          )}

          <p className="text-sm text-gray-500">
            Submitted on {new Date(rating.createdAt).toLocaleString()}
          </p>

        </div>
      )}
      {app.status === AppointmentStatus.Completed && !rating && !ratingSuccess && (
        <div className="mt-8 rounded-2xl border bg-gradient-to-br from-blue-50 to-indigo-50 p-6 shadow-lg transition hover:shadow-xl">
          
          <h2 className="text-xl font-bold text-gray-800 mb-4">
            Rate your appointment
          </h2>

          <p className="text-gray-600 mb-4">
            How was your experience with <span className="font-semibold">{app.doctorName}</span>?
          </p>

          {/* Star Rating */}
          <div className="flex gap-2 mb-4">
            {[1,2,3,4,5].map((star) => (
              <button
                key={star}
                onClick={() => setScore(star)}
                className={`text-3xl transition transform hover:scale-125 ${
                  star <= score ? "text-yellow-400" : "text-gray-300"
                }`}
              >
                ★
              </button>
            ))}
          </div>

          {/* Score text */}
          <p className="text-sm text-gray-500 mb-4">
            {score === 1 && "Very poor experience"}
            {score === 2 && "Not great"}
            {score === 3 && "Average"}
            {score === 4 && "Good experience"}
            {score === 5 && "Excellent care"}
          </p>

          {/* Comment */}
          <textarea
            value={comment}
            onChange={(e) => setComment(e.target.value)}
            placeholder="Leave optional feedback..."
            className="w-full rounded-lg border p-3 mb-4 focus:outline-none focus:ring-2 focus:ring-blue-400"
          />

          {/* Submit */}
          <button
            onClick={handleSubmitRating}
            disabled={ratingLoading}
            className="w-full rounded-lg bg-blue-600 text-white py-3 font-semibold transition hover:bg-blue-700 active:scale-95"
          >
            {ratingLoading ? "Submitting..." : "Submit Rating"}
          </button>

        </div>
      )}

      <h2 className="text-xl font-semibold mb-2">Encounter</h2>

      {!encounter && (
        <div className="rounded p-4 bg-gray-50 text-gray-500">
          No encounter has been created for this appointment yet.
        </div>
      )}

      {encounter && (
        <div className="border rounded p-4 shadow-sm bg-white">

          <p><strong>Status:</strong> {encounter.status}</p>
          <p><strong>Started At:</strong> {new Date(encounter.startedAt).toLocaleString()}</p>
          <p><strong>Finalized At:</strong> {encounter.finalizedAt ? new Date(encounter.finalizedAt).toLocaleString() : "N/A"}</p>

          <div className="mt-4">
            <h3 className="font-semibold">Notes</h3>
            {encounter.notes.length === 0 ? (
              <p className="text-gray-500">No notes</p>
            ) : (
              <ul className="list-disc list-inside">
                {encounter.notes.map(note => (
                  <li key={note.id}>
                    {note.text}{" "}
                    <span className="text-gray-500">
                      ({new Date(note.createdAt).toLocaleString()})
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </div>

          <div className="mt-4">
            <h3 className="font-semibold">Diagnoses</h3>
            {encounter.diagnoses.length === 0 ? (
              <p className="text-gray-500">No diagnoses</p>
            ) : (
              <ul className="list-disc list-inside">
                {encounter.diagnoses.map(d => (
                  <li key={d.id}>
                    {d.icdCode} - {d.description}
                  </li>
                ))}
              </ul>
            )}
          </div>

          <div className="mt-4">
            <h3 className="font-semibold">Prescriptions</h3>
            {encounter.prescriptions.length === 0 ? (
              <p className="text-gray-500">No prescriptions</p>
            ) : (
              <ul className="list-disc list-inside">
                {encounter.prescriptions.map(p => (
                  <li key={p.id}>
                    {p.medicationName} - {p.dosage} ({p.instructions})
                  </li>
                ))}
              </ul>
            )}
          </div>

          <div className="mt-4">
            <h3 className="font-semibold">Addendums</h3>
            {encounter.addendums.length === 0 ? (
              <p className="text-gray-500">No addendums</p>
            ) : (
              <ul className="list-disc list-inside">
                {encounter.addendums.map(a => (
                  <li key={a.id}>
                    {a.text}{" "}
                    <span className="text-gray-500">
                      ({new Date(a.createdAt).toLocaleString()})
                    </span>
                  </li>
                ))}
              </ul>
            )}
          </div>

        </div>
      )}
    </div>
  )
}