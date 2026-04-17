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

  const handleSearch = async (values: Partial<Q>) => {
    const request = { ...query, ...values }
    setQuery(request)

    const res = await queryFn(request)
    setData(res.data.data)
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

        <SearchTable<T>
          columns={columns}
          data={data?.items || []}
          onRowClick={onRowClick}
        />

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