import { api } from "@/lib/api/axios"
import {
  CreateAppointmentRequest,
  AppointmentResponse,
  GetBookingsByDoctorAndDateRequest,
  BookingQueryResponse,
  RescheduleAppointmentRequest
} from "@/features/appointments/types/appointmentsTypes"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"


export const createAppointment = (data: CreateAppointmentRequest) =>
  api.post<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.root, data)

export const getAppointmentById = (id: string) =>
  api.get<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.byId(id))

export const getAppointmentsByDoctor = (
  doctorUserId: string,
  data: GetBookingsByDoctorAndDateRequest
) =>
  api.get<APIResponse<BookingQueryResponse[]>>(
    ENDPOINTS.appointments.byDoctor(doctorUserId),
    { params: data }
  )

export const cancelAppointment = (id: string) =>
  api.delete<APIResponse<boolean>>(ENDPOINTS.appointments.byId(id))

export const rescheduleAppointment = (id: string, data: RescheduleAppointmentRequest) =>
  api.put<APIResponse<AppointmentResponse>>(ENDPOINTS.appointments.byId(id), data)

export const getMyAppointments = (params: {
  startDate?: string
  endDate?: string
}) =>
  api.get<APIResponse<AppointmentResponse[]>>(
    ENDPOINTS.appointments.mine,
    { params }
  )