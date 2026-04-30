import { ratingService } from "@/features/ratings/services/ratingService"
import { useMutation, useQueryClient } from "@tanstack/react-query"

export function useDeleteRating() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: string) => ratingService.removeRatingByAdmin(id),

    onSuccess: (_, ratingId) => {
      queryClient.invalidateQueries({
        queryKey: ["doctor-ratings"],
      })
    },
  })
}