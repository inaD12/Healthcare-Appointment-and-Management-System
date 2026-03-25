import { APIResponse } from "@/types/types"
import {
  AddAllergyRequest,
  AddChronicConditionRequest,
  AllergyCommandResponse,
  ConditionCommandResponse,
  PatientDashboard,
  PatientProfile,
  RemoveAllergyRequest,
  RemoveConditionRequest,
} from "../types/patientTypes"
import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"

export const patientService = {
  addAllergy: (patientId: string, data: AddAllergyRequest) =>
    api.post<APIResponse<AllergyCommandResponse>>(
      ENDPOINTS.patients.allergies(patientId),
      data
    ),

  removeAllergy: (patientId: string, data: RemoveAllergyRequest) =>
    api.delete(ENDPOINTS.patients.allergies(patientId), {
      data,
    }),

  addChronicCondition: (patientId: string, data: AddChronicConditionRequest) =>
    api.post<APIResponse<ConditionCommandResponse>>(
      ENDPOINTS.patients.chronicConditions(patientId),
      data
    ),

  removeChronicCondition: (patientId: string, data: RemoveConditionRequest) =>
    api.delete(ENDPOINTS.patients.chronicConditions(patientId), {
      data,
    }),
}

export async function getPatientProfile(patientId: string): Promise<PatientProfile> {
  const query = `
    query GetPatientProfile($patientId: String!) {
      patientHeader(patientId: $patientId) {
        id
        fullName
        birthDate
        allergies
        conditions
      }
    }
  `

  const res = await api.post(ENDPOINTS.patients.graphql, {
    query,
    variables: { patientId },
  })

  const patient = res.data?.data?.patientHeader?.[0]

  return {
    id: patient?.id ?? "",
    fullName: patient?.fullName ?? "",
    birthDate: patient?.birthDate ?? "",
    allergies: patient?.allergies ?? [],
    conditions: patient?.conditions ?? [],
  }
}

export async function getPatientDashboard(): Promise<PatientDashboard> {
  const query = `
    query GetMyPatientDashboard {
      myPatientHeader {
        id
        fullName
        birthDate
        allergies
        conditions
      }

      myEncounters(first: 20) {
        nodes {
          id
          startedAt
          status
          doctorId
          patientId
        }
        totalCount
      }

      myAppointments(first: 20) {
        nodes {
          id
          start
          end
          status
          doctorId
          patientId
          doctorName
        }
        totalCount
      }
    }
  `;

  const res = await api.post(ENDPOINTS.patients.graphql, { query });

  const header = res.data?.data?.myPatientHeader?.[0];
  const encounters = res.data?.data?.myEncounters?.nodes ?? [];
  const appointments = res.data?.data?.myAppointments?.nodes ?? [];

  return {
    id: header?.id ?? "",
    fullName: header?.fullName ?? "",
    birthDate: header?.birthDate ?? "",
    allergies: header?.allergies ?? [],
    conditions: header?.conditions ?? [],
    encounters: encounters.map((e: any) => ({
      id: e.id,
      startedAt: e.startedAt,
      status: e.status,
      doctorId: e.doctorId,
      patientId: e.patientId,
    })),
    appointments: appointments.map((a: any) => ({
      id: a.id,
      start: a.start,
      end: a.end,
      status: a.status,
      doctorId: a.doctorId,
      patientId: a.patientId,
      doctorName: a.doctorName
    })),
  };
}