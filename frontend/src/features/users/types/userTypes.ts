import * as z from "zod"
import { UsersBusinessConfiguration as C } from "../config/business"

export type Roles = "Patient" | "Doctor" | "Admin";

export const ROLES = {
  ADMIN: "Admin",
  DOCTOR: "Doctor",
  PATIENT: "Patient",
} as const

export type UserQueryResponse = {
  id: string;
  email: string;
  roles: Roles[];
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  emailVerified: boolean;
};

export type UserCommandResponse = {
  id: string
}

export type UserPaginatedResponse = {
  items: UserQueryResponse[]
  page: number
  pageSize: number
  totalCount: number
  hasNextPage: boolean
  hasPreviousPage: boolean
};


export const registerUserByAdminSchema = z.object({
  email: z
    .string()
    .min(C.EMAIL_MIN_LENGTH, `Email must be at least ${C.EMAIL_MIN_LENGTH} characters`)
    .max(C.EMAIL_MAX_LENGTH, `Email must be at most ${C.EMAIL_MAX_LENGTH} characters`)
    .email({ message: "Invalid email address" }),

  password: z
    .string()
    .min(C.PASSWORD_MIN_LENGTH, `Password must be at least ${C.PASSWORD_MIN_LENGTH} characters`)
    .max(C.PASSWORD_MAX_LENGTH, `Password must be at most ${C.PASSWORD_MAX_LENGTH} characters`),

  firstName: z
    .string()
    .min(C.FIRSTNAME_MIN_LENGTH, `First name must be at least ${C.FIRSTNAME_MIN_LENGTH} characters`)
    .max(C.FIRSTNAME_MAX_LENGTH, `First name must be at most ${C.FIRSTNAME_MAX_LENGTH} characters`),

  lastName: z
    .string()
    .min(C.LASTNAME_MIN_LENGTH, `Last name must be at least ${C.LASTNAME_MIN_LENGTH} characters`)
    .max(C.LASTNAME_MAX_LENGTH, `Last name must be at most ${C.LASTNAME_MAX_LENGTH} characters`),

  dateOfBirth: z.string().min(1, "Date of birth is required"),

  phoneNumber: z
    .string()
    .min(C.PHONENUMBER_MIN_LENGTH, `Phone number must be at least ${C.PHONENUMBER_MIN_LENGTH} characters`)
    .max(C.PHONENUMBER_MAX_LENGTH, `Phone number must be at most ${C.PHONENUMBER_MAX_LENGTH} characters`),

  address: z
    .string()
    .min(C.ADDRESS_MIN_LENGTH, `Address must be at least ${C.ADDRESS_MIN_LENGTH} characters`)
    .max(C.ADDRESS_MAX_LENGTH, `Address must be at most ${C.ADDRESS_MAX_LENGTH} characters`),

  role: z.enum(["Patient", "Doctor", "Admin"], "Select a role"),
})

export const registerUserSchema = z.object({
  email: z
    .string()
    .min(C.EMAIL_MIN_LENGTH, `Email must be at least ${C.EMAIL_MIN_LENGTH} characters`)
    .max(C.EMAIL_MAX_LENGTH, `Email must be at most ${C.EMAIL_MAX_LENGTH} characters`)
    .email({ message: "Invalid email address" }),

  password: z
    .string()
    .min(C.PASSWORD_MIN_LENGTH, `Password must be at least ${C.PASSWORD_MIN_LENGTH} characters`)
    .max(C.PASSWORD_MAX_LENGTH, `Password must be at most ${C.PASSWORD_MAX_LENGTH} characters`),

  firstName: z
    .string()
    .min(C.FIRSTNAME_MIN_LENGTH, `First name must be at least ${C.FIRSTNAME_MIN_LENGTH} characters`)
    .max(C.FIRSTNAME_MAX_LENGTH, `First name must be at most ${C.FIRSTNAME_MAX_LENGTH} characters`),

  lastName: z
    .string()
    .min(C.LASTNAME_MIN_LENGTH, `Last name must be at least ${C.LASTNAME_MIN_LENGTH} characters`)
    .max(C.LASTNAME_MAX_LENGTH, `Last name must be at most ${C.LASTNAME_MAX_LENGTH} characters`),

  dateOfBirth: z.string().min(1, "Date of birth is required"),

  phoneNumber: z
    .string()
    .min(C.PHONENUMBER_MIN_LENGTH, `Phone number must be at least ${C.PHONENUMBER_MIN_LENGTH} characters`)
    .max(C.PHONENUMBER_MAX_LENGTH, `Phone number must be at most ${C.PHONENUMBER_MAX_LENGTH} characters`),

  address: z
    .string()
    .min(C.ADDRESS_MIN_LENGTH, `Address must be at least ${C.ADDRESS_MIN_LENGTH} characters`)
    .max(C.ADDRESS_MAX_LENGTH, `Address must be at most ${C.ADDRESS_MAX_LENGTH} characters`),
})

export const getAllUsersSchema = z.object({
  email: z
    .string()
    .optional()
    .default(""),

  role: z.enum(["Patient", "Doctor", "Admin"])
    .optional(),

  firstName: z
    .string()
    .max(C.FIRSTNAME_MAX_LENGTH, `First name must be at most ${C.FIRSTNAME_MAX_LENGTH} characters`)
    .optional()
    .default(""),

  lastName: z
    .string()
    .max(C.LASTNAME_MAX_LENGTH, `Last name must be at most ${C.LASTNAME_MAX_LENGTH} characters`)
    .optional()
    .default(""),

  phoneNumber: z
    .string()
    .max(C.PHONENUMBER_MAX_LENGTH, `Phone number must be at most ${C.PHONENUMBER_MAX_LENGTH} characters`)
    .optional()
    .default(""),

  address: z
    .string()
    .max(C.ADDRESS_MAX_LENGTH, `Address must be at most ${C.ADDRESS_MAX_LENGTH} characters`)
    .optional()
    .default(""),
  
  emailVerified:z
    .boolean()
    .optional(),

  sortOrder: z.enum(["ASC", "DESC"]).default("ASC"),
  
  sortPropertyName: z.string().default("Id"),
  
  page: z.number().default(1),
  
  pageSize: z.number().default(10),
})

export const updateUserSchema = z
  .object({
    firstName: z
      .string()
      .min(C.FIRSTNAME_MIN_LENGTH, `First name must be at least ${C.FIRSTNAME_MIN_LENGTH} characters`)
      .max(C.FIRSTNAME_MAX_LENGTH, `First name must be at most ${C.FIRSTNAME_MAX_LENGTH} characters`)
      .optional(),

    lastName: z
      .string()
      .min(C.LASTNAME_MIN_LENGTH, `Last name must be at least ${C.LASTNAME_MIN_LENGTH} characters`)
      .max(C.LASTNAME_MAX_LENGTH, `Last name must be at most ${C.LASTNAME_MAX_LENGTH} characters`)
      .optional(),
  })
  .refine(
    (data) => !!data.firstName || !!data.lastName,
    {
      message: "At least one field must be provided",
    }
  );

export type RegisterFormValues = z.infer<typeof registerUserSchema>
export type RegisterByAdminFormValues = z.infer<typeof registerUserByAdminSchema>
export type RegisterUserRequest = z.infer<typeof registerUserSchema>
export type RegisterUserByAdminRequest = z.infer<typeof registerUserByAdminSchema>
export type UpdateUserRequest = z.infer<typeof updateUserSchema>
export type GetAllUsersRequest = z.infer<typeof getAllUsersSchema>