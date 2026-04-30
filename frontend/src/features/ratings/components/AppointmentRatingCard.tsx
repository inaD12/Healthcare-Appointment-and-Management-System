"use client"

import { useState } from "react"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"
import { Button } from "@/components/ui/button"
import { Textarea } from "@/components/ui/textarea"
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"

type Props = {
  rating: RatingQueryViewModel | null
  loading?: boolean

  onCreate?: (data: { score: number; comment: string }) => Promise<void>
  onEdit?: (data: { score: number; comment: string }) => Promise<void>
  onDelete?: () => Promise<void>
}

export default function AppointmentRatingCard({
  rating,
  loading = false,
  onCreate,
  onEdit,
  onDelete,
}: Props) {
  const [isEditing, setIsEditing] = useState(false)

  const [score, setScore] = useState(rating?.score ?? 5)
  const [comment, setComment] = useState(rating?.comment ?? "")

  const canCreate = !!onCreate
  const canEdit = !!onEdit && !!rating
  const canDelete = !!onDelete

  const isReadOnlyMode = !canCreate && !canEdit && !canDelete

  const handleSubmit = async () => {
    if (rating && onEdit) {
      await onEdit({ score, comment })
      setIsEditing(false)
      return
    }

    if (!rating && onCreate) {
      await onCreate({ score, comment })
    }
  }

  const startEdit = () => {
    setScore(rating?.score ?? 5)
    setComment(rating?.comment ?? "")
    setIsEditing(true)
  }

  const cancelEdit = () => {
    setIsEditing(false)
    setScore(rating?.score ?? 5)
    setComment(rating?.comment ?? "")
  }

  if (isReadOnlyMode) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Rating</CardTitle>
        </CardHeader>

        <CardContent className="space-y-4">
          {rating ? (
            <>
              <div className="flex text-3xl text-yellow-400">
                {[1, 2, 3, 4, 5].map((s) => (
                  <span key={s}>{s <= rating.score ? "★" : "☆"}</span>
                ))}
              </div>

              {rating.comment && (
                <p className="italic text-muted-foreground">
                  "{rating.comment}"
                </p>
              )}

              <p className="text-sm text-muted-foreground">
                {new Date(rating.createdAt).toLocaleString()}
              </p>
            </>
          ) : (
            <p className="text-muted-foreground text-sm">
              No rating available.
            </p>
          )}
        </CardContent>
      </Card>
    )
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>{!canEdit ? "Rating" : rating ? "Your Rating" : "Rate appointment"}</CardTitle>
      </CardHeader>

      <CardContent className="space-y-4">
        {rating && !isEditing && (
          <>
            <div className="flex text-3xl text-yellow-400">
              {[1, 2, 3, 4, 5].map((s) => (
                <span key={s}>{s <= rating.score ? "★" : "☆"}</span>
              ))}
            </div>

            {rating.comment && (
              <p className="italic text-muted-foreground">
                "{rating.comment}"
              </p>
            )}

            <div className="flex gap-2">
              {canEdit && <Button onClick={startEdit}>Edit</Button>}

              {canDelete && (
                <Button variant="destructive" onClick={onDelete}>
                  Delete
                </Button>
              )}
            </div>
          </>
        )}

        {(!rating || isEditing) && (canCreate || canEdit) && (
          <>
            <div className="flex gap-2 text-3xl">
              {[1, 2, 3, 4, 5].map((s) => (
                <button
                  key={s}
                  onClick={() => setScore(s)}
                  className={s <= score ? "text-yellow-400" : "text-gray-300"}
                >
                  ★
                </button>
              ))}
            </div>

            <Textarea
              value={comment}
              onChange={(e) => setComment(e.target.value)}
              placeholder="Leave feedback..."
            />

            <div className="flex gap-2">
              <Button onClick={handleSubmit} disabled={loading}>
                {rating ? "Save" : "Submit"}
              </Button>

              {rating && canEdit && (
                <Button variant="secondary" onClick={cancelEdit}>
                  Cancel
                </Button>
              )}
            </div>
          </>
        )}
      </CardContent>
    </Card>
  )
}