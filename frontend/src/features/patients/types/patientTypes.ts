import { z } from "zod"
import { PatientsBusinessConfiguration as cfg } from "../config/business"

export const AddAllergySchema = z.object({
  Substance: z.string()
    .min(cfg.SUBSTANCE_MIN_LENGTH, { message: `Substance must be at least ${cfg.SUBSTANCE_MIN_LENGTH} characters` })
    .max(cfg.SUBSTANCE_MAX_LENGTH, { message: `Substance must be at most ${cfg.SUBSTANCE_MAX_LENGTH} characters` }),

  Reaction: z.string()
    .min(cfg.REACTION_MIN_LENGTH, { message: `Reaction must be at least ${cfg.REACTION_MIN_LENGTH} characters` })
    .max(cfg.REACTION_MAX_LENGTH, { message: `Reaction must be at most ${cfg.REACTION_MAX_LENGTH} characters` }),
})

export const AddChronicConditionSchema = z.object({
  Name: z.string()
    .min(cfg.CHRONIC_CONDITION_NAME_MIN_LENGTH, { message: `Condition name must be at least ${cfg.CHRONIC_CONDITION_NAME_MIN_LENGTH} characters` })
    .max(cfg.CHRONIC_CONDITION_NAME_MAX_LENGTH, { message: `Condition name must be at most ${cfg.CHRONIC_CONDITION_NAME_MAX_LENGTH} characters` }),
})

export const RemoveAllergySchema = z.object({
  AllergyId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid allergy ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid allergy ID" }),
})

export const RemoveConditionSchema = z.object({
  ConditionId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid condition ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid condition ID" }),
})

export const AllergyCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const ConditionCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const StartEncounterSchema = z.object({
  appointmentId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid appointment ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid appointment ID" }),
})

export const AddNoteSchema = z.object({
  note: z.string()
    .min(cfg.CLINICAL_NOTE_TEXT_MIN_LENGTH, { message: `Note must be at least ${cfg.CLINICAL_NOTE_TEXT_MIN_LENGTH} characters` })
    .max(cfg.CLINICAL_NOTE_TEXT_MAX_LENGTH, { message: `Note must be at most ${cfg.CLINICAL_NOTE_TEXT_MAX_LENGTH} characters` }),
})

export const RemoveNoteSchema = z.object({
  noteId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid note ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid note ID" }),
})

export const AddDiagnosisSchema = z.object({
  icdCode: z.string()
    .min(cfg.ICD_MIN_LENGTH, { message: `ICD code must be at least ${cfg.ICD_MIN_LENGTH} characters` })
    .max(cfg.ICD_MAX_LENGTH, { message: `ICD code must be at most ${cfg.ICD_MAX_LENGTH} characters` }),

  description: z.string()
    .min(cfg.DIAGNOSIS_DESCRIPTION_MIN_LENGTH, { message: `Description must be at least ${cfg.DIAGNOSIS_DESCRIPTION_MIN_LENGTH} characters` })
    .max(cfg.DIAGNOSIS_DESCRIPTION_MAX_LENGTH, { message: `Description must be at most ${cfg.DIAGNOSIS_DESCRIPTION_MAX_LENGTH} characters` }),
})

export const RemoveDiagnosisSchema = z.object({
  diagnosisId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid diagnosis ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid diagnosis ID" }),
})

export const PrescribeMedicationSchema = z.object({
  name: z.string()
    .min(cfg.PRESCRIPTION_NAME_MIN_LENGTH, { message: `Medication name must be at least ${cfg.PRESCRIPTION_NAME_MIN_LENGTH} characters` })
    .max(cfg.PRESCRIPTION_NAME_MAX_LENGTH, { message: `Medication name must be at most ${cfg.PRESCRIPTION_NAME_MAX_LENGTH} characters` }),

  dosage: z.string()
    .min(cfg.PRESCRIPTION_DOSAGE_MIN_LENGTH, { message: `Dosage must be at least ${cfg.PRESCRIPTION_DOSAGE_MIN_LENGTH} characters` })
    .max(cfg.PRESCRIPTION_DOSAGE_MAX_LENGTH, { message: `Dosage must be at most ${cfg.PRESCRIPTION_DOSAGE_MAX_LENGTH} characters` }),

  instructions: z.string()
    .min(cfg.PRESCRIPTION_INSTRUCTIONS_MIN_LENGTH, { message: `Instructions must be at least ${cfg.PRESCRIPTION_INSTRUCTIONS_MIN_LENGTH} characters` })
    .max(cfg.PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH, { message: `Instructions must be at most ${cfg.PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH} characters` }),
})

