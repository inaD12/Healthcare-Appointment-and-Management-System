import { useMutation, useQueryClient } from "@tanstack/react-query"
import { removeRatingByAdmin } from "@/features/ratings/services/ratingService"

export function useDeleteRating() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (id: string) => removeRatingByAdmin(id),

    onSuccess: (_, ratingId) => {
      queryClient.invalidateQueries({
        queryKey: ["doctor-ratings"],
      })
    },
  })
}