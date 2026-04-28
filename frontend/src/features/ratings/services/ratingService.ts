import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"
import {
  AddRatingRequest,
  EditRatingRequest,
  GetAllRatingsByDoctorRequest,
  RatingCommandResponse,
  RatingPaginatedQueryResponse,
  RatingQueryViewModel
} from "../types/ratingsTypes"

export const ratingService = {
  getRatingsByDoctor: (
    doctorId: string,
    request: GetAllRatingsByDoctorRequest
  ) =>
    api.get<APIResponse<RatingPaginatedQueryResponse>>(
      ENDPOINTS.ratings.byDoctor(doctorId),
      { params: request }
    ),

  addRating: (data: AddRatingRequest) =>
    api.post<APIResponse<RatingCommandResponse>>(
      ENDPOINTS.ratings.root,
      data
    ),

  editRating: (
    ratingId: string,
    data: EditRatingRequest
  ) =>
    api.put<APIResponse<boolean>>(
      ENDPOINTS.ratings.byId(ratingId),
      data
    ),

  removeRating: (ratingId: string) =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.ratings.byId(ratingId)
    ),

  removeRatingByAdmin: (ratingId: string) =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.ratings.adminById(ratingId)
    ),

  getRatingByAppointment: (appointmentId: string) =>
    api.get<APIResponse<RatingQueryViewModel>>(
      ENDPOINTS.ratings.byAppointment(appointmentId)
    ),
}