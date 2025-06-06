"use client"

import { useState, useEffect, useCallback } from "react"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { useUser } from "@/contexts/user-context"
import { CitaCard } from "@/components/cita-card"
import { EmptyCitasState } from "@/components/empty-citas-state"
import { RequestAppointmentDialog } from "@/components/request-appointment-dialog"

interface Cita {
  id: number
  fecha: string
  motivo: string
  descripcion: string
  precio: number
  estado: string
  vehiculo?: {
    idVehiculo: number
    marca: string
    modelo: string
  }
}

interface HistorialCitasProps {
  dni: string
  onEditProfile: () => void
}

export function HistorialCitas({ dni, onEditProfile }: HistorialCitasProps) {
  const [citas, setCitas] = useState<Cita[]>([])
  const [loadingCitas, setLoadingCitas] = useState(false)
  const [errorCitas, setErrorCitas] = useState<string | null>(null)
  const { user } = useUser()

  // Función para generar un valor aleatorio para el header de ngrok
  const generateRandomValue = () => {
    return Math.random().toString(36).substring(2, 15)
  }

  // Función para cargar las citas del usuario
  const cargarCitas = useCallback(async (userDni: string) => {
    if (!userDni) return

    setLoadingCitas(true)
    setErrorCitas(null)

    try {
      console.log(`Cargando citas para DNI: ${userDni}`)
      const response = await fetch(`https://helped-bug-stirring.ngrok-free.app/reservas?id=${userDni}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          "ngrok-skip-browser-warning": generateRandomValue(),
        },
        body: JSON.stringify({ dni: userDni }),
      })

      if (!response.ok) {
        const errorText = await response.text()
        console.error(`Error al obtener las citas: ${response.status}`, errorText)
        throw new Error(`Error al obtener las citas: ${response.status}`)
      }

      const data = await response.json()
      console.log("Citas obtenidas:", data)
      setCitas(Array.isArray(data) ? data : [])
    } catch (error) {
      console.error("Error al cargar las citas:", error)
      setErrorCitas("No se pudieron cargar las citas. Por favor, inténtalo de nuevo más tarde.")
      setCitas([])
    } finally {
      setLoadingCitas(false)
    }
  }, [])

  // Cargar las citas cuando cambia el DNI
  useEffect(() => {
    if (dni) {
      cargarCitas(dni)
    } else {
      setCitas([])
    }
  }, [dni, cargarCitas])

  // Función para manejar la solicitud de una nueva cita
  const handleRequestCita = () => {
    // Esta función se pasará al componente EmptyCitasState
    // El diálogo se abrirá desde el botón del RequestAppointmentDialog
  }

  return (
    <Card className="mt-6">
      <CardHeader className="flex flex-row items-center justify-between">
        <div>
          <CardTitle>Historial de Citas</CardTitle>
          <CardDescription>Tus citas recientes</CardDescription>
        </div>
        {dni && (
          <RequestAppointmentDialog buttonVariant="outline" buttonSize="sm">
            Nueva Cita
          </RequestAppointmentDialog>
        )}
      </CardHeader>
      <CardContent>
        {loadingCitas ? (
          <div className="flex justify-center py-4">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary"></div>
          </div>
        ) : errorCitas ? (
          <div className="text-center py-4">
            <p className="text-red-500 mb-2">{errorCitas}</p>
            <Button variant="outline" size="sm" onClick={() => cargarCitas(dni)}>
              Reintentar
            </Button>
          </div>
        ) : citas.length > 0 ? (
          <div className="space-y-4">
            {citas.map((cita) => (
              <CitaCard key={cita.id} cita={cita} />
            ))}
          </div>
        ) : (
          <EmptyCitasState hasDni={!!dni} onEditProfile={onEditProfile} />
        )}
      </CardContent>
    </Card>
  )
}
