"use client"

import { useState, useEffect } from "react"
import { motion } from "framer-motion"
import Image from "next/image"
import Link from "next/link"
import { ArrowLeft, Check, ChevronRight, Heart, Share, ShieldCheck } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import type { Vehicle } from "@/data/vehicles"
import { AnimatedLink } from "@/components/animated-link"
import { CarCard } from "@/components/car-card"
import { useCatalogo } from "@/contexts/catalogo-context"
import { RequestAppointmentDialog } from "@/components/request-appointment-dialog"

export default function VehiculoDetalle({ params }: { params: { id: string } }) {
  const vehicleId = Number.parseInt(params.id || "0")
  const [vehicle, setVehicle] = useState<Vehicle | null>(null)
  const [relatedVehicles, setRelatedVehicles] = useState<Vehicle[]>([])
  const [activeImage, setActiveImage] = useState(0)
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  // Usar el contexto de catálogo
  const { apiVehicles, vehicles, loadCatalogo, getVehicleById } = useCatalogo()

  useEffect(() => {
    async function loadVehicleData() {
      if (isNaN(vehicleId)) {
        setError("ID de vehículo no válido")
        setIsLoading(false)
        return
      }

      setIsLoading(true)
      try {
        // Si no hay vehículos en el contexto, cargarlos
        if (apiVehicles.length === 0) {
          console.log("No hay vehículos en el contexto, cargando desde la API...")
          await loadCatalogo()
        }

        // Buscar el vehículo específico por ID en el contexto
        const foundVehicle = getVehicleById(vehicleId)

        if (!foundVehicle) {
          setError("Vehículo no encontrado")
          setIsLoading(false)
          return
        }

        setVehicle(foundVehicle)

        // Obtener vehículos relacionados (misma marca o categoría)
        const related = vehicles
          .filter(
            (v) => v.id !== foundVehicle.id && (v.brand === foundVehicle.brand || v.category === foundVehicle.category),
          )
          .slice(0, 3)

        setRelatedVehicles(related)
        setError(null)
      } catch (err) {
        console.error("Error al cargar los datos del vehículo:", err)
        setError("No se pudo cargar la información del vehículo. Por favor, inténtalo de nuevo más tarde.")
      } finally {
        setIsLoading(false)
      }
    }

    loadVehicleData()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [vehicleId]) // Solo depende de vehicleId, no de las funciones del contexto

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 pt-24 pb-16 flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto mb-4"></div>
          <h1 className="text-3xl font-bold text-gray-900 mb-4">Cargando información del vehículo</h1>
          <p className="text-gray-600 mb-6">Por favor, espera un momento...</p>
        </div>
      </div>
    )
  }

  if (error || !vehicle) {
    return (
      <div className="min-h-screen bg-gray-50 pt-24 pb-16 flex items-center justify-center">
        <div className="text-center">
          <h1 className="text-3xl font-bold text-gray-900 mb-4">{error || "Vehículo no encontrado"}</h1>
          <p className="text-gray-600 mb-6">El vehículo que estás buscando no existe o ha sido eliminado.</p>
          <Link href="/catalogo">
            <Button>Volver al catálogo</Button>
          </Link>
        </div>
      </div>
    )
  }

  // Obtener imágenes para la galería (usamos un placeholder con la marca y modelo)
  const images = [
    `/placeholder.svg?height=600&width=800&text=${vehicle.brand}+${vehicle.model}`,
    `/placeholder.svg?height=600&width=800&text=${vehicle.brand}+${vehicle.model}+Interior`,
    `/placeholder.svg?height=600&width=800&text=${vehicle.brand}+${vehicle.model}+Lateral`,
    `/placeholder.svg?height=600&width=800&text=${vehicle.brand}+${vehicle.model}+Trasera`,
  ]

  return (
    <div className="min-h-screen bg-gray-50 pt-24 pb-16">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        {/* Breadcrumbs */}
        <nav className="flex items-center text-sm text-gray-500 mb-6">
          <Link href="/" className="hover:text-primary transition-colors">
            Inicio
          </Link>
          <ChevronRight className="h-4 w-4 mx-2" />
          <Link href="/catalogo" className="hover:text-primary transition-colors">
            Catálogo
          </Link>
          <ChevronRight className="h-4 w-4 mx-2" />
          <span className="font-medium text-gray-900">{vehicle.name}</span>
        </nav>

        {/* Botón volver */}
        <div className="mb-6">
          <AnimatedLink href="/catalogo" className="inline-flex items-center text-gray-600 hover:text-primary">
            <ArrowLeft className="h-4 w-4 mr-2" />
            Volver al catálogo
          </AnimatedLink>
        </div>

        <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
          {/* Galería de imágenes */}
          <motion.div initial={{ opacity: 0, x: -20 }} animate={{ opacity: 1, x: 0 }} transition={{ duration: 0.5 }}>
            <div className="bg-white rounded-lg overflow-hidden shadow-sm border border-gray-100">
              <div className="relative h-[300px] sm:h-[400px]">
                <Image
                  src={images[activeImage] || "/placeholder.svg"}
                  alt={vehicle.name}
                  fill
                  className="object-cover"
                />
                {vehicle.category === "luxury" && <Badge className="absolute top-4 left-4 z-10 bg-primary">Lujo</Badge>}
                {vehicle.category === "standard" && (
                  <Badge className="absolute top-4 left-4 z-10 bg-secondary text-secondary-foreground">Estándar</Badge>
                )}
                {!vehicle.inStock && (
                  <Badge variant="destructive" className="absolute top-4 right-4 z-10">
                    Agotado
                  </Badge>
                )}
              </div>
              <div className="flex p-2 gap-2 overflow-x-auto">
                {images.map((image, index) => (
                  <button
                    key={index}
                    onClick={() => setActiveImage(index)}
                    className={`relative w-20 h-20 rounded overflow-hidden flex-shrink-0 border-2 transition-all ${
                      activeImage === index ? "border-primary" : "border-transparent"
                    }`}
                  >
                    <Image src={image || "/placeholder.svg"} alt={`Vista ${index + 1}`} fill className="object-cover" />
                  </button>
                ))}
              </div>
            </div>
          </motion.div>

          {/* Información del vehículo */}
          <motion.div initial={{ opacity: 0, x: 20 }} animate={{ opacity: 1, x: 0 }} transition={{ duration: 0.5 }}>
            <div className="flex justify-between items-start">
              <div>
                <h1 className="text-3xl font-bold text-gray-900">{vehicle.name}</h1>
                <p className="text-lg text-gray-600">
                  {vehicle.brand} {vehicle.model} {vehicle.year}
                </p>
              </div>
              <div className="flex space-x-2">
                <Button variant="outline" size="icon">
                  <Heart className="h-5 w-5" />
                </Button>
                <Button variant="outline" size="icon">
                  <Share className="h-5 w-5" />
                </Button>
              </div>
            </div>

            <div className="mt-6">
              <h2 className="text-3xl font-bold text-primary">
                {new Intl.NumberFormat("es-ES", {
                  style: "currency",
                  currency: "EUR",
                  maximumFractionDigits: 0,
                }).format(vehicle.price)}
              </h2>
              <p className="text-sm text-gray-500 mt-1">Precio final, impuestos incluidos</p>
            </div>

            <div className="grid grid-cols-2 gap-4 mt-6">
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Kilometraje</p>
                <p className="font-medium">{vehicle.mileage}</p>
              </div>
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Color</p>
                <p className="font-medium">{vehicle.color || "No especificado"}</p>
              </div>
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Matrícula</p>
                <p className="font-medium">{vehicle.licensePlate || "No especificada"}</p>
              </div>
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Año</p>
                <p className="font-medium">{vehicle.year}</p>
              </div>
            </div>

            {/* Información adicional */}
            <div className="mt-6">
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Número de chasis</p>
                <p className="font-medium">{vehicle.chassisNumber || "No especificado"}</p>
              </div>
            </div>

            <div className="mt-6">
              <div className="bg-gray-50 p-3 rounded-lg">
                <p className="text-sm text-gray-500">Proveedor</p>
                <p className="font-medium">{vehicle.provider?.name || "No especificado"}</p>
                {vehicle.provider && (
                  <p className="text-xs text-gray-500 mt-1">
                    {vehicle.provider.city} - {vehicle.provider.phone}
                  </p>
                )}
              </div>
            </div>

            <div className="mt-8">
              <h3 className="text-lg font-bold mb-3">Características destacadas</h3>
              <ul className="grid grid-cols-1 sm:grid-cols-2 gap-y-2">
                {vehicle.features.map((feature, index) => (
                  <li key={index} className="flex items-center">
                    <Check className="h-5 w-5 text-primary mr-2 flex-shrink-0" />
                    <span>{feature}</span>
                  </li>
                ))}
              </ul>
            </div>

            <div className="mt-8 space-y-4">
              <div className="flex items-center text-sm text-gray-600">
                <ShieldCheck className="h-5 w-5 text-primary mr-2" />
                <span>Garantía de 12 meses incluida</span>
              </div>
              <div className="flex items-center text-sm text-gray-600">
                <ShieldCheck className="h-5 w-5 text-primary mr-2" />
                <span>Vehículo revisado con 150 puntos de control</span>
              </div>
              <div className="flex items-center text-sm text-gray-600">
                <ShieldCheck className="h-5 w-5 text-primary mr-2" />
                <span>Documentación y trámites incluidos</span>
              </div>
            </div>

            <div className="mt-8">
              <RequestAppointmentDialog
                buttonVariant="default"
                buttonSize="lg"
                className="w-full"
                preselectedReason="test-drive"
                disableReasonChange={true}
                vehicleId={vehicleId} // Añadir el ID del vehículo
              >
                Solicitar una prueba
              </RequestAppointmentDialog>
            </div>
          </motion.div>
        </div>

        {/* Tabs de información */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.5, delay: 0.2 }}
          className="mt-12"
        >
          <Tabs defaultValue="descripcion">
            <TabsList className="grid w-full grid-cols-3">
              <TabsTrigger value="descripcion">Descripción</TabsTrigger>
              <TabsTrigger value="equipamiento">Equipamiento</TabsTrigger>
              <TabsTrigger value="financiacion">Financiación</TabsTrigger>
            </TabsList>
            <TabsContent value="descripcion" className="p-6 bg-white rounded-lg shadow-sm border border-gray-100 mt-4">
              <h3 className="text-lg font-bold mb-4">Descripción del vehículo</h3>
              <p className="text-gray-700 mb-4">{vehicle.description}</p>
              <p className="text-gray-700">
                Este vehículo ha sido importado directamente y cuenta con todas las garantías y certificaciones
                necesarias. Ha pasado por nuestro riguroso proceso de inspección de 150 puntos y está listo para su
                entrega inmediata.
              </p>
            </TabsContent>
            <TabsContent value="equipamiento" className="p-6 bg-white rounded-lg shadow-sm border border-gray-100 mt-4">
              <h3 className="text-lg font-bold mb-4">Equipamiento completo</h3>
              <div className="grid grid-cols-1 md:grid-cols-2 gap-x-8 gap-y-2">
                {[
                  ...vehicle.features,
                  "Climatizador automático",
                  "Sistema de navegación",
                  "Asientos calefactables",
                  "Cámara de visión trasera",
                  "Sensores de aparcamiento",
                  "Control de crucero adaptativo",
                  "Sistema de sonido premium",
                  "Techo panorámico",
                  "Llantas de aleación",
                ].map((feature, index) => (
                  <div key={index} className="flex items-center py-2 border-b border-gray-100">
                    <Check className="h-5 w-5 text-primary mr-2 flex-shrink-0" />
                    <span>{feature}</span>
                  </div>
                ))}
              </div>
            </TabsContent>
            <TabsContent value="financiacion" className="p-6 bg-white rounded-lg shadow-sm border border-gray-100 mt-4">
              <h3 className="text-lg font-bold mb-4">Opciones de financiación</h3>
              <p className="text-gray-700 mb-4">
                Ofrecemos diferentes opciones de financiación adaptadas a tus necesidades. Puedes financiar hasta el
                100% del valor del vehículo con plazos de hasta 84 meses.
              </p>
              <div className="bg-gray-50 p-4 rounded-lg mb-4">
                <h4 className="font-bold mb-2">Ejemplo de financiación:</h4>
                <ul className="space-y-2">
                  <li className="flex justify-between">
                    <span>Precio del vehículo:</span>
                    <span className="font-medium">
                      {new Intl.NumberFormat("es-ES", {
                        style: "currency",
                        currency: "EUR",
                        maximumFractionDigits: 0,
                      }).format(vehicle.price)}
                    </span>
                  </li>
                  <li className="flex justify-between">
                    <span>Entrada:</span>
                    <span className="font-medium">
                      {new Intl.NumberFormat("es-ES", {
                        style: "currency",
                        currency: "EUR",
                        maximumFractionDigits: 0,
                      }).format(vehicle.price * 0.2)}
                    </span>
                  </li>
                  <li className="flex justify-between">
                    <span>Importe a financiar:</span>
                    <span className="font-medium">
                      {new Intl.NumberFormat("es-ES", {
                        style: "currency",
                        currency: "EUR",
                        maximumFractionDigits: 0,
                      }).format(vehicle.price * 0.8)}
                    </span>
                  </li>
                  <li className="flex justify-between">
                    <span>Plazo:</span>
                    <span className="font-medium">60 meses</span>
                  </li>
                  <li className="flex justify-between">
                    <span>Cuota mensual:</span>
                    <span className="font-medium">
                      {new Intl.NumberFormat("es-ES", {
                        style: "currency",
                        currency: "EUR",
                        maximumFractionDigits: 0,
                      }).format(((vehicle.price * 0.8) / 60) * 1.15)}
                    </span>
                  </li>
                </ul>
              </div>
              <p className="text-sm text-gray-500">
                *Ejemplo orientativo. Consulta condiciones específicas con nuestro departamento financiero.
              </p>
            </TabsContent>
          </Tabs>
        </motion.div>

        {/* Vehículos relacionados */}
        {relatedVehicles.length > 0 && (
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: 0.3 }}
            className="mt-16"
          >
            <h2 className="text-2xl font-bold text-gray-900 mb-6">Vehículos similares</h2>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              {relatedVehicles.map((relatedVehicle) => (
                <CarCard key={relatedVehicle.id} car={relatedVehicle} />
              ))}
            </div>
          </motion.div>
        )}
      </div>
    </div>
  )
}
