export interface LoginUserRequest{
    email: string;
    password: string;
}

export interface LoginUserResponse {
  message: string;
  data: {
    token: string;
  };
}
