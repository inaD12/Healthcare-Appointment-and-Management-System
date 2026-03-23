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

export type AddAllergyRequest = z.infer<typeof AddAllergySchema>
export type AddChronicConditionRequest = z.infer<typeof AddChronicConditionSchema>
export type RemoveAllergyRequest = z.infer<typeof RemoveAllergySchema>
export type RemoveConditionRequest = z.infer<typeof RemoveConditionSchema>