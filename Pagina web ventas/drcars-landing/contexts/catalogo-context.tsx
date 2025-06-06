"use client"

import { createContext, useContext, useState, useCallback, useEffect, type ReactNode } from "react"
import { type ApiVehicle, getCatalogo, testApiConnection } from "@/services/api-service"
import { type Vehicle, mapApiVehicleToVehicle } from "@/data/vehicles"
import { API_BASE_URL } from "@/utils/auth-constants"

interface CatalogoContextType {
  apiVehicles: ApiVehicle[]
  vehicles: Vehicle[]
  isLoading: boolean
  error: string | null
  loadCatalogo: () => Promise<void>
  getVehicleById: (id: number) => Vehicle | null
  isApiConnected: boolean
  testConnection: () => Promise<boolean>
  ngrokWarningDetected: boolean
  ngrokUrl: string
}

const CatalogoContext = createContext<CatalogoContextType | undefined>(undefined)

export function CatalogoProvider({ children }: { children: ReactNode }) {
  const [apiVehicles, setApiVehicles] = useState<ApiVehicle[]>([])
  const [vehicles, setVehicles] = useState<Vehicle[]>([])
  const [isLoading, setIsLoading] = useState<boolean>(false)
  const [error, setError] = useState<string | null>(null)
  const [isApiConnected, setIsApiConnected] = useState<boolean>(false)
  const [ngrokWarningDetected, setNgrokWarningDetected] = useState<boolean>(false)
  const ngrokUrl = API_BASE_URL
  const [connectionStatus, setConnectionStatus] = useState<"checking" | "connected" | "disconnected">("checking")

  // Usar useCallback para evitar recrear la función en cada renderizado
  const loadCatalogo = useCallback(async () => {
    setIsLoading(true)
    setError(null)
    setNgrokWarningDetected(false)

    try {
      console.log("🚀 Cargando catálogo desde la API...")
      const data = await getCatalogo()

      if (data.length === 0) {
        console.warn("⚠️ No se recibieron vehículos de la API")
        setError("No se encontraron vehículos en el catálogo. Por favor, inténtalo de nuevo más tarde.")
      } else {
        console.log(`✅ Recibidos ${data.length} vehículos de la API`)
        setApiVehicles(data)

        // Convertir los vehículos de la API al formato de la aplicación
        const mappedVehicles = data.map(mapApiVehicleToVehicle)
        setVehicles(mappedVehicles)
        console.log(`🔄 Convertidos ${mappedVehicles.length} vehículos al formato de la aplicación`)
      }
    } catch (err: any) {
      console.error("❌ Error al cargar el catálogo:", err)

      // Detectar si es el error específico de ngrok
      if (err.message === "NGROK_WARNING_PAGE") {
        setNgrokWarningDetected(true)
        setError(
          "Se ha detectado la página de advertencia de ngrok. Por favor, sigue las instrucciones para continuar.",
        )
      } else {
        setError("No se pudo cargar el catálogo. Por favor, inténtalo de nuevo más tarde.")
      }
    } finally {
      setIsLoading(false)
    }
  }, []) // Sin dependencias para que la referencia sea estable

  // Modificar la función testConnection en el contexto para manejar mejor los errores
  const testConnection = useCallback(async () => {
    try {
      console.log("🔄 Probando conexión con la API...")
      const isConnected = await testApiConnection()
      console.log(`🔄 Resultado de la prueba de conexión: ${isConnected ? "Conectado" : "Desconectado"}`)
      setIsApiConnected(isConnected)
      return isConnected
    } catch (err) {
      console.error("❌ Error inesperado al probar la conexión:", err)
      setIsApiConnected(false)
      return false
    }
  }, [])

  // Modificar el useEffect para manejar mejor los errores de conexión
  useEffect(() => {
    const checkConnection = async () => {
      try {
        setConnectionStatus("checking")
        const isConnected = await testConnection()
        setConnectionStatus(isConnected ? "connected" : "disconnected")

        if (isConnected) {
          loadCatalogo().catch((err) => {
            console.error("❌ Error al cargar el catálogo después de conectar:", err)
          })
        }
      } catch (err) {
        console.error("❌ Error inesperado al verificar la conexión:", err)
        setConnectionStatus("disconnected")
      }
    }

    checkConnection()
  }, [testConnection, loadCatalogo])

  // Usar useCallback también para getVehicleById
  const getVehicleById = useCallback(
    (id: number): Vehicle | null => {
      const apiVehicle = apiVehicles.find((v) => v.idVehiculo === id)
      if (!apiVehicle) return null

      return mapApiVehicleToVehicle(apiVehicle)
    },
    [apiVehicles], // Solo depende de apiVehicles
  )

  const value = {
    apiVehicles,
    vehicles,
    isLoading,
    error,
    loadCatalogo,
    getVehicleById,
    isApiConnected,
    testConnection,
    ngrokWarningDetected,
    ngrokUrl,
  }

  return <CatalogoContext.Provider value={value}>{children}</CatalogoContext.Provider>
}

export function useCatalogo() {
  const context = useContext(CatalogoContext)
  if (context === undefined) {
    throw new Error("useCatalogo debe ser usado dentro de un CatalogoProvider")
  }
  return context
}
