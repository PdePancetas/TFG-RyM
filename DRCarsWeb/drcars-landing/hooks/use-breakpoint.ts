"use client"

import { useState, useEffect } from "react"

type Breakpoint = "xs" | "sm" | "md" | "lg" | "xl" | "2xl"

const breakpointValues = {
  xs: 0,
  sm: 640,
  md: 768,
  lg: 1024,
  xl: 1280,
  "2xl": 1536,
}

export function useBreakpoint(breakpoint: Breakpoint): boolean {
  const [isAboveBreakpoint, setIsAboveBreakpoint] = useState(false)

  useEffect(() => {
    const checkBreakpoint = () => {
      setIsAboveBreakpoint(window.innerWidth >= breakpointValues[breakpoint])
    }

    // Comprobar al montar
    checkBreakpoint()

    // Comprobar al cambiar el tamaño de la ventana
    window.addEventListener("resize", checkBreakpoint)

    // Limpiar
    return () => window.removeEventListener("resize", checkBreakpoint)
  }, [breakpoint])

  return isAboveBreakpoint
}

export function useCurrentBreakpoint(): Breakpoint {
  const [currentBreakpoint, setCurrentBreakpoint] = useState<Breakpoint>("xs")

  useEffect(() => {
    const checkBreakpoint = () => {
      const width = window.innerWidth

      if (width >= breakpointValues["2xl"]) {
        setCurrentBreakpoint("2xl")
      } else if (width >= breakpointValues.xl) {
        setCurrentBreakpoint("xl")
      } else if (width >= breakpointValues.lg) {
        setCurrentBreakpoint("lg")
      } else if (width >= breakpointValues.md) {
        setCurrentBreakpoint("md")
      } else if (width >= breakpointValues.sm) {
        setCurrentBreakpoint("sm")
      } else {
        setCurrentBreakpoint("xs")
      }
    }

    // Comprobar al montar
    checkBreakpoint()

    // Comprobar al cambiar el tamaño de la ventana
    window.addEventListener("resize", checkBreakpoint)

    // Limpiar
    return () => window.removeEventListener("resize", checkBreakpoint)
  }, [])

  return currentBreakpoint
}
