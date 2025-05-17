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

export interface RegisterUserRequest{
    email: string;
    password: string;
    firstName: string;
    lastName: string;
    dateOfBirth: Date;
    phoneNumber: string;
    address: string;
}

export interface RegisterUserResponse {
  message: string;
}
