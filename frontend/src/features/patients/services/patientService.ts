import { APIResponse } from "@/types/types";
import { AddAllergyRequest, AddChronicConditionRequest, AllergyCommandResponse, ConditionCommandResponse, RemoveAllergyRequest, RemoveConditionRequest } from "../types/patientTypes";
import { api } from "@/lib/api/axios";

export const patientService = {
  addAllergy: (patientId: string, data: AddAllergyRequest) =>
    api.post<APIResponse<AllergyCommandResponse>>(`/api/patients/${patientId}/allergies`, data),

  removeAllergy: (patientId: string, data: RemoveAllergyRequest) =>
    api.delete(`/api/patients/${patientId}/allergies`, {
      data,
    }),

  addChronicCondition: (patientId: string, data: AddChronicConditionRequest) =>
    api.post<APIResponse<ConditionCommandResponse>>(`/api/patients/${patientId}/chronic-conditions`, data),

  removeChronicCondition: (patientId: string, data: RemoveConditionRequest) =>
    api.delete(`/api/patients/${patientId}/chronic-conditions`, {
      data,
    }),
}