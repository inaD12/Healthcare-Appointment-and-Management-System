import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { GetAllUsersRequest, RegisterUserByAdminRequest, RegisterUserRequest,UpdateUserRequest,UserCommandResponse, UserPaginatedResponse, UserQueryResponse } from "../types/userTypes";
import { APIResponse } from "@/types/types";

export const registerUser = (data: RegisterUserRequest) =>
  api.post<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.root,
     data
  );

export const registerUserByAdmin = (data: RegisterUserByAdminRequest) =>
  api.post<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.admin,
     data
  );

export const getCurrentUser = () =>
  api.get<APIResponse<UserQueryResponse>>(
    ENDPOINTS.users.me
  );

export const updateCurrentUser = (request: UpdateUserRequest) =>
  api.put<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.me,
    request
  );

export const deleteCurrentUser = () =>
  api.delete<APIResponse<boolean>>(
    ENDPOINTS.users.me
  );

export const deleteUserByAdmin = (id: string) =>
  api.delete<APIResponse<boolean>>(
    ENDPOINTS.users.adminById(id)
  );

export const updateUserByAdmin = (request: UpdateUserRequest, id: string) =>
  api.put<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.adminById(id),
    request
  );

export const getUserByAdmin = (id: string) =>
  api.get<APIResponse<UserQueryResponse>>(
    ENDPOINTS.users.adminById(id)
  );

export const getAllUsers = (request: GetAllUsersRequest) =>
  api.get<APIResponse<UserPaginatedResponse>>(
    ENDPOINTS.users.admin,
    {
      params: request
    }
  );