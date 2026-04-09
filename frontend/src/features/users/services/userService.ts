import { api } from "@/lib/api/axios"
import { ENDPOINTS } from "@/config/endpoints"
import { RegisterUserRequest, UserCommandResponse, UserQueryResponse } from "../types/register";
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