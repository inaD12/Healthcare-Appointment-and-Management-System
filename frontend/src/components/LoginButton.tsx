"use client"

import keycloak from "@/config/keycloak";
import { useEffect } from "react";

interface LoginButtonProps {
  redirectUri?: string;
}

export default function LoginButton({ redirectUri }: LoginButtonProps) {
  const handleLogin = () => {
    keycloak.login({
      redirectUri: redirectUri ?? window.location.href,
    });
  };

  useEffect(() => {
    keycloak.init({ onLoad: "check-sso", pkceMethod: "S256" }).then((auth) => {
      if (auth) console.log("Already logged in");
    });
  }, []);

  return (
    <button
      onClick={handleLogin}
      className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 transition"
    >
      Login
    </button>
  );
}