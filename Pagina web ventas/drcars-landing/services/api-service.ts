import { API_BASE_URL } from "@/utils/auth-constants"

// Interfaz para los datos que vienen de la API
export interface ApiVehicle {
  idVehiculo: number
  marca: string
  modelo: string
  annoFabricacion: number
  color: string
  kilometraje: number
  matricula: string
  numeroChasis: string
  transmision: string
  combustible: string
  precioCompra: number
  estado: string
  proveedor: {
    idProveedor: number
    nombre: string
    tipoProveedor: string
    telefono: string
    ciudad: string
  }
}

// Simplificar la función getCatalogo para evitar manipulación excesiva de datos
export async function getCatalogo(): Promise<ApiVehicle[]> {
  try {
    // Usar la URL base sin parámetros adicionales
    const url = `${API_BASE_URL}/catalogo`
    console.log(`🔍 Intentando obtener catálogo desde: ${url}`)

    // Realizar la petición con el header ngrok-skip-browser-warning
    console.log("⏳ Iniciando fetch...")
    const response = await fetch(url, {
      method: "GET",
      headers: {
        // Usar el valor "4" como en la imagen
        "ngrok-skip-browser-warning": "4",
      },
      cache: "no-store",
    })

    console.log(`✅ Respuesta recibida - Status: ${response.status}, OK: ${response.ok}`)

    // Verificar si la respuesta es OK
    if (!response.ok) {
      const errorText = await response.text().catch(() => "No se pudo obtener el texto del error")
      console.error(`❌ Error en la respuesta: ${errorText}`)
      throw new Error(`Error al obtener el catálogo: ${response.status} - ${errorText}`)
    }

    // Intentar obtener el texto de la respuesta primero para depuración
    const responseText = await response.text()
    console.log(`📄 Texto de respuesta recibido (primeros 200 caracteres): ${responseText.substring(0, 200)}...`)

    // Si la respuesta está vacía
    if (!responseText.trim()) {
      console.warn("⚠️ La respuesta está vacía")
      return []
    }

    // Intentar parsear el JSON
    try {
      const data = JSON.parse(responseText)

      // Verificar si data es un array
      if (Array.isArray(data)) {
        console.log(`📊 Datos recibidos: ${data.length} vehículos`)

        // Si el array está vacío
        if (data.length === 0) {
          console.warn("⚠️ El array de vehículos está vacío")
        } else {
          // Mostrar el primer vehículo para depuración
          console.log("🚗 Primer vehículo:", JSON.stringify(data[0], null, 2))
        }

        return data
      } else {
        console.error("❌ La respuesta no es un array:", data)
        return []
      }
    } catch (parseError) {
      console.error("❌ Error al parsear JSON:", parseError)
      console.error("📄 Texto que no se pudo parsear:", responseText.substring(0, 500))
      return []
    }
  } catch (error) {
    console.error("❌ Error al obtener el catálogo:", error)
    return []
  }
}

// Modificar la función testApiConnection para hacerla más robusta
export async function testApiConnection(): Promise<boolean> {
  try {
    // Usar el endpoint de catálogo que sabemos que existe
    const url = `${API_BASE_URL}/catalogo`
    console.log(`🔍 Probando conexión con: ${url}`)

    // Agregar un timeout a la petición para evitar que se quede colgada
    const controller = new AbortController()
    const timeoutId = setTimeout(() => controller.abort(), 5000) // 5 segundos de timeout

    try {
      const response = await fetch(url, {
        method: "GET",
        headers: {
          "ngrok-skip-browser-warning": "4",
        },
        signal: controller.signal,
        cache: "no-store",
      })

      // Limpiar el timeout
      clearTimeout(timeoutId)

      console.log(`✅ Respuesta de prueba - Status: ${response.status}, OK: ${response.ok}`)
      return response.ok
    } catch (fetchError) {
      // Limpiar el timeout en caso de error
      clearTimeout(timeoutId)

      if (fetchError.name === "AbortError") {
        console.error("❌ La petición excedió el tiempo de espera")
      } else {
        console.error(`❌ Error en fetch: ${fetchError.message}`)
      }
      return false
    }
  } catch (error) {
    console.error("❌ Error al probar la conexión:", error)
    return false
  }
}
