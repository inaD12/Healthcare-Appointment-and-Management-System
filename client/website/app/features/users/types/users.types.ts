export interface LoginUserRequest{
    email: string;
    password: string;
}

export interface LoginUserResponse {
    token: string;
}