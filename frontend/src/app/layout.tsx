import '@/styles/globals.css'
import type { Metadata } from "next"
import { Geist, Geist_Mono } from "next/font/google"
import { AppProviders } from "../providers/AppProviders"
import SideBar from '@/components/sideBar/SideBar'
import { Toaster } from "sonner"

const geistSans = Geist({
  variable: "--font-geist-sans",
  subsets: ["latin"],
})

const geistMono = Geist_Mono({
  variable: "--font-geist-mono",
  subsets: ["latin"],
})

export const metadata: Metadata = {
  title: "Mediflow",
  description: "A modern platform for managing doctor appointments and healthcare workflows."
}

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body className={`${geistSans.variable} ${geistMono.variable} antialiased`}>
        <AppProviders>
          <SideBar>
            {children}
          </SideBar>

          <Toaster richColors position="top-right" />
        </AppProviders>
      </body>
    </html>
  )
}