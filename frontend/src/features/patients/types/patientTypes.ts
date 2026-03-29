import { z } from "zod"
import { PatientsBusinessConfiguration as cfg } from "../config/business"


export const AddAllergySchema = z.object({
  Substance: z.string()
    .min(cfg.SUBSTANCE_MIN_LENGTH)
    .max(cfg.SUBSTANCE_MAX_LENGTH),

  Reaction: z.string()
    .min(cfg.REACTION_MIN_LENGTH)
    .max(cfg.REACTION_MAX_LENGTH),
})

export const AddChronicConditionSchema = z.object({
  Name: z.string()
    .min(cfg.CHRONIC_CONDITION_NAME_MIN_LENGTH)
    .max(cfg.CHRONIC_CONDITION_NAME_MAX_LENGTH),
})

export const RemoveAllergySchema = z.object({
  AllergyId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const RemoveConditionSchema = z.object({
  ConditionId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const AllergyCommandResponseSchema = z.object({
  Id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const ConditionCommandResponseSchema = z.object({
  Id: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const StartEncounterSchema = z.object({
  appointmentId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const AddNoteSchema = z.object({
  note: z.string()
    .min(cfg.CLINICAL_NOTE_TEXT_MIN_LENGTH)
    .max(cfg.CLINICAL_NOTE_TEXT_MAX_LENGTH),
})

export const RemoveNoteSchema = z.object({
  noteId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const AddDiagnosisSchema = z.object({
  icdCode: z.string()
    .min(cfg.ICD_MIN_LENGTH)
    .max(cfg.ICD_MAX_LENGTH),
  description: z.string()
    .min(cfg.DIAGNOSIS_DESCRIPTION_MIN_LENGTH)
    .max(cfg.DIAGNOSIS_DESCRIPTION_MAX_LENGTH),
})

export const RemoveDiagnosisSchema = z.object({
  diagnosisId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const PrescribeMedicationSchema = z.object({
  name: z.string()
    .min(cfg.PRESCRIPTION_NAME_MIN_LENGTH)
    .max(cfg.PRESCRIPTION_NAME_MAX_LENGTH),
  dosage: z.string()
    .min(cfg.PRESCRIPTION_DOSAGE_MIN_LENGTH)
    .max(cfg.PRESCRIPTION_DOSAGE_MAX_LENGTH),
  instructions: z.string()
    .min(cfg.PRESCRIPTION_INSTRUCTIONS_MIN_LENGTH)
    .max(cfg.PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH),
})

export const RemovePrescriptionSchema = z.object({
  prescriptionId: z.string()
    .min(cfg.ID_MIN_LENGTH)
    .max(cfg.ID_MAX_LENGTH),
})

export const AddAddendumSchema = z.object({
  note: z.string()
    .min(cfg.ADDENDUM_NOTE_TEXT_MIN_LENGTH)
    .max(cfg.ADDENDUM_NOTE_TEXT_MAX_LENGTH),
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


export interface PatientProfile {
  id: string
  fullName: string
  birthDate: string
  allergies: string[]
  conditions: string[]
}

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
    encounterDetails: EncounterDetails[]
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
  medicationName: string
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
  id: string;
  start: string;
  end: string;
  status: AppointmentStatus;
  doctorId: string;
  patientId: string;
  doctorName: string;
  encounterDetails: EncounterDetails;
}

export interface PatientDashboard {
  profile: PatientProfile
  appointments: Appointment[];
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