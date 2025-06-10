"use client"

import type React from "react"

import { ThemeProvider } from "@/components/theme-provider"
import { UserProvider } from "@/contexts/user-context"
import { CatalogoProvider } from "@/contexts/catalogo-context"
import { AlertProvider } from "@/components/custom-alert"

export default function ClientLayout({ children }: { children: React.ReactNode }) {
  return (
    <ThemeProvider attribute="class" defaultTheme="light" enableSystem>
      <AlertProvider>
        <UserProvider>
          <CatalogoProvider>{children}</CatalogoProvider>
        </UserProvider>
      </AlertProvider>
    </ThemeProvider>
  )
}
