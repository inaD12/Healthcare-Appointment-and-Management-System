import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { DoctorPaginatedQueryResponse, DoctorQueryViewModel, GetAllDoctorsRequest, RecommendSpecialityRequest, RecommendSpecialityResponse } from "../types/doctors";

export const getAllDoctors = (data: GetAllDoctorsRequest) =>
  api.post<DoctorPaginatedQueryResponse>(
    ENDPOINTS.doctors.admin,
    data
  );

export const recommendSpeciality = (data: RecommendSpecialityRequest) =>
  api.post<RecommendSpecialityResponse>(
    `${ENDPOINTS.doctors.specialities}/recommend`,
    data
  );

export const getDoctorById = (doctorId: string) =>
  api.get<DoctorQueryViewModel>(
    `${ENDPOINTS.doctors.admin}/by-id/${doctorId}`
  );