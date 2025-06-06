"use client"

import type React from "react"

import { useState, useEffect, useRef } from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { CheckCircle2, ChevronDown } from "lucide-react"
import { usePathname } from "next/navigation"
import { useUser } from "@/contexts/user-context"
import { AuthDialog } from "@/components/auth-dialog"

interface RequestAppointmentDialogProps {
  buttonVariant?: "default" | "outline" | "ghost" | "link" | "secondary"
  buttonSize?: "default" | "sm" | "lg" | "icon"
  className?: string
  children?: React.ReactNode
  preselectedReason?: string
  disableReasonChange?: boolean
  vehicleId?: number
}

export function RequestAppointmentDialog({
  buttonVariant = "outline",
  buttonSize = "lg",
  className = "",
  children,
  preselectedReason,
  disableReasonChange = false,
  vehicleId,
}: RequestAppointmentDialogProps) {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isSuccess, setIsSuccess] = useState(false)
  const [open, setOpen] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [showAuthDialog, setShowAuthDialog] = useState(false)
  const authDialogClosedRef = useRef(false)

  const [firstName, setFirstName] = useState("")
  const [lastName, setLastName] = useState("")
  const [email, setEmail] = useState("")
  const [dni, setDni] = useState("")
  const [date, setDate] = useState("")
  const [time, setTime] = useState("")
  const [reason, setReason] = useState(preselectedReason || "")
  const [notes, setNotes] = useState("")

  const [showTimeDropdown, setShowTimeDropdown] = useState(false)
  const [showReasonDropdown, setShowReasonDropdown] = useState(false)

  // Obtener el contexto de usuario para verificar autenticación
  const { isAuthenticated, user, lastAuthAction, resetAuthAction } = useUser()

  // Obtener la ruta actual para verificar si estamos en la página principal
  const pathname = usePathname()
  const isHomePage = pathname === "/"
  const isVehicleDetailsPage = pathname?.startsWith("/vehiculos/")

  // Efecto para detectar cuando el usuario inicia sesión
  useEffect(() => {
    // Si el usuario acaba de iniciar sesión y teníamos el diálogo de auth abierto
    if (isAuthenticated && showAuthDialog && lastAuthAction === "login") {
      console.log("Usuario autenticado después de mostrar AuthDialog, cerrando diálogo de autenticación")

      // Marcar que el diálogo de autenticación se ha cerrado
      authDialogClosedRef.current = true

      // Cerrar el diálogo de autenticación
      setShowAuthDialog(false)

      // Resetear el estado de autenticación para evitar procesamiento repetido
      resetAuthAction()

      // Esperar a que se cierre completamente el diálogo de autenticación antes de abrir el formulario
      setTimeout(() => {
        console.log("Abriendo formulario de cita después del retraso")

        // Abrir el formulario de cita
        setOpen(true)

        // Pre-llenar el formulario con los datos del usuario si están disponibles
        if (user) {
          setFirstName(user.name.split(" ")[0] || "")
          setLastName(user.name.split(" ").slice(1).join(" ") || "")
          setEmail(user.email)
          setDni(user.dni || "") // ✅ Autocompletar DNI si existe
        }
      }, 500) // Reducir el retraso a 500ms para una mejor experiencia de usuario
    }
  }, [isAuthenticated, showAuthDialog, lastAuthAction, user, resetAuthAction])

  // Lista de horas disponibles
  const hours = Array.from({ length: 24 }, (_, i) => {
    const hour = i.toString().padStart(2, "0")
    return `${hour}:00`
  })

  // Lista de motivos
  const reasons = [
    { value: "test-drive", label: "Prueba de vehículo" },
    { value: "custom-order", label: "Pedido personalizado" },
    { value: "financing", label: "Información sobre financiación" },
    { value: "other", label: "Otro motivo" },
  ]

  // Función para generar un valor aleatorio para el header de ngrok
  const generateRandomValue = () => {
    return Math.random().toString(36).substring(2, 15)
  }

  // Función para determinar el precio según el motivo
  const getPriceByReason = (reasonValue: string): number => {
    switch (reasonValue) {
      case "test-drive":
        return 50
      case "custom-order":
      case "financing":
      case "other":
      default:
        return 0
    }
  }

  // Función para extraer el ID del vehículo de la URL si no se proporciona como prop
  const getVehicleIdFromUrl = (): number | undefined => {
    if (!isVehicleDetailsPage) return undefined

    const matches = pathname?.match(/\/vehiculos\/(\d+)/)
    if (matches && matches[1]) {
      return Number.parseInt(matches[1], 10)
    }
    return undefined
  }

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsSubmitting(true)
    setError(null)

    try {
      // Formatear la fecha y hora en el formato requerido: YYYY-MM-DDThh:mm:ss
      const [hour, minute] = time.split(":").map(Number)
      const formattedDate = new Date(date)
      formattedDate.setHours(hour || 0, minute || 0, 0)

      const formattedDateTime = formattedDate.toISOString().split(".")[0] // Formato YYYY-MM-DDThh:mm:ss

      // Crear el objeto de datos para la petición
      const requestData: any = {
        nombre: firstName,
        apellidos: lastName,
        email: email,
        dni: dni,
        fecha: formattedDateTime,
        motivo: reason,
        descripcion: notes,
        precio: getPriceByReason(reason),
      }

      // Si estamos en la página de detalles de un vehículo, añadir el ID del vehículo
      const effectiveVehicleId = vehicleId || getVehicleIdFromUrl()
      if (effectiveVehicleId) {
        requestData.idVehiculo = effectiveVehicleId
        console.log(`Añadiendo ID de vehículo: ${effectiveVehicleId} a la solicitud`)
      }

      console.log("Enviando datos de reserva:", requestData)

      // Realizar la petición POST si estamos en la página principal o en detalles de vehículo
      if (isHomePage || isVehicleDetailsPage) {
        const response = await fetch("https://helped-bug-stirring.ngrok-free.app/reservas/crear", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            "ngrok-skip-browser-warning": generateRandomValue(),
          },
          body: JSON.stringify(requestData),
        })

        if (!response.ok) {
          const errorData = await response.text()
          console.error("Error en la respuesta:", response.status, errorData)
          throw new Error(`Error al crear la reserva: ${response.status}`)
        }

        console.log("Reserva creada exitosamente")
      } else {
        // Si no estamos en la página principal ni en detalles, simulamos el envío
        await new Promise((resolve) => setTimeout(resolve, 1500))
        console.log("Simulación de envío completada (no estamos en una página que requiera envío real)")
      }

      setIsSubmitting(false)
      setIsSuccess(true)
    } catch (err) {
      console.error("Error al enviar el formulario:", err)
      setError("Ha ocurrido un error al procesar tu solicitud. Por favor, inténtalo de nuevo.")
      setIsSubmitting(false)
    }
  }

  const resetForm = () => {
    setFirstName("")
    setLastName("")
    setEmail("")
    setDni("")
    setDate("")
    setTime("")
    setReason(preselectedReason || "")
    setNotes("")
    setIsSuccess(false)
    setError(null)
    setIsSubmitting(false)
  }

  const handleSelectTime = (selectedTime: string) => {
    setTime(selectedTime)
    setShowTimeDropdown(false)
  }

  const handleSelectReason = (selectedReason: string) => {
    setReason(selectedReason)
    setShowReasonDropdown(false)
  }

  const getReasonLabel = (value: string) => {
    return reasons.find((r) => r.value === value)?.label || ""
  }

  // Función para manejar el clic en el botón de solicitar cita
  const handleButtonClick = () => {
    if (!isAuthenticated) {
      console.log("Usuario no autenticado, mostrando diálogo de autenticación")
      setShowAuthDialog(true)
    } else {
      console.log("Usuario autenticado, mostrando formulario de cita")

      // ✅ RESETEAR ESTADOS ANTES DE ABRIR
      resetForm()
      setOpen(true)

      // Pre-llenar el formulario con los datos del usuario si están disponibles
      if (user) {
        setFirstName(user.name.split(" ")[0] || "")
        setLastName(user.name.split(" ").slice(1).join(" ") || "")
        setEmail(user.email)
        setDni(user.dni || "") // ✅ Autocompletar DNI si existe
      }
    }
  }

  // Función para manejar el cambio de estado del diálogo de autenticación
  const handleAuthDialogChange = (isOpen: boolean) => {
    setShowAuthDialog(isOpen)

    // Si el diálogo se está cerrando y el usuario está autenticado y el cierre no fue por login exitoso
    if (!isOpen && isAuthenticated && !authDialogClosedRef.current) {
      console.log("Diálogo de autenticación cerrado manualmente, abriendo formulario de cita")

      // Abrir el formulario de cita
      setTimeout(() => {
        setOpen(true)

        // Pre-llenar el formulario con los datos del usuario
        if (user) {
          setFirstName(user.name.split(" ")[0] || "")
          setLastName(user.name.split(" ").slice(1).join(" ") || "")
          setEmail(user.email)
          setDni(user.dni || "") // ✅ Autocompletar DNI si existe
        }
      }, 300)
    }

    // Resetear el flag cuando el diálogo se cierra
    if (!isOpen) {
      authDialogClosedRef.current = false
    }
  }

  return (
    <>
      {/* Botón para abrir el diálogo */}
      <Button variant={buttonVariant} size={buttonSize} className={className} onClick={handleButtonClick}>
        {children || "Solicitar Cita"}
      </Button>

      {/* Diálogo de autenticación - Ahora pasamos showSuccessPopup={false} */}
      {showAuthDialog && (
        <AuthDialog
          isOpen={showAuthDialog}
          onOpenChange={handleAuthDialogChange}
          redirectAfterLogin={false}
          showSuccessPopup={false} // No mostrar popup de éxito cuando se inicia sesión desde aquí
        />
      )}

      {/* Diálogo de solicitud de cita */}
      <Dialog
        open={open}
        onOpenChange={(value) => {
          setOpen(value)
          if (!value) resetForm()
        }}
      >
        <DialogContent className="sm:max-w-[650px] max-h-[85vh] overflow-y-auto z-[9000]">
          <DialogHeader>
            <DialogTitle className="text-2xl font-bold">Solicitar Cita</DialogTitle>
          </DialogHeader>

          {isSuccess ? (
            <div className="py-6 text-center">
              <div className="flex justify-center mb-4">
                <CheckCircle2 className="h-16 w-16 text-green-500" />
              </div>
              <h3 className="text-xl font-bold mb-2">¡Cita solicitada con éxito!</h3>
              <p className="text-gray-600 mb-6">
                Hemos recibido tu solicitud de cita. Te confirmaremos la fecha y hora por email o teléfono.
              </p>
              <Button onClick={() => setOpen(false)}>Hecho</Button>
            </div>
          ) : (
            <form onSubmit={handleSubmit} className="space-y-6 py-4">
              {error && (
                <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md">{error}</div>
              )}

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="firstName">Nombre</Label>
                  <Input
                    id="firstName"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                    placeholder="Tu nombre"
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="lastName">Apellidos</Label>
                  <Input
                    id="lastName"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                    placeholder="Tus apellidos"
                    required
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <Input
                    id="email"
                    type="email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                    placeholder="tu@email.com"
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="dni">DNI</Label>
                  <Input
                    id="dni"
                    value={dni}
                    onChange={(e) => setDni(e.target.value)}
                    placeholder="12345678A"
                    required
                  />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <Label htmlFor="date">Fecha preferida</Label>
                  <Input
                    id="date"
                    type="date"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                    min={new Date().toISOString().split("T")[0]}
                    required
                  />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="time">Hora preferida</Label>
                  <div className="relative">
                    <button
                      type="button"
                      className="w-full flex items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm"
                      onClick={() => setShowTimeDropdown(!showTimeDropdown)}
                    >
                      {time || "Selecciona una hora"}
                      <ChevronDown className="h-4 w-4 opacity-50" />
                    </button>

                    {showTimeDropdown && (
                      <div className="absolute z-10 mt-1 w-full max-h-60 overflow-auto rounded-md bg-white py-1 shadow-lg border border-gray-200">
                        {hours.map((hour) => (
                          <div
                            key={hour}
                            className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${time === hour ? "bg-gray-200" : ""}`}
                            onClick={() => handleSelectTime(hour)}
                          >
                            {hour}
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="reason">Motivo de la cita</Label>
                {disableReasonChange && preselectedReason ? (
                  <div className="bg-gray-50 p-3 rounded-md border border-gray-200">
                    <p className="text-gray-700">{getReasonLabel(preselectedReason)}</p>
                  </div>
                ) : (
                  <div className="relative">
                    <button
                      type="button"
                      className="w-full flex items-center justify-between rounded-md border border-input bg-background px-3 py-2 text-sm"
                    >
                      {reason ? getReasonLabel(reason) : "Selecciona un motivo"}
                      <ChevronDown className="h-4 w-4 opacity-50" />
                    </button>

                    {showReasonDropdown && (
                      <div className="absolute z-10 mt-1 w-full max-h-60 overflow-auto rounded-md bg-white py-1 shadow-lg border border-gray-200">
                        {reasons.map((r) => (
                          <div
                            key={r.value}
                            className={`px-3 py-2 cursor-pointer hover:bg-gray-100 ${reason === r.value ? "bg-gray-200" : ""}`}
                            onClick={() => handleSelectReason(r.value)}
                          >
                            {r.label}
                          </div>
                        ))}
                      </div>
                    )}
                  </div>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="notes">Descripción</Label>
                <Textarea
                  id="notes"
                  value={notes}
                  onChange={(e) => setNotes(e.target.value)}
                  placeholder="Información adicional que quieras proporcionarnos..."
                  rows={3}
                />
              </div>

              <div className="flex justify-end pt-4">
                <Button type="submit" disabled={isSubmitting} className="min-w-[150px]">
                  {isSubmitting ? "Enviando..." : "Solicitar cita"}
                </Button>
              </div>
            </form>
          )}
        </DialogContent>
      </Dialog>
    </>
  )
}
