"use client"

import { ReactNode, useState, JSX } from "react"
import { Home, Calendar, User, Settings, LogIn, LogOut, UserPlus } from "lucide-react"
import Link from "next/link"
import { useRouter } from "next/navigation"
import { useAuth } from "@/features/auth/hooks/useAuth"

interface SideBarProps {
  children: ReactNode
}

export default function SideBar({ children }: SideBarProps) {
  const [isExpanded, setIsExpanded] = useState(false)

  const { authenticated, roles, keycloak } = useAuth()

  const router = useRouter()

  const isAdmin = roles.includes("Admin")
  const isDoctor = roles.includes("Doctor")
  const isPatient = roles.includes("Patient")

  const handleLogin = () =>
    keycloak.login({ redirectUri: window.location.href })

  const handleRegister = () => router.push("/register")

  const handleLogout = () =>
    keycloak.logout({ redirectUri: window.location.origin })

  const navItems: { label: string; icon: JSX.Element; href: string }[] = []

  navItems.push({ label: "Home", icon: <Home size={20} />, href: "/" })

  if (isDoctor) {
    navItems.push({
      label: "Doctor Profile",
      icon: <User size={20} />,
      href: "/doctors/profile",
    })

    navItems.push({
      label: "Settings",
      icon: <Settings size={20} />,
      href: "/settings",
    })
  }

  if (isPatient) {
    navItems.push({
      label: "Doctors",
      icon: <Calendar size={20} />,
      href: "/doctors",
    })

    navItems.push({
      label: "Profile",
      icon: <User size={20} />,
      href: "/patient-info",
    })

    navItems.push({
      label: "Settings",
      icon: <Settings size={20} />,
      href: "/settings",
    })
  }

  if (isAdmin) {
    navItems.push({
    label: "Create User",
    icon: <UserPlus size={20} />,
    href: "/admin/users/create",
  })

    navItems.push({
      label: "Settings",
      icon: <Settings size={20} />,
      href: "/settings",
    })
  }

  return (
    <div className="flex min-h-screen">
      <aside
        className={`flex flex-col justify-between bg-gray-900 text-white transition-all duration-300 sticky top-0 h-screen
        ${isExpanded ? "w-64" : "w-16"}`}
        onMouseEnter={() => setIsExpanded(true)}
        onMouseLeave={() => setIsExpanded(false)}
      >
        <div className="flex flex-col mt-4 space-y-2">
          {navItems.map((item) => (
            <Link
              key={item.label}
              href={item.href}
              className="flex items-center px-4 py-3 hover:bg-gray-800 rounded-md transition-all duration-200"
            >
              <div className="flex-shrink-0">{item.icon}</div>

              <span
                className={`ml-3 text-sm font-medium whitespace-nowrap transition-all duration-300
                ${isExpanded ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-4"}`}
              >
                {item.label}
              </span>
            </Link>
          ))}
        </div>

        <div className="mb-4 px-2">
          <div className="border-t border-gray-700 my-3"></div>

          {authenticated ? (
            <button
              onClick={handleLogout}
              className="flex items-center w-full px-3 py-3 rounded-md transition-all duration-200 hover:bg-red-500/20 text-red-400 hover:text-red-300"
            >
              <LogOut size={20} />
              <span
                className={`ml-3 text-sm font-medium whitespace-nowrap transition-all duration-300
                ${isExpanded ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-4"}`}
              >
                Logout
              </span>
            </button>
          ) : (
            <div className="flex flex-col gap-2">
              <button
                onClick={handleLogin}
                className="flex items-center w-full px-3 py-3 rounded-md transition-all duration-200 hover:bg-blue-500/20 text-blue-400 hover:text-blue-300"
              >
                <LogIn size={20} />
                <span
                  className={`ml-3 text-sm font-medium whitespace-nowrap transition-all duration-300
                  ${isExpanded ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-4"}`}
                >
                  Login
                </span>
              </button>

              <button
                onClick={handleRegister}
                className="flex items-center w-full px-3 py-3 rounded-md transition-all duration-200 hover:bg-green-500/20 text-green-400 hover:text-green-300"
              >
                <UserPlus size={20} />
                <span
                  className={`ml-3 text-sm font-medium whitespace-nowrap transition-all duration-300
                  ${isExpanded ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-4"}`}
                >
                  Register
                </span>
              </button>
            </div>
          )}
        </div>
      </aside>

      <main className="flex-1 p-6 bg-gray-50">{children}</main>
    </div>
  )
}