"use client"

import type React from "react"

import { useState } from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { CheckCircle2 } from "lucide-react"
import { brands } from "@/data/vehicles"

interface RequestCustomVehicleDialogProps {
  buttonVariant?: "default" | "outline" | "ghost" | "link" | "secondary"
  buttonSize?: "default" | "sm" | "lg" | "icon"
  className?: string
  children?: React.ReactNode
}

export function RequestCustomVehicleDialog({
  buttonVariant = "default",
  buttonSize = "default",
  className = "",
  children,
}: RequestCustomVehicleDialogProps) {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isSuccess, setIsSuccess] = useState(false)
  const [open, setOpen] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsSubmitting(true)

    // Simulación de envío
    await new Promise((resolve) => setTimeout(resolve, 1500))

    // Aquí iría la lógica real de envío del formulario
    // const response = await fetch('/api/request-custom-vehicle', {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify(formData),
    // })

    setIsSubmitting(false)
    setIsSuccess(true)
  }

  const resetForm = () => {
    setIsSuccess(false)
  }

  return (
    <Dialog
      open={open}
      onOpenChange={(value) => {
        setOpen(value)
        if (!value) resetForm()
      }}
    >
      <DialogTrigger asChild>
        <Button variant={buttonVariant} size={buttonSize} className={className}>
          {children || "Solicitar vehículo personalizado"}
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[700px] max-h-[85vh] overflow-y-auto z-[9000]">
        <DialogHeader className="px-6 pt-6">
          <DialogTitle className="text-2xl font-bold">Solicitar vehículo personalizado</DialogTitle>
        </DialogHeader>

        {isSuccess ? (
          <div className="py-6 px-6 text-center">
            <div className="flex justify-center mb-4">
              <CheckCircle2 className="h-16 w-16 text-green-500" />
            </div>
            <h3 className="text-xl font-bold mb-2">¡Solicitud enviada con éxito!</h3>
            <p className="text-gray-600 mb-6">
              Hemos recibido tu solicitud. Un asesor se pondrá en contacto contigo en las próximas 24 horas.
            </p>
            <Button onClick={resetForm}>Realizar otra solicitud</Button>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="px-6 pb-6">
            <div className="space-y-4">
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="name">Nombre completo</Label>
                  <Input id="name" placeholder="Tu nombre" required />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="phone">Teléfono</Label>
                  <Input id="phone" placeholder="Tu teléfono" required />
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <Input id="email" type="email" placeholder="tu@email.com" required />
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="brand">Marca deseada</Label>
                  <Select>
                    <SelectTrigger id="brand">
                      <SelectValue placeholder="Selecciona una marca" />
                    </SelectTrigger>
                    <SelectContent>
                      {brands.map((brand) => (
                        <SelectItem key={brand} value={brand}>
                          {brand}
                        </SelectItem>
                      ))}
                      <SelectItem value="other">Otra marca</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label htmlFor="model">Modelo</Label>
                  <Input id="model" placeholder="Modelo deseado" />
                </div>
              </div>

              <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label htmlFor="budget">Presupuesto aproximado (€)</Label>
                  <Input id="budget" type="number" placeholder="50000" />
                </div>
                <div className="space-y-2">
                  <Label htmlFor="timeframe">Plazo de entrega deseado</Label>
                  <Select>
                    <SelectTrigger id="timeframe">
                      <SelectValue placeholder="Selecciona un plazo" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="urgent">Lo antes posible</SelectItem>
                      <SelectItem value="1month">En 1 mes</SelectItem>
                      <SelectItem value="3months">En 3 meses</SelectItem>
                      <SelectItem value="6months">En 6 meses</SelectItem>
                      <SelectItem value="flexible">Flexible</SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="details">Detalles adicionales</Label>
                <Textarea
                  id="details"
                  placeholder="Describe las características específicas que buscas en tu vehículo personalizado..."
                  rows={4}
                />
              </div>

              <div className="pt-4">
                <Button type="submit" disabled={isSubmitting} className="w-full">
                  {isSubmitting ? "Enviando..." : "Enviar solicitud"}
                </Button>
              </div>
            </div>
          </form>
        )}
      </DialogContent>
    </Dialog>
  )
}
