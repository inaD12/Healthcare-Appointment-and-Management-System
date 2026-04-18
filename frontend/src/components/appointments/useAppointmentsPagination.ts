"use client"

import { useCallback, useEffect, useState } from "react"
import { Appointment } from "@/features/patients/types/patientTypes"

type PageInfo = {
  hasNextPage: boolean
  endCursor: string | null
}

type FetchFn = (after?: string) => Promise<{
  appointments: Appointment[]
  pageInfo: PageInfo
}>

export function useAppointmentsPagination(
  fetchFn: FetchFn,
  pageSize = 10
) {
  const [appointments, setAppointments] = useState<Appointment[]>([])
  const [cursor, setCursor] = useState<string | null>(null)
  const [hasNextPage, setHasNextPage] = useState(false)
  const [loading, setLoading] = useState(true)
  const [loadingMore, setLoadingMore] = useState(false)

  useEffect(() => {
    let ignore = false

    const load = async () => {
      setLoading(true)

      try {
        const res = await fetchFn(undefined)
        if (ignore) return

        setAppointments(res.appointments)
        setCursor(res.pageInfo.endCursor)
        setHasNextPage(res.pageInfo.hasNextPage)
      } finally {
        if (!ignore) setLoading(false)
      }
    }

    load()

    return () => {
      ignore = true
    }
  }, [fetchFn])

  const loadMore = useCallback(async () => {
    if (!cursor || !hasNextPage) return

    setLoadingMore(true)

    try {
      const res = await fetchFn(cursor)

      setAppointments((prev) => [...prev, ...res.appointments])
      setCursor(res.pageInfo.endCursor)
      setHasNextPage(res.pageInfo.hasNextPage)
    } finally {
      setLoadingMore(false)
    }
  }, [cursor, hasNextPage, fetchFn])

  return {
    appointments,
    cursor,
    hasNextPage,
    loading,
    loadingMore,
    loadMore,
  }
}