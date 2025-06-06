"use client"

import { X } from "lucide-react"
import { Button } from "@/components/ui/button"
import Link from "next/link"
import { SearchDialog } from "@/components/search-dialog"
import { AuthDialog } from "@/components/auth-dialog"
import { RequestCustomVehicleDialog } from "@/components/request-custom-vehicle-dialog"
import { RequestAppointmentDialog } from "@/components/request-appointment-dialog"
import { useUser } from "@/contexts/user-context"
import { useRouter } from "next/navigation"

interface MobileNavProps {
  onClose: () => void
}

export function MobileNav({ onClose }: MobileNavProps) {
  const router = useRouter()
  const handleLinkClick = () => {
    // Close the mobile nav when a link is clicked
    onClose()
  }

  // Dentro de la función MobileNav, añadir:
  const { isAuthenticated, logout } = useUser()

  const handleLogout = () => {
    console.log("🔴 ACCIÓN: Botón de cerrar sesión pulsado desde MobileNav")
    logout(false) // Pasamos false para usar nuestro sistema de alertas personalizado
    onClose()
    router.push("/")
  }

  return (
    <div className="fixed inset-0 bg-black/90 z-50 flex flex-col p-6">
      <div className="flex justify-end">
        <Button variant="ghost" size="icon" onClick={onClose} className="text-white">
          <X className="h-6 w-6" />
        </Button>
      </div>
      <div className="flex flex-col items-center justify-center flex-1 space-y-6">
        {/* Enlaces de navegación */}
        <div className="flex flex-col items-center space-y-6 mb-4">
          <Link href="/catalogo" className="text-white text-2xl font-bold" onClick={handleLinkClick}>
            Catálogo
          </Link>
          <Link href="/servicios" className="text-white text-2xl font-bold" onClick={handleLinkClick}>
            Servicios
          </Link>
          <Link href="/nosotros" className="text-white text-2xl font-bold" onClick={handleLinkClick}>
            Nosotros
          </Link>
          <Link href="/contacto" className="text-white text-2xl font-bold" onClick={handleLinkClick}>
            Contacto
          </Link>
        </div>

        {/* Separador */}
        <div className="w-24 h-0.5 bg-white/20 my-2"></div>

        {/* Botones de acción */}
        <div className="grid grid-cols-1 gap-4 w-full max-w-xs">
          <SearchDialog buttonVariant="default" buttonSize="lg" buttonText="Buscar Vehículos" />
          <RequestCustomVehicleDialog buttonSize="lg" className="w-full" />
          <RequestAppointmentDialog buttonSize="lg" className="w-full" />

          {/* Mostrar AuthDialog o perfil según el estado de autenticación */}
          {!isAuthenticated ? (
            <AuthDialog buttonVariant="outline" buttonSize="lg" />
          ) : (
            <>
              <Link href="/perfil" onClick={handleLinkClick}>
                <Button variant="outline" size="lg" className="w-full">
                  Mi Perfil
                </Button>
              </Link>
              <Button variant="destructive" size="lg" className="w-full" onClick={handleLogout}>
                Cerrar Sesión
              </Button>
            </>
          )}
        </div>
      </div>
    </div>
  )
}
