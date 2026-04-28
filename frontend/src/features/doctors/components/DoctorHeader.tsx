import { Card, CardContent, CardTitle, CardDescription } from "@/components/ui/card"
import { DoctorQueryViewModel } from "@/features/doctors/types/doctorsTypes"
import { DoctorRatings } from "@/features/ratings/components/DoctorRatings"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"

type Props = {
  doctor: DoctorQueryViewModel
  ratings: RatingQueryViewModel[]
  ratingsPage: number
  ratingsTotalPages: number
  onPageChange: (page: number) => void
}

export default function DoctorHeader({
  doctor,
  ratings,
  ratingsPage,
  ratingsTotalPages,
  onPageChange,
}: Props) {
  return (
    <>
      <Card>
        <CardContent>
          <CardTitle className="text-2xl flex justify-between">
            {doctor.firstName} {doctor.lastName}
            <span className="text-yellow-600 text-sm">
              ⭐ {doctor.averageRating?.toFixed(1) ?? "0.0"} ({doctor.ratingsCount ?? 0})
            </span>
          </CardTitle>
          <CardDescription>{doctor.bio}</CardDescription>
          <p><b>Specialities:</b> {doctor.specialities.join(", ")}</p>
        </CardContent>
      </Card>

      <DoctorRatings
        ratings={ratings}
        page={ratingsPage}
        totalPages={ratingsTotalPages}
        onPageChange={onPageChange}
      />
    </>
  )
}