import { useQuery } from "@tanstack/react-query"
import { getUserByAdmin } from "@/features/users/services/userService"

export function useAdminUser(id: string) {
  return useQuery({
    queryKey: ["admin-user", id],
    queryFn: async () => {
      const res = await getUserByAdmin(id)
      return res.data.data
    },
  })
}