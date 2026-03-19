import * as z from "zod"
import { AppointmentsBusinessConfiguration } from "../config/business"

export const createAppointmentSchema = z.object({
  doctorUserId: z
    .string()
    .min(AppointmentsBusinessConfiguration.DOCTORID_MIN_LENGTH)
    .max(AppointmentsBusinessConfiguration.DOCTORID_MAX_LENGTH),

  scheduledStartTime: z.string(),

  duration: z.union([
    z.literal(15),
    z.literal(30),
    z.literal(60),
  ]),
})

export type CreateAppointmentRequest = z.infer<typeof createAppointmentSchema>


export const appointmentResponseSchema = z.object({
  id: z.string().min(AppointmentsBusinessConfiguration.ID_MIN_LENGTH),
  doctorId: z.string(),
  patientId: z.string(),
  start: z.string(),
  end: z.string(),
  status: z.enum(["Scheduled", "BooRescheduledked", "Cancelled", "Completed"]),
})

export type AppointmentResponse = z.infer<typeof appointmentResponseSchema>


export const appointmentPaginatedResponseSchema = z.object({
  items: z.array(appointmentResponseSchema),
  page: z.number(),
  pageSize: z.number(),
  totalCount: z.number(),
  hasNextPage: z.boolean(),
  hasPreviousPage: z.boolean(),
})

export type AppointmentPaginatedResponse = z.infer<typeof appointmentPaginatedResponseSchema>