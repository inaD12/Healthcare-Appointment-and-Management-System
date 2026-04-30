import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"

export default function RatingCard({ rating }: { rating: RatingQueryViewModel }) {

  return (
    <Card className="shadow-sm">

      <CardHeader>
        <CardTitle>Patient Rating</CardTitle>
      </CardHeader>

      <CardContent className="space-y-3">

        <div className="flex text-3xl text-yellow-400">
          {[1,2,3,4,5].map(star => (
            <span key={star}>
              {star <= rating.score ? "★" : "☆"}
            </span>
          ))}
        </div>

        {rating.comment && (
          <p className="italic text-muted-foreground">
            "{rating.comment}"
          </p>
        )}

        <p className="text-sm text-muted-foreground">
          Submitted {new Date(rating.createdAt).toLocaleString()}
        </p>

      </CardContent>

    </Card>
  )
}