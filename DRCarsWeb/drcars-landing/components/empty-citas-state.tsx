"use client"

import { Button } from "@/components/ui/button"
import { Calendar } from "lucide-react"

interface EmptyCitasStateProps {
  onRequestCita?: () => void
  hasDni: boolean
  onEditProfile?: () => void
}

export function EmptyCitasState({ onRequestCita, hasDni, onEditProfile }: EmptyCitasStateProps) {
  return (
    <div className="text-center py-12">
      <Calendar className="h-12 w-12 mx-auto text-gray-300 mb-4" />

      {hasDni ? (
        <>
          <h3 className="text-lg font-medium text-gray-900 mb-2">No tienes citas registradas</h3>
          <p className="text-gray-500 mb-4">Solicita una cita para ver tu historial de citas aquí.</p>
          {onRequestCita && <Button onClick={onRequestCita}>Solicitar Cita</Button>}
        </>
      ) : (
        <>
          <h3 className="text-lg font-medium text-gray-900 mb-2">Añade tu DNI para ver tus citas</h3>
          <p className="text-gray-500 mb-4">Para ver tu historial de citas, necesitas añadir tu DNI en tu perfil.</p>
          {onEditProfile && <Button onClick={onEditProfile}>Editar Perfil</Button>}
        </>
      )}
    </div>
  )
}
