import { APIResponse } from "@/types/types"
import {
  AddAddendumRequest,
  AddAllergyRequest,
  AddChronicConditionRequest,
  AddDiagnosisRequest,
  AddendumCommandResponse,
  AddNoteRequest,
  AllergyCommandResponse,
  AppointmentByIdResponse,
  AppointmentStatus,
  ConditionCommandResponse,
  DiagnosisCommandResponse,
  EncounterCommandResponse,
  NoteCommandResponse,
  PatientDashboard,
  PatientProfile,
  PrescribeMedicationRequest,
  PrescriptionCommandResponse,
  RemoveAllergyRequest,
  RemoveConditionRequest,
  RemoveDiagnosisRequest,
  RemoveNoteRequest,
  RemovePrescriptionRequest,
  StartEncounterRequest,
} from "../types/patientTypes"
import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"

export const patientService = {
  addAllergy: (patientId: string, data: AddAllergyRequest) =>
    api.post<APIResponse<AllergyCommandResponse>>(ENDPOINTS.patients.allergies(patientId), data),

  removeAllergy: (patientId: string, data: RemoveAllergyRequest) =>
    api.delete(ENDPOINTS.patients.allergies(patientId), { data }),

  addChronicCondition: (patientId: string, data: AddChronicConditionRequest) =>
    api.post<APIResponse<ConditionCommandResponse>>(ENDPOINTS.patients.chronicConditions(patientId), data),

  removeChronicCondition: (patientId: string, data: RemoveConditionRequest) =>
    api.delete(ENDPOINTS.patients.chronicConditions(patientId), { data }),

  startEncounter: (data: StartEncounterRequest) =>
    api.post<APIResponse<EncounterCommandResponse>>(ENDPOINTS.encounters.root, data),

  addNote: (encounterId: string, data: AddNoteRequest) =>
    api.post<APIResponse<NoteCommandResponse>>(ENDPOINTS.encounters.notes(encounterId), data),

  removeNote: (encounterId: string, data: RemoveNoteRequest) =>
    api.delete(ENDPOINTS.encounters.notes(encounterId), { data }),

  addDiagnosis: (encounterId: string, data: AddDiagnosisRequest) =>
    api.post<APIResponse<DiagnosisCommandResponse>>(ENDPOINTS.encounters.diagnoses(encounterId), data),

  removeDiagnosis: (encounterId: string, data: RemoveDiagnosisRequest) =>
    api.delete(ENDPOINTS.encounters.diagnoses(encounterId), { data }),

  prescribeMedication: (encounterId: string, data: PrescribeMedicationRequest) =>
    api.post<APIResponse<PrescriptionCommandResponse>>(ENDPOINTS.encounters.prescriptions(encounterId), data),

  removePrescription: (encounterId: string, data: RemovePrescriptionRequest) =>
    api.delete(ENDPOINTS.encounters.prescriptions(encounterId), { data }),

  addAddendum: (encounterId: string, data: AddAddendumRequest) =>
    api.post<APIResponse<AddendumCommandResponse>>(ENDPOINTS.encounters.addendums(encounterId), data),

  lockEncounter: (encounterId: string) =>
    api.post(ENDPOINTS.encounters.lock(encounterId)),

  finalizeEncounter: (encounterId: string) =>
    api.post(ENDPOINTS.encounters.finalize(encounterId)),
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
  `

  const res = await api.post(ENDPOINTS.patients.graphql, { query })

  const header = res.data?.data?.myPatientHeader
  const appointments = res.data?.data?.myAppointments?.nodes ?? []

  const mapStatus = (status: string) => {
    switch (status.toUpperCase()) {
      case "SCHEDULED": return AppointmentStatus.Scheduled
      case "RESCHEDULED": return AppointmentStatus.Rescheduled
      case "CANCELLED": return AppointmentStatus.Cancelled
      case "COMPLETED": return AppointmentStatus.Completed
      default: return AppointmentStatus.Scheduled
    }
  }

  return {
    profile: {
      id: header?.id ?? "",
      fullName: header?.fullName ?? "",
      birthDate: header?.birthDate ?? "",
      allergies: header?.allergies ?? [],
      conditions: header?.conditions ?? [],
    },
    appointments: appointments.map((a: any) => ({
      id: a.id,
      start: a.start,
      end: a.end,
      status: mapStatus(a.status),
      doctorId: a.doctorId,
      patientId: a.patientId,
      doctorName: a.doctorName
    })),
  }
}

export async function getAppointmentWithEncounters(
  appointmentId: string
): Promise<AppointmentByIdResponse | null> {
  const query = `
    query GetAppointmentWithEncounters($appointmentId: String!) {
      appointmentById(appointmentId: $appointmentId) {
        id
        start
        end
        status
        doctorId
        patientId
        doctorName
        encounterDetails {
          id
          startedAt
          finalizedAt
          status
          notes {
            id
            text
            createdAt
          }
          diagnoses {
            id
            icdCode
            description
          }
          prescriptions {
            id
            medicationName
            dosage
            instructions
          }
          addendums {
            id
            text
            createdAt
          }
        }
      }
    }
  `

  const res = await api.post(ENDPOINTS.patients.graphql, {
    query,
    variables: { appointmentId }
  })

  return res.data?.data ?? null
}

