"use client"

import type React from "react"

import { useEffect } from "react"
import Swal from "sweetalert2"
import "sweetalert2/dist/sweetalert2.min.css"

// Componente que configura SweetAlert2 globalmente
export function SweetAlertProvider({ children }: { children: React.ReactNode }) {
  // Configuración global de SweetAlert2
  useEffect(() => {
    // Personalizar el tema si es necesario
    Swal.mixin({
      customClass: {
        confirmButton: "bg-green-500 hover:bg-green-600 text-white py-2 px-4 rounded",
        cancelButton: "bg-gray-300 hover:bg-gray-400 text-gray-800 py-2 px-4 rounded ml-2",
      },
      buttonsStyling: false,
    })
  }, [])

  return <>{children}</>
}
