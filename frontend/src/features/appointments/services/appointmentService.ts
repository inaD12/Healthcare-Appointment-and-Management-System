import { api } from "@/lib/api/axios"
import {
  CreateAppointmentRequest,
  AppointmentResponse
} from "@/features/appointments/types/appointmentsTypes"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"


export const createAppointment = (data: CreateAppointmentRequest) =>
  api.post<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.root, data)

export const getMyAppointments = () =>
  api.get<APIResponse<AppointmentResponse[]>>(`${ENDPOINTS.appointments.root}/my`)

export const getAppointmentById = (id: string) =>
  api.get<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.byId(id))

export const cancelAppointment = (id: string) =>
  api.delete<APIResponse<boolean>>(ENDPOINTS.appointments.byId(id))

export const rescheduleAppointment = (id: string, data: { start: string; end: string }) =>
  api.put<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.byId(id), data)