export const RemovePrescriptionSchema = z.object({
  prescriptionId: z.string()
    .min(cfg.ID_MIN_LENGTH, { message: "Invalid prescription ID" })
    .max(cfg.ID_MAX_LENGTH, { message: "Invalid prescription ID" }),
})

export const AddAddendumSchema = z.object({
  note: z.string()
    .min(cfg.ADDENDUM_NOTE_TEXT_MIN_LENGTH, { message: `Addendum must be at least ${cfg.ADDENDUM_NOTE_TEXT_MIN_LENGTH} characters` })
    .max(cfg.ADDENDUM_NOTE_TEXT_MAX_LENGTH, { message: `Addendum must be at most ${cfg.ADDENDUM_NOTE_TEXT_MAX_LENGTH} characters` }),
})

export const EncounterCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const NoteCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const DiagnosisCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const PrescriptionCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const AddendumCommandResponseSchema = z.object({
  id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})


export interface EncounterListItem {
  id: string
  startedAt: string
  status: EncounterStatus
  doctorId: string
  patientId: string
}

export interface AppointmentByIdResponse {
  appointmentById: {
    id: string
    start: string
    end: string
    status: AppointmentStatus
    doctorId: string
    patientId: string
    doctorName?: string
    patientName?: string
    encounterDetails: EncounterDetails
  }[]
}

export interface EncounterDetails {
  id: string
  startedAt: string
  finalizedAt?: string | null
  status: EncounterStatus
  notes: Note[]
  diagnoses: Diagnosis[]
  prescriptions: Prescription[]
  addendums: Addendum[]
}

export interface Diagnosis {
  id: string
  icdCode: string
  description: string
}

export interface Prescription {
  id: string
  name: string
  dosage: string
  instructions: string
}

export interface Addendum {
  id: string
  text: string
  createdAt: string
}

export enum EncounterStatus {
  InProgress = "IN_PROGRESS",
  Finalized = "FINALIZED",
  Locked = "LOCKED"
}

export interface Note {
  id: string
  text: string
  createdAt: string
}

export enum AppointmentStatus {
  Scheduled = "SCHEDULED",
  Rescheduled = "RESCHEDULED",
  Cancelled = "CANCELLED",
  Completed = "COMPLETED"
}

export interface Appointment {
  id: string
  start: string
  end: string
  status: AppointmentStatus
  doctorId: string
  patientId: string
  doctorName: string
  patientName: string
  encounterDetails: EncounterDetails
}

export interface MyPatientInfo {
  profile: PatientProfile
  appointments: Appointment[]
}

export interface PatientInfo {
  profile: PatientProfile
  appointments: Appointment[]
  pageInfo: {
    hasNextPage: boolean
    endCursor: string | null
  }
}

export interface Allergy {
  id: string
  substance: string
  reaction: string
}

export interface Condition {
  id: string
  name: string
}

export interface PatientProfile {
  id: string
  fullName: string
  birthDate: string
  allergies: Allergy[]
  conditions: Condition[]
}

export interface DashboardAppointment {
  id: string
  start: string
  end: string
  status: AppointmentStatus
  doctorId: string
  doctorName: string
}

export interface DashboardPrescription {
  id: string
  medicationName: string
  dosage: string
  instructions: string
  createdAt: string
  deletedAt?: string | null
}

export interface DashboardMedicalNote {
  id: string
  text: string
  createdAt: string
  deletedAt?: string | null
}

export interface DashboardAddendum {
  id: string
  text: string
  createdAt: string
  deletedAt?: string | null
}

export interface DashboardDiagnosis {
  id: string
  icdCode: string
  description: string
  createdAt: string
  deletedAt?: string | null
}

export interface DashboardEncounter {
  id: string
  startedAt: string
  updatedAt: string
  status: string
  doctorId: string
  appointmentId: string

  prescriptions: DashboardPrescription[]
  notes: DashboardMedicalNote[]
  addendums: DashboardAddendum[]
  diagnoses: DashboardDiagnosis[]
}

