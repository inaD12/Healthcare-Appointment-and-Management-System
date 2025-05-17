import { type RouteConfig, index, route } from "@react-router/dev/routes";

export default [
    index("routes/home.tsx"),
    route("login", "routes/LoginRoute.tsx"),
    route("register", "routes/RegisterRoute.tsx")
] satisfies RouteConfig;