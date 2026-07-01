"use client"

import { useEffect, useState } from "react"
import { useRouter } from "next/navigation"

import {
  DoctorQueryViewModel,
  GetAllDoctorsRequest,
} from "@/features/doctors/types/doctorsTypes"


import { Card, CardContent } from "@/components/ui/card"
import { AIRecommendationCard } from "@/features/doctors/components/AIRecommendationCard"
import { DoctorCard } from "@/features/doctors/components/DoctorCard"
import { DoctorsFiltersCard } from "@/features/doctors/components/DoctorFiltersCard"
import { DoctorsHeader } from "@/features/doctors/components/DoctorsHeader"
import { DoctorsPagination } from "@/features/doctors/components/DoctorsPagination"
import { doctorService } from "@/features/doctors/services/doctorService"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

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
      const res = await doctorService.getAllDoctors({ ...filters, page })
      const data = res.data.data

      setDoctors(data.items)
      setTotalPages(Math.ceil(data.totalCount / data.pageSize))
      setFilters((prev) => ({ ...prev, page }))
    } catch (err: any) {
      if (err.response?.status === 404) {
        setDoctors([])
        setTotalPages(1)
        setError("")
      } else {
        setError("Failed to fetch doctors. Please try again.")
      }
    } finally {
      setLoading(false)
    }
  }

  async function handleRecommend() {
    setAiError("")

    try {
      const res = await doctorService.recommendSpeciality({ Symptoms })

      const specialities =
        res.data.data.specialities?.map((s) => s.name) ?? []

      setRecommendedSpecialities(specialities)

      // if (specialities.length > 0) {
      //   setFilters((prev) => ({
      //     ...prev,
      //     speciality: specialities[0],
      //     page: 1,
      //   }))

      //   fetchDoctors(1)
      // }
    } catch {
      setAiError("Something went wrong. Please try again.")
    }
  }

  useEffect(() => {
    if (auth?.authenticated) {
      fetchDoctors()
    }
  }, [auth?.authenticated])

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
        <p className="text-center text-muted-foreground py-10">
          Loading doctors...
        </p>
      ) : error ? (
        <Card className="border-red-200 bg-red-50">
          <CardContent className="py-8 text-center text-red-700">
            <p className="font-semibold">Something went wrong</p>
            <p className="text-sm mt-1">
              Failed to load doctors. Please try again.
            </p>
          </CardContent>
        </Card>
      ) : doctors.length === 0 ? (
        <Card className="border-dashed">
          <CardContent className="py-12 flex flex-col items-center text-center space-y-3">
            <h3 className="text-xl font-semibold">
              No doctors found
            </h3>

            <p className="text-muted-foreground max-w-md">
              We couldn't find any doctors matching your current filters.
              Try adjusting the doctor's name, specialty, or sorting options.
            </p>
          </CardContent>
        </Card>
      ) : (
        <div className="space-y-4">
          {doctors.map((doctor) => (
            <DoctorCard
              key={doctor.id}
              doctor={doctor}
              onClick={() => router.push(`/doctors/${doctor.userId}`)}
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