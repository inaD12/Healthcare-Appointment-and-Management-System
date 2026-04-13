import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { RegisterUserRequest,UpdateCurrentUserRequest,UserCommandResponse, UserQueryResponse } from "../types/userTypes";
import { APIResponse } from "@/types/types";

export const registerUser = (data: RegisterUserRequest) =>
  api.post<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.root,
     data
  );

export const getCurrentUser = () =>
  api.get<APIResponse<UserQueryResponse>>(
    ENDPOINTS.users.me
  );

export const updateCurrentUser = (request: UpdateCurrentUserRequest) =>
  api.put<APIResponse<UserCommandResponse>>(
    ENDPOINTS.users.me,
    request
  );

export const deleteCurrentUser = () =>
  api.delete<APIResponse<boolean>>(
    ENDPOINTS.users.me
  );