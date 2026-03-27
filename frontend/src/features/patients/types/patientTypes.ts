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
  encounters: EncounterListItem[];
}

export interface PatientDashboard {
  profile: PatientProfile
  appointments: Appointment[];
}

export type AddAllergyRequest = z.infer<typeof AddAllergySchema>
export type AddChronicConditionRequest = z.infer<typeof AddChronicConditionSchema>
export type RemoveAllergyRequest = z.infer<typeof RemoveAllergySchema>
export type RemoveConditionRequest = z.infer<typeof RemoveConditionSchema>
export type AllergyCommandResponse = z.infer<typeof AllergyCommandResponseSchema>
export type ConditionCommandResponse = z.infer<typeof ConditionCommandResponseSchema>