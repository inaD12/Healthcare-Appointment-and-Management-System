"use client"

import { useEffect, useState } from "react"
import { SearchFilters } from "./SearchFilters"
import { SearchTable } from "./SearchTable"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { SearchPagination } from "./Pagination"
import { PaginatedQuery, FilterField, ColumnDef } from "./types"

interface SearchPageProps<T, Q extends PaginatedQuery> {
  title: string
  filters: FilterField[]
  columns: ColumnDef<T>[]
  queryFn: (query: Q) => Promise<any>
  defaultQuery: Q
  onRowClick?: (row: T) => void
}

export function SearchPage<T, Q extends PaginatedQuery>({
  title,
  filters,
  columns,
  queryFn,
  defaultQuery,
  onRowClick,
}: SearchPageProps<T, Q>) {

  const [query, setQuery] = useState<Q>(defaultQuery)
  const [data, setData] = useState<any>()

  const [error, setError] = useState<string | null>(null)

  const handleSearch = async (values: Partial<Q>) => {
    const request = { ...query, ...values }
    setQuery(request)

    try {
        setError(null)

        const res = await queryFn(request)
        setData(res.data.data)
    } catch (err: any) {
        if (err.response?.status === 404) {
        setData({ items: [], page: 1, totalCount: 0, pageSize: 10 })
        setError("No results found.")
        } else {
        setError("Something went wrong. Please try again.")
        }
    }
    }

  useEffect(() => {
    handleSearch({})
  }, [])

  return (
    <Card>
      <CardHeader>
        <CardTitle>{title}</CardTitle>
      </CardHeader>

      <CardContent className="space-y-6">

        <SearchFilters filters={filters} onSearch={handleSearch} />

        {error ? (
            <div className="text-center text-gray-500 py-8">
                {error}
            </div>
            ) : data?.items?.length ? (
            <SearchTable<T>
                columns={columns}
                data={data.items}
                onRowClick={onRowClick}
            />
            ) : (
            <div className="text-center text-gray-500 py-8">
                No results found.
            </div>
        )}

        <SearchPagination
          page={data?.page}
          totalCount={data?.totalCount}
          pageSize={data?.pageSize}
          onPageChange={(p) => handleSearch({ page: p } as Partial<Q>)}
        />

      </CardContent>
    </Card>
  )
}