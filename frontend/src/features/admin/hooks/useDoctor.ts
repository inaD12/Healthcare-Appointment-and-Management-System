import { doctorService } from "@/features/doctors/services/doctorService"
import { useQuery } from "@tanstack/react-query"

export function useDoctor(id: string, enabled: boolean) {
  return useQuery({
    queryKey: ["doctor", id],
    enabled,
    queryFn: async () => {
      const res = await doctorService.getDoctorByUserId(id)
      return res.data.data
    },
  })
}