export interface PatientDashboard {
  myPatientHeader: PatientProfile

  upcomingAppointment: {
    nodes: DashboardAppointment[]
  }

  lastAppointment: {
    nodes: DashboardAppointment[]
  }

  myEncounters: {
    nodes: DashboardEncounter[]
  }
}

export interface DoctorAppointment {
  id: string
  start: string
  end: string
  status: AppointmentStatus
  patientName: string
  patientId: string
}

export interface DoctorEncounterNote {
  id: string
  text: string
  createdAt: string
  deletedAt?: string | null
}

export interface DoctorEncounterDiagnosis {
  id: string
  icdCode: string
  description: string
  createdAt: string
  deletedAt?: string | null
}

export interface DoctorEncounterPrescription {
  id: string
  medicationName: string
  dosage: string
  instructions: string
  createdAt: string
  deletedAt?: string | null
}

export interface DoctorEncounterAddendum {
  id: string
  text: string
  createdAt: string
  deletedAt?: string | null
}

export interface DoctorEncounter {
  id: string
  appointmentId: string
  patientId: string
  doctorId: string
  patientName: string
  status: "IN_PROGRESS" | "FINALIZED" | "LOCKED"

  startedAt: string
  finalizedAt?: string | null
  updatedAt?: string | null

  notes: DoctorEncounterNote[]
  diagnoses: DoctorEncounterDiagnosis[]
  prescriptions: DoctorEncounterPrescription[]
  addendums: DoctorEncounterAddendum[]
}

export interface DoctorDashboard {
  appointmentsByDoctor: {
    nodes: DoctorAppointment[]
  }

  encountersByDoctor: {
    nodes: DoctorEncounter[]
  }
}

export interface DoctorDashboardView {
  todayAppointments: DoctorAppointment[]
  nextAppointment?: DoctorAppointment
  totalToday: number
  minutesUntilNext?: number
}

export interface TodaySummary {
  nextAppointment?: DoctorAppointment
  todayAppointments: DoctorAppointment[]
  minutesUntilNext?: number
  totalToday: number
}

export interface DoctorWorkQueue {
  unfinishedEncounters: DoctorEncounter[]
}

export interface DoctorEncounterConnectionNode {
  id: string
  appointmentId: string
  patientId: string
  patientName: string
  status: "IN_PROGRESS" | "FINALIZED" | "LOCKED"

  startedAt: string
  finalizedAt?: string | null
  updatedAt?: string | null
}

export interface DoctorEncounterConnection {
  nodes: DoctorEncounterConnectionNode[]
  totalCount: number
  pageInfo: {
    hasNextPage: boolean
    endCursor: string | null
  }
}

export type StartEncounterRequest = z.infer<typeof StartEncounterSchema>
export type EncounterCommandResponse = z.infer<typeof EncounterCommandResponseSchema>
export type AddNoteRequest = z.infer<typeof AddNoteSchema>
export type RemoveNoteRequest = z.infer<typeof RemoveNoteSchema>
export type AddDiagnosisRequest = z.infer<typeof AddDiagnosisSchema>
export type RemoveDiagnosisRequest = z.infer<typeof RemoveDiagnosisSchema>
export type PrescribeMedicationRequest = z.infer<typeof PrescribeMedicationSchema>
export type RemovePrescriptionRequest = z.infer<typeof RemovePrescriptionSchema>
export type AddAddendumRequest = z.infer<typeof AddAddendumSchema>

export type NoteCommandResponse = z.infer<typeof NoteCommandResponseSchema>
export type DiagnosisCommandResponse = z.infer<typeof DiagnosisCommandResponseSchema>
export type PrescriptionCommandResponse = z.infer<typeof PrescriptionCommandResponseSchema>
export type AddendumCommandResponse = z.infer<typeof AddendumCommandResponseSchema>

export type AddAllergyRequest = z.infer<typeof AddAllergySchema>
export type AddChronicConditionRequest = z.infer<typeof AddChronicConditionSchema>
export type RemoveAllergyRequest = z.infer<typeof RemoveAllergySchema>
export type RemoveConditionRequest = z.infer<typeof RemoveConditionSchema>
export type AllergyCommandResponse = z.infer<typeof AllergyCommandResponseSchema>
export type ConditionCommandResponse = z.infer<typeof ConditionCommandResponseSchema>