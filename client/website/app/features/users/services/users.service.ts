import apiClient from "../../../api/apiClient";
import apiRoutes from "../../../api/apiEndpoints.config";
import type { LoginUserRequest, LoginUserResponse, RegisterUserRequest, RegisterUserResponse } from "../types/users.types";

const userService = {
    login: async (data: LoginUserRequest): Promise<LoginUserResponse> => {
        const response = await apiClient.post<LoginUserResponse>(
            apiRoutes.users.login,
            data
        );
        return response.data;
    },

    register: async (data: RegisterUserRequest): Promise<RegisterUserResponse> => {
        const response = await apiClient.post<RegisterUserResponse>(
            apiRoutes.users.register,
            data
        );
        return response.data;
    }
}

export default userService;