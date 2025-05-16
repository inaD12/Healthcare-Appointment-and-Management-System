import { jwtDecode } from "jwt-decode";

const TOKEN_KEY = "auth_token";

export const tokenService = {
    setToken: (token: string) => {
        localStorage.setItem(TOKEN_KEY, token);
    },

    getToken: (): string | null => {
        return localStorage.getItem(TOKEN_KEY);
    },

    removeToken: () => {
        localStorage.removeItem(TOKEN_KEY);
    },

    isTokenValid: (): boolean => {
        const token = localStorage.getItem(TOKEN_KEY);
        if (!token) return false;

        try {
            const decoded: { exp: number } = jwtDecode(token);
            return decoded.exp * 1000 > Date.now();
        } catch (e) {
            return false;
        }
    }
};
