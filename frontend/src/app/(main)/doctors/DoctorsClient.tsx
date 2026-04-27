"use client"

import { useEffect, useState } from "react"
import {
  DoctorQueryViewModel,
  GetAllDoctorsRequest,
} from "@/features/doctors/types/doctors"

import {
  getAllDoctors,
  recommendSpeciality,
} from "@/features/doctors/services/doctorService"

import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"

import {
  Card,
  CardContent,
  CardTitle,
  CardDescription,
  CardHeader,
} from "@/components/ui/card"

import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select"

import { useRouter } from "next/navigation"
import { DoctorSpecialitiesFilter } from "@/features/doctors/components/DoctorSpecialitiesFilter"

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
        setTotalPages(
          Math.ceil(data.totalCount / data.pageSize)
        )
      }

      setFilters((prev) => ({ ...prev, page }))
    } catch (err: any) {
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

  if (!auth?.authenticated) return <p>Checking authentication...</p>

  return (
    <div className="max-w-6xl mx-auto p-8 space-y-8">

      <div>
        <h1 className="text-3xl font-semibold">Doctors</h1>
        <p className="text-sm text-muted-foreground">
          Search medical specialists
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>AI Recommendation</CardTitle>
          <CardDescription>
            Describe symptoms to suggest specialities
          </CardDescription>
        </CardHeader>

        <CardContent className="flex gap-3">
          <Input
            value={Symptoms}
            onChange={(e) => setSymptoms(e.target.value)}
            placeholder="Describe symptoms..."
          />
          <Button onClick={handleRecommend}>Ask AI</Button>
        </CardContent>

        {aiError && (
          <p className="px-6 pb-4 text-sm text-red-500">
            {aiError}
          </p>
        )}

        {recommendedSpecialities.length > 0 && (
          <p className="px-6 pb-4 text-sm text-muted-foreground">
            Recommended:{" "}
            {recommendedSpecialities.join(", ")}
          </p>
        )}
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Filters</CardTitle>
        </CardHeader>

        <CardContent>
          <div className="flex flex-wrap gap-3 items-center w-full">

            <Input
              placeholder="First name"
              className="flex-1 min-w-[140px]"
              value={filters.firstName}
              onChange={(e) =>
                setFilters({
                  ...filters,
                  firstName: e.target.value,
                  page: 1,
                })
              }
            />

            <Input
              placeholder="Last name"
              className="flex-1 min-w-[140px]"
              value={filters.lastName}
              onChange={(e) =>
                setFilters({
                  ...filters,
                  lastName: e.target.value,
                  page: 1,
                })
              }
            />

            <div className="flex-[1.5] min-w-[200px]">
              <DoctorSpecialitiesFilter
                value={filters.speciality}
                onChange={(val) =>
                  setFilters((prev) => ({
                    ...prev,
                    speciality: val,
                    page: 1,
                  }))
                }
              />
            </div>

            <div className="flex-1 min-w-[140px]">
              <Select
                value={filters.sortOrder}
                onValueChange={(value) =>
                  setFilters({
                    ...filters,
                    sortOrder: value as "ASC" | "DESC",
                  })
                }
              >
                <SelectTrigger className="w-full">
                  <SelectValue placeholder="Sort" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="ASC">ASC</SelectItem>
                  <SelectItem value="DESC">DESC</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <Button
              className="shrink-0 px-6"
              onClick={() => fetchDoctors(1)}
            >
              Search
            </Button>

          </div>
        </CardContent>
      </Card>

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
            <Card
              key={doctor.id}
              onClick={() =>
                router.push(`/doctors/${doctor.userId}`)
              }
              className="cursor-pointer hover:shadow-lg hover:scale-[1.01] transition"
            >
              <CardContent className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">

                <div>
                  <CardTitle>
                    {doctor.firstName} {doctor.lastName}
                  </CardTitle>

                  <CardDescription>
                    {doctor.bio}
                  </CardDescription>

                  <p className="mt-2">
                    <b>Specialities:</b>{" "}
                    {doctor.specialities.join(", ")}
                  </p>

                  <p className="text-sm text-muted-foreground mt-2">
                    <span className="text-yellow-400">★</span>{" "}
                    {doctor.averageRating?.toFixed(1) ?? "0.0"} (
                    {doctor.ratingsCount ?? 0})
                  </p>
                </div>

              </CardContent>
            </Card>
          ))}
        </div>
      )}

      <div className="flex justify-center gap-4 items-center">
        <Button
          variant="outline"
          disabled={filters.page === 1}
          onClick={() => fetchDoctors(filters.page - 1)}
        >
          Previous
        </Button>

        <span className="text-sm text-muted-foreground">
          Page {filters.page} / {totalPages}
        </span>

        <Button
          variant="outline"
          disabled={filters.page === totalPages}
          onClick={() => fetchDoctors(filters.page + 1)}
        >
          Next
        </Button>
      </div>

    </div>
  )
}