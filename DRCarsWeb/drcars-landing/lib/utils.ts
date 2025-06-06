import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

// Añadir una función para formatear fechas
export function formatDate(dateString: string): string {
  try {
    const date = new Date(dateString)

    // Verificar si la fecha es válida
    if (isNaN(date.getTime())) {
      return "Fecha no válida"
    }

    // Formatear la fecha
    const formattedDate = date.toLocaleDateString("es-ES", {
      day: "2-digit",
      month: "2-digit",
      year: "numeric",
    })

    // Formatear la hora
    const formattedTime = date.toLocaleTimeString("es-ES", {
      hour: "2-digit",
      minute: "2-digit",
    })

    return `${formattedDate} - ${formattedTime}`
  } catch (error) {
    console.error("Error al formatear la fecha:", error)
    return "Fecha no válida"
  }
}
