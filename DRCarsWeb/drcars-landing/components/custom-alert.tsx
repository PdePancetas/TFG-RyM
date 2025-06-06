"use client"

import type React from "react"

import { useEffect, useState } from "react"
import { motion, AnimatePresence } from "framer-motion"
import { CheckCircle, XCircle, AlertTriangle, Info } from "lucide-react"

export type AlertType = "success" | "error" | "warning" | "info"

interface CustomAlertProps {
  type: AlertType
  title: string
  message: string
  duration?: number
  onClose?: () => void
  autoClose?: boolean
  autoCloseDuration?: number
}

// Componente para una alerta individual
export function CustomAlert({
  type = "success",
  title,
  message,
  duration = 0,
  onClose,
  autoClose = false,
  autoCloseDuration = 1500,
}: CustomAlertProps) {
  const [isVisible, setIsVisible] = useState(true)
  const [progress, setProgress] = useState(100)

  useEffect(() => {
    // Si autoClose es true, cerramos automáticamente después de autoCloseDuration
    if (autoClose) {
      // Actualizar la barra de progreso cada 10ms
      const interval = setInterval(() => {
        setProgress((prev) => {
          const newProgress = prev - (10 / autoCloseDuration) * 100
          return newProgress < 0 ? 0 : newProgress
        })
      }, 10)

      const timer = setTimeout(() => {
        setIsVisible(false)
        if (onClose) setTimeout(onClose, 300) // Dar tiempo para la animación de salida
      }, autoCloseDuration)

      return () => {
        clearTimeout(timer)
        clearInterval(interval)
      }
    }
    // Si no es autoClose pero tiene duration, usamos ese comportamiento
    else if (duration > 0) {
      const timer = setTimeout(() => {
        setIsVisible(false)
        if (onClose) setTimeout(onClose, 300) // Dar tiempo para la animación de salida
      }, duration)

      return () => clearTimeout(timer)
    }
  }, [autoClose, autoCloseDuration, duration, onClose])

  // Función para obtener el icono según el tipo
  const getIcon = () => {
    switch (type) {
      case "success":
        return <CheckCircle className="h-16 w-16 text-green-500" />
      case "error":
        return <XCircle className="h-16 w-16 text-red-500" />
      case "warning":
        return <AlertTriangle className="h-16 w-16 text-amber-500" />
      case "info":
        return <Info className="h-16 w-16 text-blue-500" />
      default:
        return <CheckCircle className="h-16 w-16 text-green-500" />
    }
  }

  // Función para obtener el color de la barra de progreso según el tipo
  const getProgressBarColor = () => {
    switch (type) {
      case "success":
        return "bg-green-500"
      case "error":
        return "bg-red-500"
      case "warning":
        return "bg-amber-500"
      case "info":
        return "bg-blue-500"
      default:
        return "bg-green-500"
    }
  }

  return (
    <AnimatePresence>
      {isVisible && (
        <motion.div
          className="fixed inset-0 z-[99999] flex items-center justify-center bg-black/50 backdrop-blur-sm pointer-events-auto"
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          exit={{ opacity: 0 }}
          transition={{ duration: 0.2 }}
          onClick={(e) => e.stopPropagation()}
        >
          <motion.div
            className="bg-white rounded-lg shadow-2xl w-full max-w-md mx-4 overflow-hidden relative"
            initial={{ scale: 0.9, opacity: 0 }}
            animate={{ scale: 1, opacity: 1 }}
            exit={{ scale: 0.9, opacity: 0 }}
            transition={{ type: "spring", damping: 25, stiffness: 300 }}
            onClick={(e) => e.stopPropagation()}
          >
            <div className="p-6 text-center">
              <motion.div
                className="flex justify-center mb-6"
                initial={{ scale: 0, rotate: -180 }}
                animate={{ scale: 1, rotate: 0 }}
                transition={{ type: "spring", damping: 15, stiffness: 200, delay: 0.2 }}
              >
                {getIcon()}
              </motion.div>

              <motion.h3
                className="text-xl font-bold mb-2"
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.3 }}
              >
                {title}
              </motion.h3>

              <motion.p
                className="text-gray-600 mb-6"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ delay: 0.4 }}
              >
                {message}
              </motion.p>
            </div>

            {/* Barra de progreso */}
            {autoClose && (
              <div className="h-1 w-full bg-gray-200 absolute bottom-0 left-0">
                <motion.div
                  className={`h-full ${getProgressBarColor()}`}
                  initial={{ width: "100%" }}
                  animate={{ width: `${progress}%` }}
                  transition={{ duration: 0 }}
                />
              </div>
            )}
          </motion.div>
        </motion.div>
      )}
    </AnimatePresence>
  )
}

// Componente para gestionar alertas globalmente
interface AlertProviderProps {
  children: React.ReactNode
}

interface AlertOptions extends Omit<CustomAlertProps, "onClose"> {}

interface AlertContextType {
  showAlert: (options: AlertOptions) => void
  closeAlert: () => void
}

// Crear un contexto global para las alertas
import { createContext, useContext } from "react"

const AlertContext = createContext<AlertContextType | undefined>(undefined)

export function AlertProvider({ children }: AlertProviderProps) {
  const [alert, setAlert] = useState<(AlertOptions & { id: number }) | null>(null)

  const showAlert = (options: AlertOptions) => {
    // Si ya hay una alerta visible, primero la cerramos
    if (alert) {
      closeAlert()
      // Pequeño retraso antes de mostrar la nueva alerta
      setTimeout(() => {
        setAlert({ ...options, id: Date.now() })
      }, 300)
    } else {
      setAlert({ ...options, id: Date.now() })
    }
  }

  const closeAlert = () => {
    setAlert(null)
  }

  return (
    <AlertContext.Provider value={{ showAlert, closeAlert }}>
      {children}
      <div style={{ position: "relative", zIndex: 100000 }}>
        {alert && <CustomAlert {...alert} onClose={closeAlert} />}
      </div>
    </AlertContext.Provider>
  )
}

// Hook para usar el contexto de alertas
export function useAlert() {
  const context = useContext(AlertContext)
  if (context === undefined) {
    throw new Error("useAlert must be used within an AlertProvider")
  }
  return context
}
