"use client"

import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableHead,
  TableCell,
} from "@/components/ui/table"

import { Card } from "@/components/ui/card"
import { ColumnDef } from "./types"

interface SearchTableProps<T> {
  columns: ColumnDef<T>[]
  data: T[]
  onRowClick?: (row: T) => void
}

export function SearchTable<T>({
  columns,
  data,
  onRowClick,
}: SearchTableProps<T>) {
  if (!data || data.length === 0) {
    return (
      <Card className="p-6 text-center text-gray-500">
        No results found.
      </Card>
    )
  }

  return (
    <div className="rounded-md border">

      <Table>

        <TableHeader>
          <TableRow>
            {columns.map((col) => (
              <TableHead key={col.header} className="py-4">
                {col.header}
              </TableHead>
            ))}
          </TableRow>
        </TableHeader>

        <TableBody>
          {data.map((row: any) => (
            <TableRow
              key={row.id}
              onClick={() => onRowClick?.(row)}
              className={
                onRowClick
                  ? "cursor-pointer hover:bg-gray-50"
                  : ""
              }
            >
              {columns.map((col) => (
                <TableCell key={col.header} className="py-4">
                  {typeof col.accessor === "function"
                    ? col.accessor(row)
                    : row[col.accessor]}
                </TableCell>
              ))}
            </TableRow>
          ))}
        </TableBody>

      </Table>

    </div>
  )
}