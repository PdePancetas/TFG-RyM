import type { ApiVehicle } from "@/services/api-service"

export interface Vehicle {
  id: number
  name: string
  brand: string
  model: string
  year: number
  price: number
  image: string
  mileage: string
  transmission: string
  fuel: string
  category: "luxury" | "standard"
  features: string[]
  description: string
  inStock: boolean
  color?: string
  licensePlate?: string
  chassisNumber?: string
  status?: string
  provider?: {
    id: number
    name: string
    type: string
    phone: string
    city: string
  }
}

// Modificar la función mapApiVehicleToVehicle para crear el nombre a partir de marca y modelo
export function mapApiVehicleToVehicle(apiVehicle: ApiVehicle): Vehicle {
  // Determinar la categoría basada en el precio
  // Asumimos que vehículos con precio > 30000 son de lujo
  const category = apiVehicle.precioCompra > 30000 ? "luxury" : "standard"

  // Crear el nombre combinando marca y modelo
  const name = `${apiVehicle.marca || ""} ${apiVehicle.modelo || ""}`.trim() || "Vehículo sin especificar"

  // Generar características basadas en los datos disponibles
  const features = [
    `Color: ${apiVehicle.color || "No especificado"}`,
    `Kilometraje: ${apiVehicle.kilometraje || 0} km`,
    `Año: ${apiVehicle.annoFabricacion || "No especificado"}`,
    `Estado: ${apiVehicle.estado || "No especificado"}`,
    `Proveedor: ${apiVehicle.proveedor?.nombre || "No especificado"}`,
  ]

  return {
    id: apiVehicle.idVehiculo,
    name: name,
    brand: apiVehicle.marca || "Sin marca",
    model: apiVehicle.modelo || "Sin modelo",
    year: apiVehicle.annoFabricacion || new Date().getFullYear(),
    price: apiVehicle.precioCompra || 0,
    image: `/placeholder.svg?height=600&width=800&text=${apiVehicle.marca || "Vehículo"}+${apiVehicle.modelo || ""}`,
    mileage: `${(apiVehicle.kilometraje || 0).toLocaleString()} km`,
    transmission: apiVehicle.transmision, 
    fuel: apiVehicle.combustible,
    category,
    features,
    description: `${apiVehicle.marca || "Vehículo"} ${apiVehicle.modelo || ""} del año ${apiVehicle.annoFabricacion || "N/A"} con ${apiVehicle.kilometraje || 0} km. Color ${apiVehicle.color || "No especificado"}.`,
    inStock: apiVehicle.estado === "VENTA",
    color: apiVehicle.color || "No especificado",
    licensePlate: apiVehicle.matricula || "No especificada",
    chassisNumber: apiVehicle.numeroChasis || "No especificado",
    status: apiVehicle.estado || "No especificado",
    provider: apiVehicle.proveedor
      ? {
          id: apiVehicle.proveedor.idProveedor,
          name: apiVehicle.proveedor.nombre || "No especificado",
          type: apiVehicle.proveedor.tipoProveedor || "No especificado",
          phone: apiVehicle.proveedor.telefono || "No especificado",
          city: apiVehicle.proveedor.ciudad || "No especificado",
        }
      : undefined,
  }
}

// Mantenemos las variables estáticas para compatibilidad con el resto de la aplicación
// pero no las usaremos para mostrar vehículos
export const vehicles: Vehicle[] = []
export const brands: string[] = []
export const models: string[] = []
export const years: number[] = []
export const fuels: string[] = []
export const transmissions: string[] = []
