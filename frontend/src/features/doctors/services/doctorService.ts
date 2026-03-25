import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { DoctorPaginatedQueryResponse, DoctorQueryViewModel, GetAllDoctorsRequest, RecommendSpecialityRequest, RecommendSpecialityResponse} from "../types/doctors";
import { APIResponse } from "@/types/types";

export const getAllDoctors = (data: GetAllDoctorsRequest) =>
  api.get<APIResponse<DoctorPaginatedQueryResponse>>(
    ENDPOINTS.doctors.admin,
    {
      params: data,
    }
  );

export const recommendSpeciality = (data: RecommendSpecialityRequest) =>
  api.post<APIResponse<RecommendSpecialityResponse>>(
    `${ENDPOINTS.doctors.specialities}/recommend`,
    data
  );

export const getDoctorByUserId = (doctorUserId: string) =>
  api.get<APIResponse<DoctorQueryViewModel>>(
    `${ENDPOINTS.doctors.admin}/by-user/${doctorUserId}`
  );