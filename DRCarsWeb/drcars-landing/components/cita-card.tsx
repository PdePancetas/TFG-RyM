import { Badge } from "@/components/ui/badge"
import { formatDate } from "@/lib/utils"

interface Cita2 {
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

interface Cita {
  idReserva: number;
  fechaReserva: string;
  precioReserva: number;

  trabajador: {
    telefono: string;
    email: string;
  };

  vehiculo: {
    marca: string;
    modelo: string;
  };
}


interface CitaCardProps {
  cita: Cita
}

export function CitaCard({ cita }: CitaCardProps) {
  // Función para obtener el color del estado
  const getStatusColor = (estado: string) => {
    switch (estado?.toUpperCase()) {
      case "PENDIENTE":
        return "bg-amber-100 text-amber-800 border-amber-200"
      case "COMPLETADA":
        return "bg-green-100 text-green-800 border-green-200"
      case "CANCELADA":
        return "bg-red-100 text-red-800 border-red-200"
      default:
        return "bg-gray-100 text-gray-800 border-gray-200"
    }
  }

  // Función para formatear el motivo
  const formatMotivo = (motivo: string) => {
    switch (motivo) {
      case "test-drive":
        return "Prueba de vehículo"
      case "custom-order":
        return "Pedido personalizado"
      case "financing":
        return "Información sobre financiación"
      case "other":
        return "Otro motivo"
      default:
        return motivo || "No especificado"
    }
  }

  return (
    <div className="rounded-lg border p-4 hover:shadow-md transition-shadow">
      <div className="flex justify-between items-start">
        <div>
          <h4 className="font-medium">
            {cita.vehiculo ? `${cita.vehiculo.marca} ${cita.vehiculo.modelo}` : "Cita sin vehículo específico"}
          </h4>
          <p className="text-sm text-gray-500">{formatDate(cita.fechaReserva)}</p>
        </div>
        <div className="text-right">
          <p className="font-bold">{cita?.precioReserva ? cita.precioReserva.toLocaleString("es-ES") +"€": "Precio no disponible"}</p>
          <Badge className={"Pendiente"}>{"Pendiente"}</Badge>
        </div>
      </div>
    </div>
  )
}
