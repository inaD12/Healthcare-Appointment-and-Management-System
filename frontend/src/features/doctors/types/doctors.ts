import * as z from "zod"
import { DoctorsBusinessConfiguration as C } from "../config/business"


export const workTimeRangeSchema = z.object({
  start: z.string(),
  end: z.string(),
})

export const workDaySchema = z.object({
  dayOfWeek: z.number(),
  workTimes: z.array(workTimeRangeSchema),
})

export const doctorAvailabilityExceptionSchema = z.object({
  start: z.string(),
  end: z.string(),
  reason: z
    .string()
    .max(C.REASON_MAX_LENGTH, `Reason must be at most ${C.REASON_MAX_LENGTH} characters`),
  type: z.number(),
})


export const doctorSchema = z.object({
  id: z
    .string()
    .min(C.ID_MIN_LENGTH)
    .max(C.ID_MAX_LENGTH),

  firstName: z
    .string()
    .min(C.FIRSTNAME_MIN_LENGTH, `First name must be at least ${C.FIRSTNAME_MIN_LENGTH} characters`)
    .max(C.FIRSTNAME_MAX_LENGTH, `First name must be at most ${C.FIRSTNAME_MAX_LENGTH} characters`),

  lastName: z
    .string()
    .min(C.LASTNAME_MIN_LENGTH, `Last name must be at least ${C.LASTNAME_MIN_LENGTH} characters`)
    .max(C.LASTNAME_MAX_LENGTH, `Last name must be at most ${C.LASTNAME_MAX_LENGTH} characters`),

  userId: z.string(),

  bio: z
    .string()
    .min(C.BIO_MIN_LENGTH, `Bio must be at least ${C.BIO_MIN_LENGTH} characters`)
    .max(C.BIO_MAX_LENGTH, `Bio must be at most ${C.BIO_MAX_LENGTH} characters`),

  timeZoneId: z.string(),

  specialities: z.array(
    z
      .string()
      .min(C.SPECIALITY_MIN_LENGTH)
      .max(C.SPECIALITY_MAX_LENGTH)
  ),

  workDays: z.array(workDaySchema),

  availabilityExceptions: z.array(doctorAvailabilityExceptionSchema),

  averageRating: z.number().optional().default(0),

  ratingsCount: z.number().optional().default(0),
})


export const doctorPaginatedResponseSchema = z.object({
  items: z.array(doctorSchema),
  page: z.number(),
  pageSize: z.number(),
  totalCount: z.number(),
  hasNextPage: z.boolean(),
  hasPreviousPage: z.boolean(),
})

export const getAllDoctorsSchema = z.object({
  firstName: z
    .string()
    .max(C.FIRSTNAME_MAX_LENGTH)
    .optional()
    .default(""),

  lastName: z
    .string()
    .max(C.LASTNAME_MAX_LENGTH)
    .optional()
    .default(""),

  speciality: z
    .string()
    .max(C.SPECIALITY_MAX_LENGTH)
    .optional()
    .default(""),

  timeZoneId: z.string().optional().default(""),

  sortOrder: z.enum(["ASC", "DESC"]).default("ASC"),

  sortPropertyName: z.string().default("Id"),

  page: z.number().default(1),

  pageSize: z.number().default(10),
})

export const recommendSpecialitySchema = z.object({
  Symptoms: z
    .string()
    .min(C.SYMPTOMS_MIN_LENGTH, `Symptoms must be at least ${C.SYMPTOMS_MIN_LENGTH} characters`)
    .max(C.SYMPTOMS_MAX_LENGTH, `Symptoms must be at most ${C.SYMPTOMS_MAX_LENGTH} characters`),
})


export type WorkTimeRangeDto = z.infer<typeof workTimeRangeSchema>

export type WorkDayDto = z.infer<typeof workDaySchema>

export type DoctorAvailabilityExceptionDto = z.infer<
  typeof doctorAvailabilityExceptionSchema
>

export type DoctorQueryViewModel = z.infer<typeof doctorSchema>

export type DoctorPaginatedQueryResponse = z.infer<
  typeof doctorPaginatedResponseSchema
>

export type GetAllDoctorsRequest = z.infer<typeof getAllDoctorsSchema>

export type RecommendSpecialityRequest = z.infer<typeof recommendSpecialitySchema>

export interface RecommendSpecialityResponse {
    specialities: { name: string }[]
}