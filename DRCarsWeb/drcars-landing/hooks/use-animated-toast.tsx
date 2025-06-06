"use client"

import { useState, useCallback } from "react"
import { createRoot } from "react-dom/client"
import { AnimatedToast, type ToastType } from "@/components/ui/animated-toast"

interface ToastOptions {
  type?: ToastType
  title?: string
  message: string
  duration?: number
}

export function useAnimatedToast() {
  const [toasts, setToasts] = useState<{ id: string; element: HTMLDivElement }[]>([])

  const showToast = useCallback(({ type = "info", title = "", message, duration = 3000 }: ToastOptions) => {
    // Crear un elemento div para el toast
    const toastElement = document.createElement("div")
    document.body.appendChild(toastElement)

    // Generar un ID único para este toast
    const id = `toast-${Date.now()}-${Math.random().toString(36).substr(2, 9)}`

    // Añadir el toast a la lista
    setToasts((prev) => [...prev, { id, element: toastElement }])

    // Renderizar el toast
    const root = createRoot(toastElement)
    root.render(
      <AnimatedToast
        type={type}
        title={title}
        message={message}
        duration={duration}
        onClose={() => {
          // Eliminar el elemento del DOM cuando se cierre
          document.body.removeChild(toastElement)
          // Eliminar el toast de la lista
          setToasts((prev) => prev.filter((toast) => toast.id !== id))
        }}
      />,
    )

    return id
  }, [])

  const success = useCallback(
    (message: string, title = "¡Éxito!", duration?: number) => {
      return showToast({ type: "success", title, message, duration })
    },
    [showToast],
  )

  const error = useCallback(
    (message: string, title = "Error", duration?: number) => {
      return showToast({ type: "error", title, message, duration })
    },
    [showToast],
  )

  const warning = useCallback(
    (message: string, title = "Advertencia", duration?: number) => {
      return showToast({ type: "warning", title, message, duration })
    },
    [showToast],
  )

  const info = useCallback(
    (message: string, title = "Información", duration?: number) => {
      return showToast({ type: "info", title, message, duration })
    },
    [showToast],
  )

  return { showToast, success, error, warning, info }
}
