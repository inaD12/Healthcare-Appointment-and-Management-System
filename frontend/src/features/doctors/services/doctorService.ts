import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { AddAvailabilityExceptionRequest, AddSpecialityRequest, AddWorkDayRequest, CreateDoctorRequest, DeleteAvailabilityExceptionRequest, DoctorPaginatedQueryResponse, DoctorQueryViewModel, GetAllDoctorsRequest, RecommendSpecialityRequest, RecommendSpecialityResponse, RemoveSpecialityRequest, RemoveWorkDayRequest, UpdateDoctorRequest, UpdateWorkDayRequest} from "../types/doctors";
import { APIResponse } from "@/types/types";

export const createDoctor = (data: CreateDoctorRequest) =>
  api.post<APIResponse<DoctorQueryViewModel>>(ENDPOINTS.doctors.me, data)

export const updateDoctorInfo = (data: UpdateDoctorRequest) =>
  api.put<APIResponse<null>>(ENDPOINTS.doctors.me, data)

export const getMyDoctorInfo = () =>
  api.get<APIResponse<DoctorQueryViewModel>>(ENDPOINTS.doctors.me)

export const getDoctorByUserId = (userId: string) =>
  api.get<APIResponse<DoctorQueryViewModel>>(`${ENDPOINTS.doctors.admin}/by-user/${userId}`)

export const getAllDoctors = (query: GetAllDoctorsRequest) =>
  api.get<APIResponse<DoctorPaginatedQueryResponse>>(ENDPOINTS.doctors.admin, { params: query })

export const addSpeciality = (data: AddSpecialityRequest) =>
  api.post<APIResponse<null>>(ENDPOINTS.doctors.meSpecialities, data)

export const removeSpeciality = (data: RemoveSpecialityRequest) =>
  api.delete<APIResponse<null>>(ENDPOINTS.doctors.meSpecialities, { data })

export const recommendSpeciality = (data: RecommendSpecialityRequest) =>
  api.post<APIResponse<RecommendSpecialityResponse>>(ENDPOINTS.doctors.specialities + "/recommend", data)

export const addWorkDaySchedule = (data: AddWorkDayRequest) =>
  api.post<APIResponse<null>>(ENDPOINTS.doctors.scheduleWorkdays, data)

export const updateWorkDaySchedule = (data: UpdateWorkDayRequest) =>
  api.put<APIResponse<null>>(ENDPOINTS.doctors.scheduleWorkdays, data)

export const removeWorkDaySchedule = (data: RemoveWorkDayRequest) =>
  api.delete<APIResponse<null>>(ENDPOINTS.doctors.scheduleWorkdays, { data })

export const addExtraAvailability = (data: AddAvailabilityExceptionRequest) =>
  api.post<APIResponse<null>>(ENDPOINTS.doctors.availabilityExtra, data)

export const deleteExtraAvailability = (data: DeleteAvailabilityExceptionRequest) =>
  api.delete<APIResponse<null>>(ENDPOINTS.doctors.availabilityExtra, { data })

export const addUnavailability = (data: AddAvailabilityExceptionRequest) =>
  api.post<APIResponse<null>>(ENDPOINTS.doctors.availabilityUnavailable, data)

export const deleteUnavailability = (data: DeleteAvailabilityExceptionRequest) =>
  api.delete<APIResponse<null>>(ENDPOINTS.doctors.availabilityUnavailable, { data })