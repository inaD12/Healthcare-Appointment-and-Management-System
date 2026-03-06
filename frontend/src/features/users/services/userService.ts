import { api } from "@/src/lib/api/axios"
import { ENDPOINTS } from "@/src/config/endpoints"
import { RegisterUserRequest, UserCommandResponse } from "../types/register";

export const registerUser = (data: RegisterUserRequest) =>
  api.post<UserCommandResponse>(ENDPOINTS.users.root, data)