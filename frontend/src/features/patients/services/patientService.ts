import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"
import {
  AddAddendumRequest,
  AddAllergyRequest,
  AddChronicConditionRequest,
  AddDiagnosisRequest,
  AddNoteRequest,
  PrescribeMedicationRequest,
  RemoveAllergyRequest,
  RemoveConditionRequest,
  RemoveDiagnosisRequest,
  RemoveNoteRequest,
  RemovePrescriptionRequest,
  StartEncounterRequest,
  AddendumCommandResponse,
  AllergyCommandResponse,
  ConditionCommandResponse,
  DiagnosisCommandResponse,
  EncounterCommandResponse,
  NoteCommandResponse,
  PrescriptionCommandResponse,
  PatientProfile,
  AppointmentStatus,
  AppointmentByIdResponse,
  PatientInfo,
  PatientDashboard,
  DoctorDashboard,
  DoctorAppointment,
  DoctorDashboardView,
  DoctorEncounter,
} from "../types/patientTypes"

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

  lockEncounter: (encounterId: string) =>
    api.post(ENDPOINTS.encounters.lock(encounterId)),

  finalizeEncounter: (encounterId: string) =>
    api.post(ENDPOINTS.encounters.finalize(encounterId)),

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

  getPatientProfile: async (patientId: string): Promise<PatientProfile> => {
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
    const res = await api.post(ENDPOINTS.patients.graphql, { query, variables: { patientId } })
    const patient = res.data?.data?.patientHeader?.[0]
    return {
      id: patient?.id ?? "",
      fullName: patient?.fullName ?? "",
      birthDate: patient?.birthDate ?? "",
      allergiesList: patient?.allergies ?? [],
      conditionsList: patient?.conditions ?? [],
    }
  },

  getPatientInfo: async (): Promise<PatientInfo> => {
    const query = `
      query GetMyPatientInfo {
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
        allergiesList: header?.allergies ?? [],
        conditionsList: header?.conditions ?? [],
      },
      appointments: appointments.map((a: any) => ({
        id: a.id,
        start: a.start,
        end: a.end,
        status: mapStatus(a.status),
        doctorId: a.doctorId,
        patientId: a.patientId,
        doctorName: a.doctorName,
      })),
    }
  },
  
  getAppointmentWithEncounters: async (appointmentId: string): Promise<AppointmentByIdResponse | null> => {
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
          patientName
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
    const res = await api.post(ENDPOINTS.patients.graphql, { query, variables: { appointmentId } })
    return res.data?.data ?? null
  },
  getPatientDashboard: async (): Promise<PatientDashboard> => {
    const query = `
      query GetPatientDashboard {
        myPatientHeader {
          id
          fullName
          birthDate
          allergiesList
          conditionsList
        }

        upcomingAppointment: myAppointments(
          first: 1
          where: { status: { eq: SCHEDULED } }
          order: [{ start: ASC }]
        ) {
          nodes {
            id
            start
            end
            status
            doctorId
            doctorName
          }
        }

        lastAppointment: myAppointments(
          first: 1
          where: { status: { eq: COMPLETED } }
          order: [{ start: DESC }]
        ) {
          nodes {
            id
            start
            end
            status
            doctorId
            doctorName
          }
        }

        myEncounters(
          first: 3
          order: [{ updatedAt: DESC }]
        ) {
          nodes {
            id
            startedAt
            updatedAt
            status
            doctorId
            appointmentId

            prescriptions {
              id
              medicationName
              dosage
              instructions
              createdAt
              deletedAt
            }

            notes {
              id
              text
              createdAt
              deletedAt
            }

            addendums {
              id
              text
              createdAt
              deletedAt
            }

            diagnoses {
              id
              icdCode
              description
              createdAt
              deletedAt
            }
          }
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, { query })

    return res.data?.data
  },
  
getDoctorDashboard: async (
    doctorId: string
  ): Promise<DoctorDashboardView> => {
    const today = new Date().toISOString().split("T")[0]

    const query = `
      query DoctorDashboard($doctorId: String!, $todayStart: DateTime!, $todayEnd: DateTime!) {

        todayAppointments: appointmentsByDoctor(
          doctorId: $doctorId
          first: 20
          where: {
            start: {
              gte: $todayStart
              lt: $todayEnd
            }
          }
          order: [{ start: ASC }]
        ) {
          nodes {
            id
            start
            end
            status
            patientName
            patientId
          }
        }

        encountersByDoctor(
          doctorId: $doctorId
          first: 20
          where: {
            status: {
              in: [IN_PROGRESS, FINALIZED]
            }
          }
          order: [{ updatedAt: DESC }]
        ) {
          nodes {
            id
            appointmentId
            patientId
            doctorId
            status
            startedAt
            finalizedAt
            updatedAt
          }
        }

      }
    `

    const todayStart = new Date(`${today}T00:00:00.000Z`).toISOString()
    const todayEnd = new Date(`${today}T23:59:59.999Z`).toISOString()

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: {
        doctorId,
        todayStart,
        todayEnd,
      },
    })

    const data = res.data?.data

    const todayAppointments: DoctorAppointment[] =
      data?.todayAppointments?.nodes ?? []

    const encounters: DoctorEncounter[] =
      data?.encountersByDoctor?.nodes ?? []

    const nextAppointment = todayAppointments
      .filter((a) => new Date(a.start) > new Date())
      .sort(
        (a, b) =>
          new Date(a.start).getTime() - new Date(b.start).getTime()
      )[0]

    const minutesUntilNext = nextAppointment
      ? Math.max(
          0,
          Math.floor(
            (new Date(nextAppointment.start).getTime() -
              new Date().getTime()) /
              60000
          )
        )
      : undefined

    const unfinishedEncounters = encounters

    return {
      todayAppointments,
      nextAppointment,
      totalToday: todayAppointments.length,
      minutesUntilNext,
      unfinishedEncounters,
    }
  }
}