"use client"

import { useEffect, useRef, useState } from "react"
import { Input } from "@/components/ui/input"
import { cn } from "@/lib/utils"

import { getAllSpecialities } from "@/features/doctors/services/doctorService"
import { Speciality } from "@/features/doctors/types/doctors"

type Props = {
  value: string
  onChange: (value: string) => void
}

export function DoctorSpecialitiesFilter({ value, onChange }: Props) {
  const [query, setQuery] = useState(value ?? "")
  const [results, setResults] = useState<Speciality[]>([])
  const [open, setOpen] = useState(false)

  const [loading, setLoading] = useState(false)
  const [page, setPage] = useState(1)
  const [hasNext, setHasNext] = useState(false)

  const loadMoreRef = useRef<HTMLDivElement | null>(null)

  useEffect(() => {
    const timeout = setTimeout(() => {
      fetchSpecialities(1, true)
    }, 250)

    return () => clearTimeout(timeout)
  }, [query])

  useEffect(() => {
    if (!open) return

    const el = loadMoreRef.current
    if (!el) return

    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting && hasNext && !loading) {
        fetchSpecialities(page + 1)
      }
    })

    observer.observe(el)
    return () => observer.disconnect()
  }, [open, hasNext, loading, page])

  const fetchSpecialities = async (
    pageToLoad = 1,
    reset = false
  ) => {
    setLoading(true)

    const res = await getAllSpecialities({
      name: query,
      description: "",
      sortOrder: "ASC",
      sortPropertyName: "Name",
      page: pageToLoad,
      pageSize: 10,
    })

    const data = res.data.data

    setResults((prev) =>
      reset ? data.items : [...prev, ...data.items]
    )

    setHasNext(data.hasNextPage)
    setPage(pageToLoad)
    setLoading(false)
  }

  const handleSelect = (s: Speciality) => {
    setQuery(s.name)
    onChange(s.name)
    setOpen(false)
  }

  return (
    <div className="relative">

      <Input
        value={query}
        placeholder="Speciality"
        onChange={(e) => {
          setQuery(e.target.value)
          onChange(e.target.value)
          setOpen(true)
        }}
        onFocus={() => setOpen(true)}
        onBlur={() => setTimeout(() => setOpen(false), 150)}
      />

      {open && (
        <div className="absolute z-20 mt-2 w-full rounded-md border bg-background shadow-md overflow-hidden">

          <div className="max-h-60 overflow-y-auto">

            {results.map((item) => (
              <button
                key={item.id}
                type="button"
                onClick={() => handleSelect(item)}
                className={cn(
                  "w-full text-left px-3 py-2 text-sm hover:bg-muted transition"
                )}
              >
                {item.name}
              </button>
            ))}

            {loading && (
              <div className="px-3 py-2 text-sm text-muted-foreground">
                Loading...
              </div>
            )}

            {!loading && results.length === 0 && (
              <div className="px-3 py-2 text-sm text-muted-foreground">
                No results
              </div>
            )}

            <div ref={loadMoreRef} className="h-2" />
          </div>
        </div>
      )}

    </div>
  )
}