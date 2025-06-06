"use client"

import type React from "react"

import { useState } from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { CheckCircle2, Search } from "lucide-react"
import { Slider } from "@/components/ui/slider"
import { Checkbox } from "@/components/ui/checkbox"
import { brands } from "@/data/vehicles"

interface RequestSearchDialogProps {
  buttonVariant?: "default" | "outline" | "ghost" | "link"
  buttonSize?: "default" | "sm" | "lg" | "icon"
  className?: string
}

export function RequestSearchDialog({
  buttonVariant = "default",
  buttonSize = "lg",
  className = "",
}: RequestSearchDialogProps) {
  const [isSubmitting, setIsSubmitting] = useState(false)
  const [isSuccess, setIsSuccess] = useState(false)
  const [priceRange, setPriceRange] = useState([30000, 100000])

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setIsSubmitting(true)

    // Simulación de envío
    await new Promise((resolve) => setTimeout(resolve, 1500))

    // Aquí iría la lógica real de envío del formulario
    // const response = await fetch('/api/request-search', {
    //   method: 'POST',
    //   headers: { 'Content-Type': 'application/json' },
    //   body: JSON.stringify(formData),
    // })

    setIsSubmitting(false)
    setIsSuccess(true)
  }

  const resetForm = () => {
    setIsSuccess(false)
    setPriceRange([30000, 100000])
  }

  return (
    <Dialog onOpenChange={(open) => !open && resetForm()}>
      <DialogTrigger asChild>
        <Button variant={buttonVariant} size={buttonSize} className={className}>
          Solicitar Búsqueda
          <Search className="ml-2 h-4 w-4" />
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[550px] z-[9000]">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold">Solicitar Búsqueda de Vehículo</DialogTitle>
        </DialogHeader>

        {isSuccess ? (
          <div className="py-6 text-center">
            <div className="flex justify-center mb-4">
              <CheckCircle2 className="h-16 w-16 text-green-500" />
            </div>
            <h3 className="text-xl font-bold mb-2">¡Solicitud enviada con éxito!</h3>
            <p className="text-gray-600 mb-6">
              Hemos recibido tu solicitud de búsqueda. Nuestro equipo comenzará a buscar el vehículo que deseas y te
              contactaremos en breve.
            </p>
            <Button onClick={resetForm}>Realizar otra solicitud</Button>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="space-y-4 py-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="search-name">Nombre completo</Label>
                <Input id="search-name" placeholder="Tu nombre" required />
              </div>
              <div className="space-y-2">
                <Label htmlFor="search-phone">Teléfono</Label>
                <Input id="search-phone" placeholder="Tu teléfono" required />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="search-email">Email</Label>
              <Input id="search-email" type="email" placeholder="tu@email.com" required />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="search-brand">Marca</Label>
                <Select>
                  <SelectTrigger id="search-brand">
                    <SelectValue placeholder="Selecciona una marca" />
                  </SelectTrigger>
                  <SelectContent>
                    {brands.map((brand) => (
                      <SelectItem key={brand} value={brand}>
                        {brand}
                      </SelectItem>
                    ))}
                    <SelectItem value="any">Cualquier marca</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="search-model">Modelo</Label>
                <Input id="search-model" placeholder="Modelo deseado" />
              </div>
            </div>

            <div className="space-y-2">
              <div className="flex justify-between">
                <Label>Rango de precio</Label>
                <span className="text-sm text-muted-foreground">
                  {priceRange[0].toLocaleString("es-ES")}€ - {priceRange[1].toLocaleString("es-ES")}€
                </span>
              </div>
              <Slider
                defaultValue={priceRange}
                min={0}
                max={250000}
                step={5000}
                value={priceRange}
                onValueChange={setPriceRange}
                className="my-4"
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="search-year">Año mínimo</Label>
                <Select>
                  <SelectTrigger id="search-year">
                    <SelectValue placeholder="Selecciona año mínimo" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="any">Cualquier año</SelectItem>
                    {[...Array(10)].map((_, i) => {
                      const year = new Date().getFullYear() - i
                      return (
                        <SelectItem key={year} value={year.toString()}>
                          {year}
                        </SelectItem>
                      )
                    })}
                  </SelectContent>
                </Select>
              </div>
              <div className="space-y-2">
                <Label htmlFor="search-fuel">Combustible</Label>
                <Select>
                  <SelectTrigger id="search-fuel">
                    <SelectValue placeholder="Tipo de combustible" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="any">Cualquiera</SelectItem>
                    <SelectItem value="Gasolina">Gasolina</SelectItem>
                    <SelectItem value="Diésel">Diésel</SelectItem>
                    <SelectItem value="Híbrido">Híbrido</SelectItem>
                    <SelectItem value="Eléctrico">Eléctrico</SelectItem>
                  </SelectContent>
                </Select>
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="search-details">Características específicas</Label>
              <Textarea
                id="search-details"
                placeholder="Describe las características específicas que buscas (color, equipamiento, etc.)..."
                rows={3}
              />
            </div>

            <div className="flex items-center space-x-2 pt-2">
              <Checkbox id="search-urgent" />
              <label
                htmlFor="search-urgent"
                className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
              >
                Búsqueda urgente (en menos de 15 días)
              </label>
            </div>

            <div className="flex justify-end pt-4">
              <Button type="submit" disabled={isSubmitting} className="min-w-[150px]">
                {isSubmitting ? "Enviando..." : "Enviar solicitud"}
              </Button>
            </div>
          </form>
        )}
      </DialogContent>
    </Dialog>
  )
}
