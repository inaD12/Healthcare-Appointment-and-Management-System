import { useState } from "react";
import type { LoginUserRequest } from "../types/users.types";
import userService from "../services/users.service";
import { useAuth } from "../contexts/AuthContext";

const Login = () => {
    const [apiError, setApiError] = useState<string | null>(null);
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const { login } = useAuth();

    const onSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setApiError(null);

        const data: LoginUserRequest = {
            email,
            password,
        };

        try {
            const response = await userService.login(data);
            login(response.data.token)
            console.log("Login successful:", response);
            window.location.href = "/";
        } catch (error: any) {
            const errorMessage =
                error.response?.data.message ||
                "Something went wrong. Please try again.";
            setApiError(errorMessage);

            console.error("Login failed:", error);
        }
    };

    return (
        <div className="flex items-center justify-center h-screen">
            <form
                onSubmit={onSubmit}
                className="flex flex-col gap-4 bg-white w-screen h-screen sm:h-auto sm:min-w-80 sm:max-w-[30%] sm:rounded-xl justify-center text-center text-black p-4 outline-3 outline-blue-700"
            >
                <h1 className="text-4xl my-10">Login</h1>

                <input
                    type="text"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    className="bg-gray-300 p-2 mb-3 rounded-md"
                />

                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    className="bg-gray-300 p-2 mb-3 rounded-md"
                />

              {apiError && <div className="text-red-700 text-x">{apiError}</div>}
                <button
                    type="submit"
                    className="mx-auto bg-blue-500 text-white p-2 h-[50px] w-[150px] rounded-md hover:bg-blue-600 transition"
                >
                    Submit
                </button>
            </form>
        </div>
    );
};

export default Login;
