import {
  createContext,
  useContext,
  useEffect,
  useState,
  type ReactNode
} from "react";
import { tokenService } from "~/utilities/token.service";

interface AuthContextType {
  isAuthenticated: boolean;
  login: (token: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const isValid = tokenService.isTokenValid();
    setIsAuthenticated(isValid);
  }, []);

  const login = (token: string) => {
    tokenService.setToken(token);
    setIsAuthenticated(true);
  };

  const logout = () => {
    tokenService.removeToken();
    setIsAuthenticated(false);
    if (typeof window !== "undefined") {
      window.location.href = "/login";
    }
  };

  const checkAuth = () => {
    const isValid = tokenService.isTokenValid();
    if (!isValid && isAuthenticated) {
      logout();
    }
  };

  useEffect(() => {
    checkAuth();

    const interval = setInterval(checkAuth, 30 * 1000);

    const handleStorageChange = (e: StorageEvent) => {
      if (e.key === "auth_token") {
        checkAuth();
      }
    };

    window.addEventListener("storage", handleStorageChange);

    return () => {
      clearInterval(interval);
      window.removeEventListener("storage", handleStorageChange);
    };
  }, [isAuthenticated]);

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
