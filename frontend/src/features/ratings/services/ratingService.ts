import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { AddRatingRequest, EditRatingRequest, GetAllRatingsByDoctorRequest, RatingCommandResponse, RatingPaginatedQueryResponse, RatingQueryViewModel } from "../types/ratingTypes"
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

export const editRating = (
  ratingId: string,
  data: EditRatingRequest
) =>
  api.put<APIResponse<boolean>>(
    ENDPOINTS.ratings.byId(ratingId),
    data
  )

export const removeRating = (
  ratingId: string,
) =>
  api.delete<APIResponse<boolean>>(
    ENDPOINTS.ratings.byId(ratingId)
  )

export const getRatingByAppointment = (
  appointmentId: string
) =>
  api.get<APIResponse<RatingQueryViewModel>>(
    ENDPOINTS.ratings.byAppointment(appointmentId)
  )