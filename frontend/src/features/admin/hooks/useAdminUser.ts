import { userService } from "@/features/users/services/userService"
import { useQuery } from "@tanstack/react-query"

export function useAdminUser(id: string) {
  return useQuery({
    queryKey: ["admin-user", id],
    queryFn: async () => {
      const res = await userService.getUserByAdmin(id)
      return res.data.data
    },
  })
}