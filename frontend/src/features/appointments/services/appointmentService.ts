import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"
import {
  CreateAppointmentRequest,
  AppointmentResponse,
  GetBookingsByDoctorAndDateRequest,
  BookingQueryResponse,
  RescheduleAppointmentRequest
} from "@/features/appointments/types/appointmentsTypes"

export const appointmentService = {
  createAppointment: (data: CreateAppointmentRequest) =>
    api.post<APIResponse<AppointmentResponse>>(
      ENDPOINTS.appointments.root,
      data
    ),

  getAppointmentsByDoctor: (
    doctorUserId: string,
    request: GetBookingsByDoctorAndDateRequest
  ) =>
    api.get<APIResponse<BookingQueryResponse[]>>(
      ENDPOINTS.appointments.byDoctor(doctorUserId),
      { params: request }
    ),

  cancelAppointment: (id: string) =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.appointments.byId(id)
    ),

  cancelAppointmentByAdmin: (id: string) =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.appointments.byIdAdmin(id)
    ),

  rescheduleAppointment: (
    id: string,
    data: RescheduleAppointmentRequest
  ) =>
    api.put<APIResponse<AppointmentResponse>>(
      ENDPOINTS.appointments.byId(id),
      data
    ),

  rescheduleAppointmentByAdmin: (
    id: string,
    data: RescheduleAppointmentRequest
  ) =>
    api.put<APIResponse<AppointmentResponse>>(
      ENDPOINTS.appointments.byIdAdmin(id),
      data
    ),

  getMyAppointments: (params: {
    startDate?: string
    endDate?: string
  }) =>
    api.get<APIResponse<AppointmentResponse[]>>(
      ENDPOINTS.appointments.mine,
      { params }
    ),

  getByDateAdmin: (
    userId: string,
    params: {
      startDate?: string
      endDate?: string
    }
  ) =>
    api.get<APIResponse<AppointmentResponse[]>>(
      ENDPOINTS.appointments.byUserIdAdmin(userId),
      { params }
    ),
}