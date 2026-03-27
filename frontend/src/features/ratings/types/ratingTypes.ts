import { z } from "zod"
import { RatingsBusinessConfiguration } from "../config/business"
import { PaginatedQueryResponseSchema } from "@/types/types"

export const GetAllRatingsByDoctorRequestDefaults = {
  PatientId: "",
  AppointmentId: "",
  MinScore: null as number | null,
  MaxScore: null as number | null,
  SortOrder: "ASC" as "ASC" | "DESC",
  SortPropertyName: "CreatedAt",
  Page: 1,
  PageSize: 10,
}

export const GetAllRatingsByDoctorRequestSchema = z.object({
  PatientId: z.string()
    .min(RatingsBusinessConfiguration.ID_MIN_LENGTH)
    .max(RatingsBusinessConfiguration.ID_MAX_LENGTH)
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.PatientId),

  AppointmentId: z.string()
    .min(RatingsBusinessConfiguration.ID_MIN_LENGTH)
    .max(RatingsBusinessConfiguration.ID_MAX_LENGTH)
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.AppointmentId),

  MinScore: z.number()
    .min(RatingsBusinessConfiguration.MIN_RATING_SCORE)
    .max(RatingsBusinessConfiguration.MAX_RATING_SCORE)
    .nullable()
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.MinScore),

  MaxScore: z.number()
    .min(RatingsBusinessConfiguration.MIN_RATING_SCORE)
    .max(RatingsBusinessConfiguration.MAX_RATING_SCORE)
    .nullable()
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.MaxScore),

  SortOrder: z.enum(["ASC", "DESC"])
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.SortOrder),

  SortPropertyName: z.string()
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.SortPropertyName),

  Page: z.number()
    .int()
    .min(1)
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.Page),

  PageSize: z.number()
    .int()
    .min(1)
    .optional()
    .default(GetAllRatingsByDoctorRequestDefaults.PageSize),
})

export const AddRatingRequestSchema = z.object({
  AppointmentId: z.string()
    .min(RatingsBusinessConfiguration.ID_MIN_LENGTH)
    .max(RatingsBusinessConfiguration.ID_MAX_LENGTH),

  Score: z.number()
    .min(RatingsBusinessConfiguration.MIN_RATING_SCORE)
    .max(RatingsBusinessConfiguration.MAX_RATING_SCORE),

  Comment: z.string()
    .min(RatingsBusinessConfiguration.MAX_COMMENT_LENGTH),
})

export const RatingQueryViewModelSchema = z.object({
  id: z.string(),
  doctorId: z.string(),
  patientId: z.string(),
  appointmentId: z.string(),
  score: z.number().int(),
  createdAt: z.string(),
  comment: z.string().nullable().optional(),
})

export const RatingCommandResponseSchema = z.object({
  Id: z.string(),
})

export const RatingPaginatedQueryResponseSchema = PaginatedQueryResponseSchema(RatingQueryViewModelSchema)
export type AddRatingRequest = z.infer<typeof AddRatingRequestSchema>
export type RatingCommandResponse = z.infer<typeof RatingCommandResponseSchema>
export type RatingQueryViewModel = z.infer<typeof RatingQueryViewModelSchema>
export type RatingPaginatedQueryResponse = z.infer<typeof RatingPaginatedQueryResponseSchema>
export type GetAllRatingsByDoctorRequest = z.infer<typeof GetAllRatingsByDoctorRequestSchema>