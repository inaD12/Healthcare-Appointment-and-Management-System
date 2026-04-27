"use client"

import { useEffect, useState } from "react"
import { useRouter } from "next/navigation"

import {
  DoctorQueryViewModel,
  GetAllDoctorsRequest,
} from "@/features/doctors/types/doctors"

import {
  getAllDoctors,
  recommendSpeciality,
} from "@/features/doctors/services/doctorService"

import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"



import { Card, CardContent } from "@/components/ui/card"
import { AIRecommendationCard } from "@/features/doctors/components/AIRecommendationCard"
import { DoctorCard } from "@/features/doctors/components/DoctorCard"
import { DoctorsFiltersCard } from "@/features/doctors/components/DoctorFiltersCard"
import { DoctorsHeader } from "@/features/doctors/components/DoctorsHeader"
import { DoctorsPagination } from "@/features/doctors/components/DoctorsPagination"

type Props = {
  initialDoctors: DoctorQueryViewModel[]
  initialTotalPages: number
}

export default function DoctorsClient({
  initialDoctors,
  initialTotalPages,
}: Props) {
  const auth = useAuthGuard()
  const router = useRouter()

  const [doctors, setDoctors] =
    useState<DoctorQueryViewModel[]>(initialDoctors)

  const [loading, setLoading] = useState(false)
  const [error, setError] = useState("")

  const [filters, setFilters] = useState<GetAllDoctorsRequest>({
    firstName: "",
    lastName: "",
    speciality: "",
    sortOrder: "ASC",
    sortPropertyName: "FirstName",
    page: 1,
    pageSize: 10,
  })

  const [totalPages, setTotalPages] =
    useState(initialTotalPages)

  const [Symptoms, setSymptoms] = useState("")
  const [recommendedSpecialities, setRecommendedSpecialities] =
    useState<string[]>([])
  const [aiError, setAiError] = useState("")

  async function fetchDoctors(page = filters.page) {
    if (!auth?.authenticated) return

    setLoading(true)
    setError("")

    try {
      const res = await getAllDoctors({ ...filters, page })
      const data = res.data.data

      if (!data.items.length) {
        setDoctors([])
        setError("No doctors found for these filters.")
      } else {
        setDoctors(data.items)
        setTotalPages(Math.ceil(data.totalCount / data.pageSize))
      }

      setFilters((prev) => ({ ...prev, page }))
    } catch {
      setError("Failed to fetch doctors.")
    } finally {
      setLoading(false)
    }
  }

  async function handleRecommend() {
    setAiError("")

    try {
      const res = await recommendSpeciality({ Symptoms })

      const specialities =
        res.data.data.specialities?.map((s) => s.name) ?? []

      setRecommendedSpecialities(specialities)

      if (specialities.length > 0) {
        setFilters((prev) => ({
          ...prev,
          speciality: specialities[0],
          page: 1,
        }))

        fetchDoctors(1)
      }
    } catch {
      setAiError("Something went wrong. Please try again.")
    }
  }

  useEffect(() => {
    fetchDoctors()
  }, [auth])

  if (!auth?.authenticated) {
    return <p>Checking authentication...</p>
  }

  return (
    <div className="max-w-6xl mx-auto p-8 space-y-8">

      <DoctorsHeader />

      <AIRecommendationCard
        symptoms={Symptoms}
        setSymptoms={setSymptoms}
        onRecommend={handleRecommend}
        aiError={aiError}
        recommendedSpecialities={recommendedSpecialities}
      />

      <DoctorsFiltersCard
        filters={filters}
        setFilters={setFilters}
        onSearch={() => fetchDoctors(1)}
      />

      {loading ? (
        <p>Loading doctors...</p>
      ) : error ? (
        <Card className="bg-red-50 border-red-200">
          <CardContent className="p-4 text-red-700">
            {error}
          </CardContent>
        </Card>
      ) : (
        <div className="space-y-4">
          {doctors.map((doctor) => (
            <DoctorCard
              key={doctor.id}
              doctor={doctor}
              onClick={() =>
                router.push(`/doctors/${doctor.userId}`)
              }
            />
          ))}
        </div>
      )}

      <DoctorsPagination
        page={filters.page}
        totalPages={totalPages}
        onPrev={() => fetchDoctors(filters.page - 1)}
        onNext={() => fetchDoctors(filters.page + 1)}
      />
    </div>
  )
}