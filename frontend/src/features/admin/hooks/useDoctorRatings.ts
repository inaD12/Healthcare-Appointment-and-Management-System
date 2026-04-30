import { useQuery, keepPreviousData } from "@tanstack/react-query"
import { RatingQueryViewModel } from "@/features/ratings/types/ratingsTypes"
import { ratingService } from "@/features/ratings/services/ratingService"

interface InitialData {
  items: RatingQueryViewModel[]
  total: number
}

export function useDoctorRatings(id: string, page: number, initialData?: InitialData) {
  return useQuery({
    queryKey: ["doctor-ratings", id, page],
    placeholderData: keepPreviousData,
    initialData: page === 1 ? initialData : undefined,
    queryFn: async () => {
      const res = await ratingService.getRatingsByDoctor(id, {
        PatientId: "",
        AppointmentId: "",
        MinScore: null,
        MaxScore: null,
        SortOrder: "DESC",
        SortPropertyName: "CreatedAt",
        Page: page,
        PageSize: 4,
      })
      return {
        items: res.data.data.items ?? [],
        total: res.data.data.totalCount ?? 0,
      }
    },
  })
}