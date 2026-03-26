import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { AddRatingRequest, GetAllRatingsByDoctorRequest, RatingCommandResponse, RatingPaginatedQueryResponse } from "../types/ratingTypes"
import { APIResponse } from "@/types/types"


export const getRatingsByDoctor = (
  doctorId: string,
  data: GetAllRatingsByDoctorRequest
) =>
  api.get<APIResponse<RatingPaginatedQueryResponse>>(
    ENDPOINTS.ratings.byDoctor(doctorId),
    { params: data }
  )

export const addRating = (
  data: AddRatingRequest
) =>
  api.post<APIResponse<RatingCommandResponse>>(
    ENDPOINTS.ratings.root,
    data
  )