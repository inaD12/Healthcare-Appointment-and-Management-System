import {
  Card,
  CardContent,
  CardTitle,
  CardDescription,
} from "@/components/ui/card"

export function DoctorCard({ doctor, onClick }: any) {
  return (
    <Card
      onClick={onClick}
      className="cursor-pointer hover:shadow-lg hover:scale-[1.01] transition"
    >
      <CardContent className="flex flex-col md:flex-row justify-between items-start md:items-center gap-4">

        <div>
          <CardTitle>
            {doctor.firstName} {doctor.lastName}
          </CardTitle>

          <CardDescription>{doctor.bio}</CardDescription>

          <p className="mt-2">
            <b>Specialities:</b> {doctor.specialities.join(", ")}
          </p>

          <p className="text-sm text-muted-foreground mt-2">
            <span className="text-yellow-400">★</span>{" "}
            {doctor.averageRating?.toFixed(1) ?? "0.0"} (
            {doctor.ratingsCount ?? 0})
          </p>
        </div>

      </CardContent>
    </Card>
  )
}