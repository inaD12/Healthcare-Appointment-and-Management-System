import keycloak from "@/config/keycloak"

export const login = () => keycloak.login()

export const logout = () => keycloak.logout()