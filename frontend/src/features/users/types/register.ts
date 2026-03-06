import * as z from "zod"
import { UsersBusinessConfiguration as C } from "../config/business"

export type RegisterUserRequest = {
  email: string
  password: string
  firstName: string
  lastName: string
  dateOfBirth: Date
  phoneNumber: string
  address: string
  role: "Patient" | "Doctor" | "Admin"
}

export type UserCommandResponse = {
  id: string
}

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

  role: z.enum(["Patient", "Doctor", "Admin"], "Select a role"),
})

export type RegisterFormValues = z.infer<typeof registerUserSchema>