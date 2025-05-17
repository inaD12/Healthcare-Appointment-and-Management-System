import { useState } from "react";
import type {RegisterUserRequest } from "../types/users.types";
import userService from "../services/users.service";

const RegisterForm = () => {
    const [apiError, setApiError] = useState<string | null>(null);
    const [email, setEmail] = useState<string>("");
    const [password, setPassword] = useState<string>("");
    const [firstName, setFirstName] = useState<string>("");
    const [lastName, setLastName] = useState<string>("");
    const [dateOfBirth, setDateOfBirth] = useState<Date>(new Date());
    const [phoneNumber, setPhoneNumber] = useState<string>("");
    const [address, setAddress] = useState<string>("");

    const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setApiError(null);

        const data: RegisterUserRequest = { email, password, firstName, lastName, dateOfBirth, phoneNumber, address};

        try {
            const response = await userService.register(data);
            window.location.href = "/login";
        } catch (error: any) {
            const errorMessage =
                error.response?.data.message || "Something went wrong. Please try again.";
            setApiError(errorMessage);
        }
    };

    return (
        <form
            onSubmit={onSubmit}
            className="flex flex-col gap-4 bg-white w-screen h-screen sm:h-auto sm:min-w-80 sm:max-w-[30%] sm:rounded-xl justify-center text-center text-black p-6 outline-3 outline-blue-700"
        >
            <h1 className="text-4xl mb-6">Register</h1>

            <input
                type="text"
                placeholder="Email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="text"
                placeholder="First Name"
                value={firstName}
                onChange={(e) => setFirstName(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="text"
                placeholder="Last Name"
                value={lastName}
                onChange={(e) => setLastName(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="date"
                value={dateOfBirth ? dateOfBirth.toISOString().split("T")[0] : ""}
                onChange={(e) => setDateOfBirth(new Date(e.target.value))}
                className="bg-gray-300 p-2 rounded-md"
            />


            <input
                type="text"
                placeholder="Phone Number"
                value={phoneNumber}
                onChange={(e) => setPhoneNumber(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            <input
                type="text"
                placeholder="Address"
                value={address}
                onChange={(e) => setAddress(e.target.value)}
                className="bg-gray-300 p-2 rounded-md"
            />

            {apiError && <div className="text-red-700 text-sm">{apiError}</div>}

            <button
                type="submit"
                className="mx-auto bg-blue-500 text-white p-2 h-[50px] w-[150px] rounded-md hover:bg-blue-600 transition"
            >
                Submit
            </button>
        </form>
    );
};

export default RegisterForm;
