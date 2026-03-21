"use client"

import { useState } from "react"
import { Card, CardContent, CardTitle } from "@/components/ui/card"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingTypes"
import {
  Pagination,
  PaginationPrevious,
  PaginationNext,
  PaginationContent,
  PaginationItem,
  PaginationEllipsis,
  PaginationLink,   
} from "@/components/ui/pagination"

interface DoctorRatingsProps {
  ratings: RatingQueryViewModel[]
  page: number
  totalPages: number
  onPageChange: (page: number) => void
  collapsedCount?: number
}

export function DoctorRatings({
  ratings,
  page,
  totalPages,
  onPageChange,
  collapsedCount = 2,
}: DoctorRatingsProps) {
  const [expanded, setExpanded] = useState(false)

  const displayedRatings = expanded ? ratings : ratings.slice(0, collapsedCount)

  const pages = Array.from({ length: totalPages }, (_, i) => i + 1)

  return (
    <Card className="p-2">
      <CardContent className="flex flex-col">
        <CardTitle className="mb-3 text-xl">Patient Reviews</CardTitle>

        {ratings.length === 0 ? (
          <p className="text-gray-500 mb-4">No reviews yet.</p>
        ) : (
          <>
            <div className="space-y-2">
              {displayedRatings.map(r => (
                <div key={r.Id} className="border-b border-gray-200 py-2">
                  <div className="flex justify-between">
                    <span className="font-medium">Score: {r.Score} ⭐</span>
                    <span className="text-sm text-gray-500">
                      {new Date(r.CreatedAt).toLocaleDateString()}
                    </span>
                  </div>
                  <p className="mt-1">{r.Comment}</p>
                </div>
              ))}
            </div>

            {ratings.length > collapsedCount && (
                <div className="mt-2 text-center">
                    <span
                    onClick={() => setExpanded(!expanded)}
                    className="cursor-pointer text-gray-500 hover:text-gray-700 transition underline"
                    >
                    {expanded ? "Collapse" : "Expand"}
                    </span>
                </div>
                )}

            {expanded && totalPages > 1 && (
                <Pagination className="mt-2 justify-center w-auto">
                    <PaginationContent className="flex flex-wrap gap-1 justify-center">
                    <PaginationItem>
                        <PaginationPrevious
                        onClick={() => page > 1 && onPageChange(page - 1)}
                        size="sm"
                        />
                    </PaginationItem>

                    {pages.map((p) => (
                        <PaginationItem key={p}>
                        <PaginationLink
                            onClick={() => onPageChange(p)}
                            isActive={p === page}
                            size="sm"
                        >
                            {p}
                        </PaginationLink>
                        </PaginationItem>
                    ))}

                    {totalPages > 5 && (
                        <PaginationItem>
                        <PaginationEllipsis />
                        </PaginationItem>
                    )}

                    <PaginationItem>
                        <PaginationNext
                        onClick={() => page < totalPages && onPageChange(page + 1)}
                        size="sm"
                        />
                    </PaginationItem>
                    </PaginationContent>
                </Pagination>
            )}
          </>
        )}
      </CardContent>
    </Card>
  )
}