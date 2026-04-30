import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"
import {
  AddAvailabilityExceptionRequest,
  AddSpecialityRequest,
  AddWorkDayRequest,
  CreateDoctorRequest,
  DeleteAvailabilityExceptionRequest,
  DoctorPaginatedQueryResponse,
  DoctorQueryViewModel,
  GetAllDoctorsRequest,
  GetAllSpecialitiesRequest,
  RecommendSpecialityRequest,
  RecommendSpecialityResponse,
  RemoveSpecialityRequest,
  RemoveWorkDayRequest,
  SpecialityPaginatedQueryResponse,
  UpdateDoctorRequest,
  UpdateWorkDayRequest
} from "../types/doctorsTypes"

export const doctorService = {
  createDoctor: (data: CreateDoctorRequest) =>
    api.post<APIResponse<DoctorQueryViewModel>>(
      ENDPOINTS.doctors.me,
      data
    ),

  updateDoctorInfo: (data: UpdateDoctorRequest) =>
    api.put<APIResponse<null>>(
      ENDPOINTS.doctors.me,
      data
    ),

  getMyDoctorInfo: () =>
    api.get<APIResponse<DoctorQueryViewModel>>(
      ENDPOINTS.doctors.me
    ),

  getDoctorByUserId: (userId: string) =>
    api.get<APIResponse<DoctorQueryViewModel>>(
      `${ENDPOINTS.doctors.root}/user/${userId}`
    ),

  getAllDoctors: (query: GetAllDoctorsRequest) =>
    api.get<APIResponse<DoctorPaginatedQueryResponse>>(
      ENDPOINTS.doctors.root,
      { params: query }
    ),

  getAllSpecialities: (query: GetAllSpecialitiesRequest) =>
    api.get<APIResponse<SpecialityPaginatedQueryResponse>>(
      ENDPOINTS.doctors.specialities,
      { params: query }
    ),

  addSpecialityByAdmin: (
    userId: string,
    data: AddSpecialityRequest
  ) =>
    api.post<APIResponse<null>>(
      ENDPOINTS.doctors.adminSpecialities(userId),
      data
    ),

  removeSpecialityByAdmin: (
    userId: string,
    data: RemoveSpecialityRequest
  ) =>
    api.delete<APIResponse<null>>(
      ENDPOINTS.doctors.adminSpecialities(userId),
      { data }
    ),

  recommendSpeciality: (data: RecommendSpecialityRequest) =>
    api.post<APIResponse<RecommendSpecialityResponse>>(
      `${ENDPOINTS.doctors.specialities}/recommend`,
      data
    ),

  addWorkDaySchedule: (data: AddWorkDayRequest) =>
    api.post<APIResponse<null>>(
      ENDPOINTS.doctors.scheduleWorkdays,
      data
    ),

  updateWorkDaySchedule: (data: UpdateWorkDayRequest) =>
    api.put<APIResponse<null>>(
      ENDPOINTS.doctors.scheduleWorkdays,
      data
    ),

  removeWorkDaySchedule: (data: RemoveWorkDayRequest) =>
    api.delete<APIResponse<null>>(
      ENDPOINTS.doctors.scheduleWorkdays,
      { data }
    ),

  addExtraAvailability: (data: AddAvailabilityExceptionRequest) =>
    api.post<APIResponse<null>>(
      ENDPOINTS.doctors.availabilityExtra,
      data
    ),

  deleteExtraAvailability: (
    data: DeleteAvailabilityExceptionRequest
  ) =>
    api.delete<APIResponse<null>>(
      ENDPOINTS.doctors.availabilityExtra,
      { data }
    ),

  addUnavailability: (data: AddAvailabilityExceptionRequest) =>
    api.post<APIResponse<null>>(
      ENDPOINTS.doctors.availabilityUnavailable,
      data
    ),

  deleteUnavailability: (
    data: DeleteAvailabilityExceptionRequest
  ) =>
    api.delete<APIResponse<null>>(
      ENDPOINTS.doctors.availabilityUnavailable,
      { data }
    ),
}