"use client"

import { useState, useEffect } from "react"
import Link from "next/link"
import { motion } from "framer-motion"
import { User, LogOut, Settings, Heart, CreditCard, ChevronDown, Home } from "lucide-react"
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu"
import { Button } from "@/components/ui/button"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
// Importar el contexto de usuario
import { useUser } from "@/contexts/user-context"
// Importar el contexto de alertas
import { useAlert } from "@/components/custom-alert"

export function UserDropdown() {
  // Dentro de la función UserDropdown, obtener lastAuthAction del contexto
  const { user, logout, isAuthenticated, lastAuthAction, resetAuthAction } = useUser()
  const [isOpen, setIsOpen] = useState(false)
  // Obtener el contexto de alertas
  const { showAlert } = useAlert()

  // Efecto para mostrar la alerta cuando lastAuthAction cambia a "logout"
  useEffect(() => {
    if (lastAuthAction === "logout") {
      console.log("🔴 UserDropdown - lastAuthAction cambió a logout, mostrando alerta personalizada")

      // Mostrar alerta de cierre de sesión con nuestro sistema personalizado
      showAlert({
        type: "success",
        title: "Sesión cerrada",
        message: "Has cerrado sesión correctamente.",
        autoClose: true,
        autoCloseDuration: 1500,
      })

      // Restablecer lastAuthAction para evitar que se ejecute repetidamente
      resetAuthAction()
    }
  }, [lastAuthAction, showAlert, resetAuthAction])

  if (!isAuthenticated || !user) return null

  // Obtener iniciales para el avatar fallback
  const getInitials = (name: string) => {
    return name
      .split(" ")
      .map((part) => part[0])
      .join("")
      .toUpperCase()
      .substring(0, 2)
  }

  const initials = getInitials(user.name)

  // Modificar la función handleLogout para mantener solo el log de cierre de sesión
  const handleLogout = () => {
    console.log("🔴 ACCIÓN: Botón de cerrar sesión pulsado")
    setIsOpen(false)
    logout(false) // Pasamos false para asegurarnos de que no se muestre ninguna notificación
  }

  return (
    <DropdownMenu open={isOpen} onOpenChange={setIsOpen}>
      <DropdownMenuTrigger asChild>
        <Button variant="ghost" className="relative h-10 w-10 rounded-full p-0">
          <Avatar className="h-10 w-10">
            <AvatarImage src={user.avatar || "/placeholder.svg"} alt={user.name} />
            <AvatarFallback>{initials}</AvatarFallback>
          </Avatar>
          <motion.div
            animate={{ rotate: isOpen ? 180 : 0 }}
            transition={{ duration: 0.2 }}
            className="absolute -bottom-1 -right-1 flex h-4 w-4 items-center justify-center rounded-full bg-primary text-white"
          >
            <ChevronDown className="h-3 w-3" />
          </motion.div>
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent align="end" className="w-56">
        <div className="flex items-center justify-start gap-2 p-2">
          <div className="flex flex-col space-y-0.5">
            <p className="text-sm font-medium">{user.name}</p>
            <p className="text-xs text-muted-foreground">{user.email}</p>
          </div>
        </div>
        <DropdownMenuSeparator />
        <Link href="/">
          <DropdownMenuItem>
            <Home className="mr-2 h-4 w-4" />
            <span>Inicio</span>
          </DropdownMenuItem>
        </Link>
        <Link href="/perfil">
          <DropdownMenuItem>
            <User className="mr-2 h-4 w-4" />
            <span>Mi Perfil</span>
          </DropdownMenuItem>
        </Link>
        <Link href="/perfil?tab=favoritos">
          <DropdownMenuItem>
            <Heart className="mr-2 h-4 w-4" />
            <span>Favoritos</span>
          </DropdownMenuItem>
        </Link>
        <Link href="/perfil?tab=compras">
          <DropdownMenuItem>
            <CreditCard className="mr-2 h-4 w-4" />
            <span>Mis Compras</span>
          </DropdownMenuItem>
        </Link>
        <Link href="/perfil?tab=ajustes">
          <DropdownMenuItem>
            <Settings className="mr-2 h-4 w-4" />
            <span>Ajustes</span>
          </DropdownMenuItem>
        </Link>
        <DropdownMenuSeparator />
        <DropdownMenuItem onClick={handleLogout}>
          <LogOut className="mr-2 h-4 w-4" />
          <span>Cerrar Sesión</span>
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
