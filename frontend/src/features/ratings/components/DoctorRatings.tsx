"use client"

import { useEffect, useState } from "react"
import { Card, CardContent, CardTitle } from "@/components/ui/card"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"
import {
  Pagination,
  PaginationPrevious,
  PaginationNext,
  PaginationContent,
  PaginationItem,
  PaginationEllipsis,
  PaginationLink,
} from "@/components/ui/pagination"

import { useAuth } from "@/features/auth/hooks/useAuth"
import { ROLES } from "@/features/users/types/usersTypes"
import { Trash2 } from "lucide-react"
import { ratingService } from "../services/ratingService"

interface DoctorRatingsProps {
  ratings: RatingQueryViewModel[]
  page: number
  totalPages: number
  onPageChange: (page: number) => void
  onDeleteSuccess?: (ratingId: string) => void
  collapsedCount?: number
}

export function DoctorRatings({
  ratings,
  page,
  totalPages,
  onPageChange,
  onDeleteSuccess,
  collapsedCount = 2,
}: DoctorRatingsProps) {
  const { roles } = useAuth()

  const isAdmin = roles.includes(ROLES.ADMIN)

  const [expanded, setExpanded] = useState(false)
  const [loadingId, setLoadingId] = useState<string | null>(null)
  const [localRatings, setLocalRatings] =
    useState<RatingQueryViewModel[]>(ratings)

  useEffect(() => {
    setLocalRatings(ratings)
  }, [ratings])

  const displayedRatings = expanded
    ? localRatings
    : localRatings.slice(0, collapsedCount)

  const pages = Array.from({ length: totalPages }, (_, i) => i + 1)

  const handleDelete = async (ratingId: string) => {
    const previous = localRatings

    setLocalRatings(prev => prev.filter(r => r.id !== ratingId))

    try {
      setLoadingId(ratingId)

      await ratingService.removeRatingByAdmin(ratingId)

      onDeleteSuccess?.(ratingId)
    } catch (err) {
      console.error("Failed to delete rating", err)

      setLocalRatings(previous)
    } finally {
      setLoadingId(null)
    }
  }

  return (
    <Card className="p-2">
      <CardContent className="flex flex-col">
        <CardTitle className="mb-3 text-xl">
          Patient Reviews
        </CardTitle>

        {localRatings.length === 0 ? (
          <p className="text-gray-500 mb-4">
            No reviews yet.
          </p>
        ) : (
          <>
            <div className="space-y-2">
              {displayedRatings.map(r => (
                <div
                  key={r.id}
                  className="
                    border-b border-gray-200
                    py-2
                    transition-opacity duration-200
                  "
                >
                  <div className="flex justify-between items-start">

                    <div className="flex flex-col">
                      <span className="font-medium">
                        Score: {r.score} ⭐
                      </span>

                      <span className="text-sm text-gray-500">
                        {new Date(r.createdAt).toLocaleDateString()}
                      </span>
                    </div>

                    {isAdmin && (
                      <button
                        onClick={() => handleDelete(r.id)}
                        disabled={loadingId === r.id}
                        className="
                          inline-flex items-center gap-1.5
                          text-xs font-medium
                          text-red-600
                          hover:text-white
                          hover:bg-red-600
                          px-2 py-1
                          rounded-md
                          border border-red-200
                          transition
                          disabled:opacity-50
                          disabled:cursor-not-allowed
                        "
                      >
                        <Trash2 className="w-3.5 h-3.5" />

                        {loadingId === r.id
                          ? "Deleting..."
                          : "Delete"}
                      </button>
                    )}

                  </div>

                  <p className="mt-1">{r.comment}</p>
                </div>
              ))}
            </div>

            {localRatings.length > collapsedCount && (
              <div className="mt-2 text-center">
                <span
                  onClick={() => setExpanded(!expanded)}
                  className="
                    cursor-pointer
                    text-gray-500
                    hover:text-gray-700
                    transition
                    underline
                  "
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
                      onClick={() =>
                        page > 1 && onPageChange(page - 1)
                      }
                      size="sm"
                    />
                  </PaginationItem>

                  {pages.map(p => (
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
                      onClick={() =>
                        page < totalPages &&
                        onPageChange(page + 1)
                      }
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