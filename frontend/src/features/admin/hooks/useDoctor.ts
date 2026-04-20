import { useQuery } from "@tanstack/react-query"
import { getDoctorByUserId } from "@/features/doctors/services/doctorService"

export function useDoctor(id: string, enabled: boolean) {
  return useQuery({
    queryKey: ["doctor", id],
    enabled,
    queryFn: async () => {
      const res = await getDoctorByUserId(id)
      return res.data.data
    },
  })
}