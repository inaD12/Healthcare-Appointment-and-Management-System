"use client"

import { useEffect, useRef, useState } from "react"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Badge } from "@/components/ui/badge"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"

import { Speciality } from "@/features/doctors/types/doctorsTypes"
import { doctorService } from "../services/doctorService"

type Props = {
  specialities: string[]
  onAdd: (value: string) => Promise<void>
  onRemove: (value: string) => Promise<void>
}

export function DoctorSpecialitiesCard({
  specialities,
  onAdd,
  onRemove,
}: Props) {
  const [query, setQuery] = useState("")
  const [results, setResults] = useState<Speciality[]>([])
  const [page, setPage] = useState(1)
  const [hasNext, setHasNext] = useState(false)

  const [open, setOpen] = useState(false)
  const [loading, setLoading] = useState(false)

  const [localSpecialities, setLocalSpecialities] = useState<string[]>(specialities)
  const [removing, setRemoving] = useState<string | null>(null)

  const containerRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    setLocalSpecialities(specialities)
  }, [specialities])

  useEffect(() => {
    const timeout = setTimeout(() => {
      fetchSpecialities(1, true)
    }, 250)

    return () => clearTimeout(timeout)
  }, [query])

  const fetchSpecialities = async (pageToLoad = 1, reset = false) => {
    setLoading(true)

    const res = await doctorService.getAllSpecialities({
      name: query,
      description: "",
      sortOrder: "ASC",
      sortPropertyName: "Name",
      page: pageToLoad,
      pageSize: 10,
    })

    const data = res.data.data

    const filtered = data.items.filter(
      (s) => !localSpecialities.includes(s.name)
    )

    setResults((prev) =>
      reset ? filtered : [...prev, ...filtered]
    )

    setHasNext(data.hasNextPage)
    setPage(pageToLoad)
    setLoading(false)
  }

  const loadMore = () => {
    if (!hasNext || loading) return
    fetchSpecialities(page + 1)
  }

  const handleSelect = (item: Speciality) => {
    setQuery(item.name)
    setOpen(false)
  }

  const handleAdd = async () => {
    const match = results.find(
      (s) => s.name.toLowerCase() === query.toLowerCase()
    )

    if (!match) return

    setLocalSpecialities((prev) => [...prev, match.name])

    try {
      await onAdd(match.name)
    } catch {
      setLocalSpecialities((prev) =>
        prev.filter((s) => s !== match.name)
      )
    }

    setQuery("")
    setResults([])
  }

  const handleRemove = async (value: string) => {
    setRemoving(value)

    setLocalSpecialities((prev) =>
      prev.filter((s) => s !== value)
    )

    try {
      await onRemove(value)
    } catch {
      setLocalSpecialities((prev) => [...prev, value])
    }

    setRemoving(null)
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Specialities</CardTitle>
      </CardHeader>

      <CardContent className="space-y-4">

        <div className="flex flex-wrap gap-2">
          {localSpecialities.map((s) => (
            <Badge
              key={s}
              variant="outline"
              className={cn(
                "flex items-center gap-2 transition-all duration-200",
                removing === s && "opacity-0 scale-90"
              )}
            >
              {s}

              <button
                onClick={() => handleRemove(s)}
                className="text-xs text-red-500 hover:text-red-600"
              >
                ×
              </button>
            </Badge>
          ))}
        </div>

        <div ref={containerRef} className="relative">
          <Input
            placeholder="Search specialities..."
            value={query}
            onChange={(e) => {
              setQuery(e.target.value)
              setOpen(true)
            }}
            onFocus={() => setOpen(true)}
          />

          {open && (
            <div
              onScroll={(e) => {
                const el = e.currentTarget
                if (
                  el.scrollTop + el.clientHeight >=
                  el.scrollHeight - 20
                ) {
                  loadMore()
                }
              }}
              className="absolute z-10 mt-2 w-full max-h-60 overflow-y-auto rounded-md border bg-white shadow"
            >
              {results.map((item) => (
                <button
                  key={item.id}
                  type="button"
                  onClick={() => handleSelect(item)}
                  className="w-full text-left px-3 py-2 text-sm hover:bg-muted"
                >
                  {item.name}
                </button>
              ))}

              {loading && (
                <div className="px-3 py-2 text-sm text-muted-foreground">
                  Loading...
                </div>
              )}
            </div>
          )}
        </div>

        <Button
          className="w-full"
          disabled={
            !results.some(
              (s) => s.name.toLowerCase() === query.toLowerCase()
            )
          }
          onClick={handleAdd}
        >
          Add speciality
        </Button>

      </CardContent>
    </Card>
  )
}