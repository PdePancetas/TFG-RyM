// Clave para el hash (no se utiliza directamente en SHA-256 pero se incluye por si se necesita en el futuro)
export const HASH_KEY = "DRCars-Auth-Key-2024"

// URL base para todas las llamadas a la API
export const API_BASE_URL = "https://helped-bug-stirring.ngrok-free.app"

// Endpoint para el registro
export const REGISTRO_ENDPOINT = `${API_BASE_URL}/auth/registro`

// Función para generar un valor aleatorio para el header de ngrok
function generateRandomValue() {
  return Math.random().toString(36).substring(2, 15)
}

// Actualizar la función enviarDatosRegistro para usar el formato de timestamp correcto
export async function enviarDatosRegistro(datos: any) {
  try {
    // Generar timestamps actuales en el formato exacto requerido: YYYY-MM-DDTHH:MM:SS
    const now = new Date()
    const year = now.getFullYear()
    const month = String(now.getMonth() + 1).padStart(2, "0")
    const day = String(now.getDate()).padStart(2, "0")
    const hours = String(now.getHours()).padStart(2, "0")
    const minutes = String(now.getMinutes()).padStart(2, "0")
    const seconds = String(now.getSeconds()).padStart(2, "0")

    const currentTimestamp = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`

    // Asegurarse de que los datos incluyan los timestamps en el formato correcto
    const datosConTimestamp = {
      ...datos,
      ultimo_acceso: currentTimestamp,
      registro_cuenta: currentTimestamp,
    }

    const response = await fetch(`${API_BASE_URL}/registro`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        "ngrok-skip-browser-warning": generateRandomValue(), // Añadir este header con valor aleatorio
      },
      body: JSON.stringify(datosConTimestamp),
    })

    // Imprimir la respuesta completa para depuración
    console.log("Respuesta del servidor:", response.status, await response.text().catch(() => "No body"))

    return response
  } catch (error) {
    console.error("Error al enviar datos de registro:", error)
    throw error
  }
}
