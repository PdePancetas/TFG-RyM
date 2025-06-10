"use client"
import { motion, AnimatePresence } from "framer-motion"
import { Button } from "@/components/ui/button"
import { CheckCircle, XCircle, AlertTriangle, Info } from "lucide-react"

export type PopupType = "success" | "error" | "warning" | "info"

interface CustomPopupProps {
  type: PopupType
  title: string
  message: string
  isOpen: boolean
  onClose: () => void
}

export function CustomPopup({ type = "success", title, message, isOpen, onClose }: CustomPopupProps) {
  // Animation variants
  const backdropVariants = {
    hidden: { opacity: 0 },
    visible: { opacity: 1 },
  }

  const popupVariants = {
    hidden: { scale: 0.8, opacity: 0 },
    visible: {
      scale: 1,
      opacity: 1,
      transition: {
        type: "spring",
        stiffness: 500,
        damping: 25,
      },
    },
    exit: {
      scale: 0.8,
      opacity: 0,
      transition: { duration: 0.2 },
    },
  }

  const iconVariants = {
    hidden: { scale: 0, rotate: -180 },
    visible: {
      scale: 1,
      rotate: 0,
      transition: {
        type: "spring",
        stiffness: 500,
        damping: 25,
        delay: 0.2,
      },
    },
  }

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

  // Función para obtener el color del botón según el tipo
  const getButtonClass = () => {
    switch (type) {
      case "success":
        return "bg-green-500 hover:bg-green-600 text-white"
      case "error":
        return "bg-red-500 hover:bg-red-600 text-white"
      case "warning":
        return "bg-amber-500 hover:bg-amber-600 text-white"
      case "info":
        return "bg-blue-500 hover:bg-blue-600 text-white"
      default:
        return "bg-green-500 hover:bg-green-600 text-white"
    }
  }

  return (
    <AnimatePresence>
      {isOpen && (
        <motion.div
          className="fixed inset-0 z-[99999] flex items-center justify-center bg-black/50 backdrop-blur-sm"
          initial="hidden"
          animate="visible"
          exit="hidden"
          variants={backdropVariants}
        >
          <motion.div
            className="bg-white rounded-lg shadow-2xl w-full max-w-md mx-4 overflow-hidden"
            variants={popupVariants}
            initial="hidden"
            animate="visible"
            exit="exit"
          >
            <div className="p-6 text-center">
              <motion.div
                className="flex justify-center mb-6"
                variants={iconVariants}
                initial="hidden"
                animate="visible"
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

              <motion.div
                initial={{ opacity: 0, y: 10 }}
                animate={{ opacity: 1, y: 0 }}
                transition={{ delay: 0.5 }}
                whileHover={{ scale: 1.05 }}
                whileTap={{ scale: 0.95 }}
              >
                <Button onClick={onClose} className={`${getButtonClass()} w-full`}>
                  OK
                </Button>
              </motion.div>
            </div>
          </motion.div>
        </motion.div>
      )}
    </AnimatePresence>
  )
}
