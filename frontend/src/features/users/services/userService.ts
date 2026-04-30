import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { APIResponse } from "@/types/types"
import {
  GetAllUsersRequest,
  RegisterUserByAdminRequest,
  RegisterUserRequest,
  UpdateUserRequest,
  UserCommandResponse,
  UserPaginatedResponse,
  UserQueryResponse,
} from "../types/usersTypes"

export const userService = {
  registerUser: (data: RegisterUserRequest) =>
    api.post<APIResponse<UserCommandResponse>>(
      ENDPOINTS.users.root,
      data
    ),

  registerUserByAdmin: (data: RegisterUserByAdminRequest) =>
    api.post<APIResponse<UserCommandResponse>>(
      ENDPOINTS.users.admin,
      data
    ),

  getCurrentUser: () =>
    api.get<APIResponse<UserQueryResponse>>(
      ENDPOINTS.users.me
    ),

  updateCurrentUser: (request: UpdateUserRequest) =>
    api.put<APIResponse<UserCommandResponse>>(
      ENDPOINTS.users.me,
      request
    ),

  deleteCurrentUser: () =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.users.me
    ),

  deleteUserByAdmin: (id: string) =>
    api.delete<APIResponse<boolean>>(
      ENDPOINTS.users.adminById(id)
    ),

  updateUserByAdmin: (id: string, request: UpdateUserRequest) =>
    api.put<APIResponse<UserCommandResponse>>(
      ENDPOINTS.users.adminById(id),
      request
    ),

  getUserByAdmin: (id: string) =>
    api.get<APIResponse<UserQueryResponse>>(
      ENDPOINTS.users.adminById(id)
    ),

  getAllUsers: (request: GetAllUsersRequest) =>
    api.get<APIResponse<UserPaginatedResponse>>(
      ENDPOINTS.users.admin,
      {
        params: request,
      }
    ),
}