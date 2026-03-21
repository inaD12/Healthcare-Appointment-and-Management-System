import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { GetAllRatingsByDoctorRequest, RatingPaginatedQueryResponse } from "../types/ratingTypes"
import { APIResponse } from "@/types/types"


export const getRatingsByDoctor = (
  doctorId: string,
  data: GetAllRatingsByDoctorRequest
) =>
  api.get<APIResponse<RatingPaginatedQueryResponse>>(
    ENDPOINTS.ratings.byDoctor(doctorId),
    { params: data }
  )
