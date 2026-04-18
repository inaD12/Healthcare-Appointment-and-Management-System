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
  DoctorAppointment,
  DoctorDashboardView,
  DoctorEncounterConnection,
  MyPatientInfo,
  Appointment,
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

  addAllergyByAdmin: (patientId: string, data: AddAllergyRequest) =>
    api.post<APIResponse<AllergyCommandResponse>>(ENDPOINTS.patients.adminAllergies(patientId), data),

  removeAllergyByAdmin: (patientId: string, data: RemoveAllergyRequest) =>
    api.delete(ENDPOINTS.patients.adminAllergies(patientId), { data }),

  addChronicConditionByAdmin: (patientId: string, data: AddChronicConditionRequest) =>
    api.post<APIResponse<ConditionCommandResponse>>(ENDPOINTS.patients.adminChronicConditions(patientId), data),

  removeChronicConditionByAdmin: (patientId: string, data: RemoveConditionRequest) =>
    api.delete(ENDPOINTS.patients.adminChronicConditions(patientId), { data }),

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

  getMyPatientInfoInitial: async (first = 10): Promise<PatientInfo> => {
    const query = `
      query GetMyPatientInfoInitial($first: Int!) {
        myPatientHeader {
          id
          fullName
          birthDate
          allergies
          conditions
        }

        myAppointments(first: $first) {
          nodes {
            id
            start
            end
            status
            doctorId
            patientId
            doctorName
          }
          pageInfo {
            hasNextPage
            endCursor
          }
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: { first },
    })

    const data = res.data?.data

    const header = data?.myPatientHeader
    const conn = data?.myAppointments

    return {
      profile: {
        id: header?.id ?? "",
        fullName: header?.fullName ?? "",
        birthDate: header?.birthDate ?? "",
        allergiesList: header?.allergies ?? [],
        conditionsList: header?.conditions ?? [],
      },

      appointments: (conn?.nodes ?? []).map(mapAppointment),

      pageInfo: {
        hasNextPage: conn?.pageInfo?.hasNextPage ?? false,
        endCursor: conn?.pageInfo?.endCursor ?? null,
      },
    }
  },

  getPatientInfoInitial: async (
    userId: string,
    first = 10
  ): Promise<PatientInfo> => {
    const query = `
      query GetPatientInfoInitial($userId: String!, $first: Int!) {
        patientHeaderByUserId(userId: $userId) {
          id
          fullName
          birthDate
          allergies
          conditions
        }

        appointmentsByUserId(userId: $userId, first: $first) {
          nodes {
            id
            start
            end
            status
            doctorId
            patientId
            doctorName
          }
          pageInfo {
            hasNextPage
            endCursor
          }
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: { userId, first },
    })

    const data = res.data?.data

    const header = data?.patientHeaderByUserId
    const conn = data?.appointmentsByUserId

    return {
      profile: {
        id: header?.id ?? "",
        fullName: header?.fullName ?? "",
        birthDate: header?.birthDate ?? "",
        allergiesList: header?.allergies ?? [],
        conditionsList: header?.conditions ?? [],
      },

      appointments: (conn?.nodes ?? []).map(mapAppointment),

      pageInfo: {
        hasNextPage: conn?.pageInfo?.hasNextPage ?? false,
        endCursor: conn?.pageInfo?.endCursor ?? null,
      },
    }
  },

  getPatientAppointmentsPage: async (
    userId: string,
    first = 10,
    after?: string
  ): Promise<{
    appointments: Appointment[]
    pageInfo: { hasNextPage: boolean; endCursor: string | null }
  }> => {
    const query = `
      query GetPatientAppointments($userId: String!, $first: Int!, $after: String) {
        appointmentsByUserId(userId: $userId, first: $first, after: $after) {
          nodes {
            id
            start
            end
            status
            doctorId
            patientId
            doctorName
          }
          pageInfo {
            hasNextPage
            endCursor
          }
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: { userId, first, after },
    })

    const conn = res.data?.data?.appointmentsByUserId

    return {
      appointments: (conn?.nodes ?? []).map(mapAppointment),

      pageInfo: {
        hasNextPage: conn?.pageInfo?.hasNextPage ?? false,
        endCursor: conn?.pageInfo?.endCursor ?? null,
      },
    }
  },

  getMyPatientAppointmentsPage: async (
    first = 10,
    after?: string
  ): Promise<{
    appointments: Appointment[]
    pageInfo: { hasNextPage: boolean; endCursor: string | null }
  }> => {
    const query = `
      query GetMyAppointments($first: Int!, $after: String) {
        myAppointments(first: $first, after: $after) {
          nodes {
            id
            start
            end
            status
            doctorId
            patientId
            doctorName
          }
          pageInfo {
            hasNextPage
            endCursor
          }
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: { first, after },
    })

    const conn = res.data?.data?.myAppointments

    return {
      appointments: (conn?.nodes ?? []).map(mapAppointment),

      pageInfo: {
        hasNextPage: conn?.pageInfo?.hasNextPage ?? false,
        endCursor: conn?.pageInfo?.endCursor ?? null,
      },
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
          totalCount
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

    const todayAppointmentsNodes: DoctorAppointment[] =
      (data?.todayAppointments?.nodes as DoctorAppointment[]) ?? []

    const totalToday =
      data?.todayAppointments?.totalCount ?? todayAppointmentsNodes.length

    const now = new Date().getTime()

      const nextAppointment = todayAppointmentsNodes
        .filter(a => new Date(a.start).getTime() > now)
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


    return {
      todayAppointments: todayAppointmentsNodes,
      nextAppointment,
      totalToday,
      minutesUntilNext,
    }
  },

  getDoctorEncounters: async (
    doctorId: string,
    cursor?: string
  ): Promise<DoctorEncounterConnection> => {
    const query = `
      query GetDoctorEncounters($doctorId: String!, $after: String) {
        encountersByDoctor(
          doctorId: $doctorId
          first: 20
          after: $after
          where: {
            status: { in: [IN_PROGRESS, FINALIZED] }
          }
          order: [{ updatedAt: DESC }]
        ) {
          nodes {
            id
            patientId
            patientName
            appointmentId
            status
            startedAt
            finalizedAt
            updatedAt
          }
          pageInfo {
            hasNextPage
            endCursor
          }
          totalCount
        }
      }
    `

    const res = await api.post(ENDPOINTS.patients.graphql, {
      query,
      variables: {
        doctorId,
        after: cursor,
      },
    })

    return res.data?.data?.encountersByDoctor
  }
}

const mapAppointmentStatus = (status: string): AppointmentStatus => {
  switch (status?.toUpperCase()) {
    case "SCHEDULED":
      return AppointmentStatus.Scheduled
    case "RESCHEDULED":
      return AppointmentStatus.Rescheduled
    case "CANCELLED":
      return AppointmentStatus.Cancelled
    case "COMPLETED":
      return AppointmentStatus.Completed
    default:
      return AppointmentStatus.Scheduled
  }
}

const mapAppointment = (a: any) => ({
  id: a.id,
  start: a.start,
  end: a.end,
  status: mapAppointmentStatus(a.status),
  doctorId: a.doctorId,
  patientId: a.patientId,
  doctorName: a.doctorName,
})