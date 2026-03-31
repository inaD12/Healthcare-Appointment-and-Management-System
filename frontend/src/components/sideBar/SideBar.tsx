"use client"

import { ReactNode, useState } from "react"
import { Home, Calendar, User, Settings } from "lucide-react"
import Link from "next/link"

interface SideBarProps {
  children: ReactNode
}

const navItems = [
  { label: "Home", icon: <Home size={20} />, href: "/" },
  { label: "Appointments", icon: <Calendar size={20} />, href: "/appointments" },
  { label: "Patients", icon: <User size={20} />, href: "/patients" },
  { label: "Settings", icon: <Settings size={20} />, href: "/settings" },
]

export default function SideBar({ children }: SideBarProps) {
  const [isExpanded, setIsExpanded] = useState(false)

  return (
    <div className="flex min-h-screen">
      <aside
        className={`flex flex-col bg-gray-900 text-white transition-all duration-300 sticky top-0 h-screen
          ${isExpanded ? "w-64" : "w-16"}`}
        onMouseEnter={() => setIsExpanded(true)}
        onMouseLeave={() => setIsExpanded(false)}
      >
        <div className="flex flex-col mt-4 space-y-4">
          {navItems.map((item) => (
            <Link
              key={item.label}
              href={item.href}
              className="flex items-center px-4 py-3 hover:bg-gray-700 rounded-md transition-colors duration-200"
            >
              <div className="flex-shrink-0">{item.icon}</div>
              <span
                className={`ml-3 text-sm font-medium whitespace-nowrap 
                  transition-all duration-300 ease-in-out
                  ${isExpanded ? "opacity-100 translate-x-0" : "opacity-0 -translate-x-4"}`}
              >
                {item.label}
              </span>
            </Link>
          ))}
        </div>
      </aside>

      <main className="flex-1 p-6 bg-gray-50">{children}</main>
    </div>
  )
}