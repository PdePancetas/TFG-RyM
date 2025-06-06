"use client"

import { useEffect, useState } from "react"

interface UseInactivityOptions {
  timeout?: number // tiempo en milisegundos
  onInactive?: () => void
}

export function useInactivity({
  timeout = 30 * 60 * 1000, // 30 minutos por defecto
  onInactive,
}: UseInactivityOptions) {
  const [isInactive, setIsInactive] = useState(false)

  useEffect(() => {
    let inactivityTimer: NodeJS.Timeout

    const resetTimer = () => {
      clearTimeout(inactivityTimer)
      if (isInactive) setIsInactive(false)

      inactivityTimer = setTimeout(() => {
        setIsInactive(true)
        if (onInactive) onInactive()
      }, timeout)
    }

    // Eventos que reinician el temporizador
    const events = ["mousedown", "mousemove", "keypress", "scroll", "touchstart"]

    // Iniciar el temporizador
    resetTimer()

    // Añadir event listeners
    events.forEach((event) => {
      window.addEventListener(event, resetTimer)
    })

    // Limpiar al desmontar
    return () => {
      clearTimeout(inactivityTimer)
      events.forEach((event) => {
        window.removeEventListener(event, resetTimer)
      })
    }
  }, [timeout, onInactive, isInactive])

  return { isInactive }
}
