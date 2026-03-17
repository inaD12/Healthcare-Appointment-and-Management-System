"use client"

import { useEffect, useState } from "react"
import { DoctorQueryViewModel, GetAllDoctorsRequest } from "@/features/doctors/types/doctors"
import { getAllDoctors, recommendSpeciality } from "@/features/doctors/services/doctorService"
import { useAuthGuard } from "@/features/auth/hooks/useAuthGuard"
import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"

export default function DoctorsPage() {
  const auth = useAuthGuard()

  const [doctors, setDoctors] = useState<DoctorQueryViewModel[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState("")
  const [filters, setFilters] = useState<GetAllDoctorsRequest>({
    firstName: "",
    lastName: "",
    speciality: "",
    timeZoneId: "",
    sortOrder: "ASC",
    sortPropertyName: "FirstName",
    page: 1,
    pageSize: 10,
  })
  const [totalPages, setTotalPages] = useState(1)
  const [Symptoms, setSymptoms] = useState("")
  const [recommendedSpecialities, setRecommendedSpecialities] = useState<string[]>([])
  const [aiError, setAiError] = useState("")

  async function fetchDoctors(page = filters.page) {
    if (!auth || !auth.authenticated) return
    setLoading(true)
    setError("")
    try {
      const res = await getAllDoctors({ ...filters, page })
      if (res.data.data.items.length === 0) {
        setDoctors([])
        setError("No doctors found for these filters.")
      } else {
        setDoctors(res.data.data.items)
        setTotalPages(Math.ceil(res.data.data.totalCount / res.data.data.pageSize))
      }
      setFilters((prev) => ({ ...prev, page }))
    } catch (err: any) {
  console.error(err)

    if (err.response?.status === 404) {
      setDoctors([])
      setError("No doctors found.")
    } else if (err.response?.status === 403) {
      setError("You don’t have permission to view doctors.")
    } else if (err.response?.status === 500) {
      setError("Server error. Please try again later.")
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
      const res = await recommendSpeciality({ Symptoms })
      const specialities: string[] = res.data.data.specialities?.map(s => s.name) ?? []
      setRecommendedSpecialities(specialities)

      if (specialities.length > 0) {
        setFilters({ ...filters, speciality: specialities[0], page: 1 })
        fetchDoctors(1)
      }
    } catch (err: any) {
      if (err.response?.status === 400) {
        setAiError(err.response.data?.message || "Invalid input for AI recommendation.")
        setRecommendedSpecialities([])
      } else {
        setAiError("Something went wrong. Please try again.")
        setRecommendedSpecialities([])
        console.error(err)
      }
    }
  }

  useEffect(() => {
    fetchDoctors()
  }, [auth])

  if (!auth || !auth.authenticated) return <p>Checking authentication...</p>

  return (
    <div className="p-8 max-w-6xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold">Doctors</h1>

      <Card className="p-4">
        <CardContent className="flex items-center gap-3">
          <Input
            value={Symptoms}
            onChange={(e) => setSymptoms(e.target.value)}
            placeholder="Describe symptoms"
            className="flex-1"
          />
          <Button size="sm" onClick={handleRecommend}>
            Ask AI
          </Button>
        </CardContent>

        {aiError && (
          <p className="mt-1 text-sm text-red-600 flex items-center gap-1">
            <svg
              className="w-4 h-4"
              fill="none"
              stroke="currentColor"
              strokeWidth={2}
              viewBox="0 0 24 24"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M12 9v2m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
              />
            </svg>
            {aiError}
          </p>
        )}

        {recommendedSpecialities.length > 0 && (
          <p className="mt-1 text-sm text-gray-700">
            Recommended specialities:{" "}
            <span className="font-semibold">{recommendedSpecialities.join(", ")}</span>
          </p>
        )}
      </Card>

      <div className="flex flex-wrap gap-3 items-center">
        <Input
          placeholder="First name"
          value={filters.firstName}
          onChange={(e) => setFilters({ ...filters, firstName: e.target.value, page: 1 })}
          className="w-40"
        />
        <Input
          placeholder="Last name"
          value={filters.lastName}
          onChange={(e) => setFilters({ ...filters, lastName: e.target.value, page: 1 })}
          className="w-40"
        />
        <Input
          placeholder="Speciality"
          value={filters.speciality}
          onChange={(e) => setFilters({ ...filters, speciality: e.target.value, page: 1 })}
          className="w-48"
        />
        <Select
          value={filters.sortOrder}
          onValueChange={(value) => setFilters({ ...filters, sortOrder: value as "ASC" | "DESC" })}
        >
          <SelectTrigger className="w-40">
            <SelectValue placeholder="Sort Order" />
          </SelectTrigger>
          <SelectContent>
            <SelectItem value="ASC">Ascending</SelectItem>
            <SelectItem value="DESC">Descending</SelectItem>
          </SelectContent>
        </Select>
        <Button size="sm" onClick={() => fetchDoctors(1)}>Search</Button>
      </div>

      {loading ? (
        <p>Loading doctors...</p>
      ) : error ? (
        error === "No doctors found." ? (
          <Card className="border-gray-300 bg-gray-50">
            <CardContent className="flex items-center gap-2">
              <svg
                className="w-5 h-5 text-gray-500"
                fill="none"
                stroke="currentColor"
                strokeWidth={2}
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M9 13h6m-6 4h6M9 9h6M7 5h10a2 2 0 012 2v10a2 2 0 01-2 2H7a2 2 0 01-2-2V7a2 2 0 012-2z"
                />
              </svg>
              <span className="text-gray-600">{error}</span>
            </CardContent>
          </Card>
        ) : (
          <Card className="border-red-400 bg-red-50 transition-opacity duration-500">
            <CardContent className="flex items-center gap-2">
              <svg
                className="w-5 h-5 text-red-500"
                fill="none"
                stroke="currentColor"
                strokeWidth={2}
                viewBox="0 0 24 24"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M12 9v2m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              <span className="text-red-700 font-medium">{error}</span>
            </CardContent>
          </Card>
        )
      ) : (
        <div className="space-y-4">
          {doctors.map((doctor) => (
            <Card key={doctor.id}>
              <CardContent className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">
                <div>
                  <CardTitle>
                    {doctor.firstName} {doctor.lastName}
                  </CardTitle>
                  <CardDescription className="text-gray-600">{doctor.bio}</CardDescription>
                  <p className="mt-2">
                    <b>Specialities:</b> {doctor.specialities.join(", ")}
                  </p>
                  <p className="text-gray-500 text-sm mt-1">Timezone: {doctor.timeZoneId}</p>
                </div>
              </CardContent>
            </Card>
          ))}
        </div>
      )}

      <div className="flex justify-center items-center gap-4">
        <Button
          variant="outline"
          disabled={filters.page === 1}
          onClick={() => fetchDoctors(filters.page - 1)}
        >
          Previous
        </Button>
        <span>Page {filters.page} / {totalPages}</span>
        <Button
          variant="outline"
          disabled={filters.page === totalPages || totalPages === 0}
          onClick={() => fetchDoctors(filters.page + 1)}
        >
          Next
        </Button>
      </div>
    </div>
  )
}