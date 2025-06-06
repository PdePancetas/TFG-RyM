"use client"

import { useState } from "react"
import { motion, AnimatePresence } from "framer-motion"
import { Button } from "@/components/ui/button"
import { Plus, X, Car, Calendar } from "lucide-react"
import { SearchDialog } from "@/components/search-dialog"
import { RequestCustomVehicleDialog } from "@/components/request-custom-vehicle-dialog"
import { RequestAppointmentDialog } from "@/components/request-appointment-dialog"
import { useBreakpoint } from "@/hooks/use-breakpoint"

export function FloatingActionButtons() {
  const [isOpen, setIsOpen] = useState(false)
  const isTablet = useBreakpoint("sm")

  // No mostrar en tablets o pantallas más grandes
  if (isTablet) {
    return null
  }

  return (
    <div className="fixed bottom-4 right-4 z-40">
      <AnimatePresence>
        {isOpen && (
          <div className="flex flex-col items-end space-y-2 mb-2">
            <motion.div
              initial={{ opacity: 0, y: 20, scale: 0.8 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, y: 20, scale: 0.8 }}
              transition={{ duration: 0.2, delay: 0.1 }}
            >
              <SearchDialog buttonVariant="default" buttonSize="icon" buttonText="" showIcon={true} />
            </motion.div>
            <motion.div
              initial={{ opacity: 0, y: 20, scale: 0.8 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, y: 20, scale: 0.8 }}
              transition={{ duration: 0.2, delay: 0.2 }}
            >
              <RequestCustomVehicleDialog
                buttonVariant="secondary"
                buttonSize="icon"
                className="p-0 w-10 h-10 flex items-center justify-center"
              >
                <Car className="h-4 w-4" />
              </RequestCustomVehicleDialog>
            </motion.div>
            <motion.div
              initial={{ opacity: 0, y: 20, scale: 0.8 }}
              animate={{ opacity: 1, y: 0, scale: 1 }}
              exit={{ opacity: 0, y: 20, scale: 0.8 }}
              transition={{ duration: 0.2, delay: 0.3 }}
            >
              <RequestAppointmentDialog
                buttonVariant="outline"
                buttonSize="icon"
                className="p-0 w-10 h-10 flex items-center justify-center bg-white"
              >
                <Calendar className="h-4 w-4" />
              </RequestAppointmentDialog>
            </motion.div>
          </div>
        )}
      </AnimatePresence>

      <Button size="icon" className="rounded-full h-12 w-12 shadow-lg" onClick={() => setIsOpen(!isOpen)}>
        {isOpen ? <X className="h-5 w-5" /> : <Plus className="h-5 w-5" />}
      </Button>
    </div>
  )
}
