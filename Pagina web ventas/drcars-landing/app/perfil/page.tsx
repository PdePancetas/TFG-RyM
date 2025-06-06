"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { motion } from "framer-motion"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Card, CardContent, CardDescription, CardFooter, CardHeader, CardTitle } from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import { Separator } from "@/components/ui/separator"
import { useUser } from "@/contexts/user-context"
import { useToast } from "@/hooks/use-toast"
import {
  User,
  Mail,
  Phone,
  MapPin,
  Calendar,
  Clock,
  Heart,
  Eye,
  Settings,
  LogOut,
  Edit,
  Save,
  X,
  Home,
  FileText,
} from "lucide-react"
import { vehicles } from "@/data/vehicles"
import { CarCard } from "@/components/car-card"
import Link from "next/link"
import { useAlert } from "@/components/custom-alert"
import { Checkbox } from "@/components/ui/checkbox"
import { HistorialCitas } from "./historial-citas"

export default function PerfilPage() {
  const { user, isAuthenticated, updateUserData, logout } = useUser()
  const router = useRouter()
  const searchParams = useSearchParams()
  const tabParam = searchParams?.get("tab")

  const { toast } = useToast()
  const { showAlert } = useAlert()
  const [isEditing, setIsEditing] = useState(false)
  const [activeTab, setActiveTab] = useState("perfil")
  const [userData, setUserData] = useState({
    name: "",
    email: "",
    phone: "600123456",
    address: "Calle Principal 123, Madrid",
    dni: "", // Añadir el campo DNI
  })

  // Establecer el tab activo basado en el parámetro de URL
  useEffect(() => {
    if (tabParam && ["perfil", "favoritos", "vistos", "ajustes", "citas"].includes(tabParam)) {
      setActiveTab(tabParam)
    }
  }, [tabParam])

  // Redirigir si no está autenticado
  useEffect(() => {
    if (!isAuthenticated) {
      router.push("/")
    } else if (user) {
      setUserData({
        name: user.name,
        email: user.email,
        phone: "600123456", // Datos de ejemplo
        address: "Calle Principal 123, Madrid", // Datos de ejemplo
        dni: user.dni || "", // Añadir el campo DNI
      })
    }
  }, [isAuthenticated, router, user])

  if (!user) {
    return null
  }

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setUserData((prev) => ({ ...prev, [name]: value }))
  }

  const handleSaveProfile = () => {
    updateUserData({
      name: userData.name,
      email: userData.email,
      dni: userData.dni, // Incluir el DNI en la actualización
    })

    setIsEditing(false)

    toast({
      title: "Perfil actualizado",
      description: "Tus datos han sido actualizados correctamente.",
      variant: "default",
    })
  }

  const handleLogout = () => {
    console.log("🔴 ACCIÓN: Botón de cerrar sesión pulsado desde perfil")
    logout(false) // Pasamos false para usar nuestro sistema de alertas personalizado
    router.push("/")
  }

  const handleTabChange = (value: string) => {
    setActiveTab(value)
    router.push(`/perfil?tab=${value}`, { scroll: false })
  }

  // Obtener vehículos favoritos
  const favoriteVehicles = vehicles.filter((vehicle) => user.favorites.includes(vehicle.id))

  // Obtener vehículos vistos recientemente
  const recentlyViewedVehicles = vehicles.filter((vehicle) => user.recentViews.includes(vehicle.id))

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

  return (
    <div className="min-h-screen bg-gray-50 pt-24 pb-16">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="mb-6">
          <Link href="/">
            <Button variant="outline" className="flex items-center gap-2">
              <Home className="h-4 w-4" />
              Volver al inicio
            </Button>
          </Link>
        </div>
        <div className="flex flex-col md:flex-row gap-8">
          {/* Sidebar */}
          <div className="w-full md:w-1/4">
            <Card>
              <CardHeader className="text-center">
                <div className="flex justify-center mb-4">
                  <Avatar className="h-24 w-24">
                    <AvatarImage src={user.avatar || "/placeholder.svg"} alt={user.name} />
                    <AvatarFallback className="text-2xl">{initials}</AvatarFallback>
                  </Avatar>
                </div>
                <CardTitle>{user.name}</CardTitle>
                <CardDescription>{user.email}</CardDescription>
                {user.dni && <CardDescription className="mt-1">DNI: {user.dni}</CardDescription>}
              </CardHeader>
              <CardContent>
                <div className="space-y-4">
                  <div className="flex items-center text-sm">
                    <Calendar className="mr-2 h-4 w-4 text-muted-foreground" />
                    <span>Cliente desde {new Date(user.joinedDate).toLocaleDateString()}</span>
                  </div>
                  <div className="flex items-center text-sm">
                    <Clock className="mr-2 h-4 w-4 text-muted-foreground" />
                    <span>Último acceso {new Date(user.lastLogin).toLocaleDateString()}</span>
                  </div>
                </div>
              </CardContent>
              <CardFooter>
                <Button variant="outline" className="w-full" onClick={handleLogout}>
                  <LogOut className="mr-2 h-4 w-4" />
                  Cerrar Sesión
                </Button>
              </CardFooter>
            </Card>
          </div>

          {/* Contenido principal */}
          <div className="w-full md:w-3/4">
            <Tabs value={activeTab} onValueChange={handleTabChange}>
              <TabsList className="grid w-full grid-cols-5">
                <TabsTrigger value="perfil">
                  <User className="h-4 w-4 mr-2" />
                  <span className="hidden sm:inline">Perfil</span>
                </TabsTrigger>
                <TabsTrigger value="citas">
                  <FileText className="h-4 w-4 mr-2" />
                  <span className="hidden sm:inline">Citas</span>
                </TabsTrigger>
                <TabsTrigger value="favoritos">
                  <Heart className="h-4 w-4 mr-2" />
                  <span className="hidden sm:inline">Favoritos</span>
                </TabsTrigger>
                <TabsTrigger value="vistos">
                  <Eye className="h-4 w-4 mr-2" />
                  <span className="hidden sm:inline">Vistos</span>
                </TabsTrigger>
                <TabsTrigger value="ajustes">
                  <Settings className="h-4 w-4 mr-2" />
                  <span className="hidden sm:inline">Ajustes</span>
                </TabsTrigger>
              </TabsList>

              {/* Pestaña de Perfil */}
              <TabsContent value="perfil">
                <Card>
                  <CardHeader className="flex flex-row items-center justify-between">
                    <div>
                      <CardTitle>Información Personal</CardTitle>
                      <CardDescription>Gestiona tu información personal</CardDescription>
                    </div>
                    {isEditing ? (
                      <div className="flex gap-2">
                        <Button variant="outline" size="sm" onClick={() => setIsEditing(false)}>
                          <X className="h-4 w-4 mr-1" />
                          Cancelar
                        </Button>
                        <Button size="sm" onClick={handleSaveProfile}>
                          <Save className="h-4 w-4 mr-1" />
                          Guardar
                        </Button>
                      </div>
                    ) : (
                      <Button variant="outline" size="sm" onClick={() => setIsEditing(true)}>
                        <Edit className="h-4 w-4 mr-1" />
                        Editar
                      </Button>
                    )}
                  </CardHeader>
                  <CardContent className="space-y-6">
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                      <div className="space-y-2">
                        <Label htmlFor="name">Nombre completo</Label>
                        <div className="relative">
                          <User className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                          <Input
                            id="name"
                            name="name"
                            value={userData.name}
                            onChange={handleInputChange}
                            className="pl-10"
                            disabled={!isEditing}
                          />
                        </div>
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="email">Email</Label>
                        <div className="relative">
                          <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                          <Input
                            id="email"
                            name="email"
                            type="email"
                            value={userData.email}
                            onChange={handleInputChange}
                            className="pl-10"
                            disabled={!isEditing}
                          />
                        </div>
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="phone">Teléfono</Label>
                        <div className="relative">
                          <Phone className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                          <Input
                            id="phone"
                            name="phone"
                            value={userData.phone}
                            onChange={handleInputChange}
                            className="pl-10"
                            disabled={!isEditing}
                          />
                        </div>
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="address">Dirección</Label>
                        <div className="relative">
                          <MapPin className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                          <Input
                            id="address"
                            name="address"
                            value={userData.address}
                            onChange={handleInputChange}
                            className="pl-10"
                            disabled={!isEditing}
                          />
                        </div>
                      </div>
                      <div className="space-y-2">
                        <Label htmlFor="dni">DNI</Label>
                        <div className="relative">
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth={2}
                              d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1m-4 0a2 2 0 104 0m-5 8a2 2 0 100-4 2 2 0 000 4zm0 0c1.306 0 2.417.835 2.83 2M9 14a3.001 3.001 0 00-2.83 2M15 11h3m-3 4h2"
                            />
                          </svg>
                          <Input
                            id="dni"
                            name="dni"
                            value={userData.dni}
                            onChange={handleInputChange}
                            className="pl-10"
                            disabled={!isEditing}
                            placeholder="12345678A"
                          />
                        </div>
                      </div>
                    </div>
                  </CardContent>
                </Card>

                {/* Historial de Citas en la pestaña de perfil */}
                <HistorialCitas dni={user.dni || ""} onEditProfile={() => setIsEditing(true)} />
              </TabsContent>

              {/* Pestaña de Citas */}
              <TabsContent value="citas">
                <HistorialCitas
                  dni={user.dni || ""}
                  onEditProfile={() => {
                    setActiveTab("perfil")
                    router.push(`/perfil?tab=perfil`, { scroll: false })
                    setTimeout(() => setIsEditing(true), 100)
                  }}
                />
              </TabsContent>

              {/* Pestaña de Favoritos */}
              <TabsContent value="favoritos">
                <Card>
                  <CardHeader>
                    <CardTitle>Vehículos Favoritos</CardTitle>
                    <CardDescription>Vehículos que has marcado como favoritos</CardDescription>
                  </CardHeader>
                  <CardContent>
                    {favoriteVehicles.length > 0 ? (
                      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {favoriteVehicles.map((vehicle) => (
                          <motion.div
                            key={vehicle.id}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.3 }}
                          >
                            <CarCard car={vehicle} />
                          </motion.div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-12">
                        <Heart className="h-12 w-12 mx-auto text-gray-300 mb-4" />
                        <h3 className="text-lg font-medium text-gray-900 mb-2">No tienes favoritos</h3>
                        <p className="text-gray-500 mb-4">
                          Explora nuestro catálogo y marca como favoritos los vehículos que te interesen.
                        </p>
                        <Button variant="outline" onClick={() => router.push("/catalogo")}>
                          Explorar Catálogo
                        </Button>
                      </div>
                    )}
                  </CardContent>
                </Card>
              </TabsContent>

              {/* Pestaña de Vistos Recientemente */}
              <TabsContent value="vistos">
                <Card>
                  <CardHeader>
                    <CardTitle>Vistos Recientemente</CardTitle>
                    <CardDescription>Vehículos que has visto recientemente</CardDescription>
                  </CardHeader>
                  <CardContent>
                    {recentlyViewedVehicles.length > 0 ? (
                      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {recentlyViewedVehicles.map((vehicle) => (
                          <motion.div
                            key={vehicle.id}
                            initial={{ opacity: 0, y: 20 }}
                            animate={{ opacity: 1, y: 0 }}
                            transition={{ duration: 0.3 }}
                          >
                            <CarCard car={vehicle} />
                          </motion.div>
                        ))}
                      </div>
                    ) : (
                      <div className="text-center py-12">
                        <Eye className="h-12 w-12 mx-auto text-gray-300 mb-4" />
                        <h3 className="text-lg font-medium text-gray-900 mb-2">No has visto vehículos recientemente</h3>
                        <p className="text-gray-500 mb-4">
                          Explora nuestro catálogo para ver los vehículos disponibles.
                        </p>
                        <Button variant="outline" onClick={() => router.push("/catalogo")}>
                          Explorar Catálogo
                        </Button>
                      </div>
                    )}
                  </CardContent>
                </Card>
              </TabsContent>

              {/* Pestaña de Ajustes */}
              <TabsContent value="ajustes">
                <Card>
                  <CardHeader>
                    <CardTitle>Ajustes de Cuenta</CardTitle>
                    <CardDescription>Gestiona tus preferencias y seguridad</CardDescription>
                  </CardHeader>
                  <CardContent className="space-y-6">
                    <div>
                      <h3 className="text-lg font-medium mb-4">Cambiar Contraseña</h3>
                      <div className="space-y-4">
                        <div className="space-y-2">
                          <Label htmlFor="current-password">Contraseña actual</Label>
                          <Input id="current-password" type="password" />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="new-password">Nueva contraseña</Label>
                          <Input id="new-password" type="password" />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="confirm-password">Confirmar nueva contraseña</Label>
                          <Input id="confirm-password" type="password" />
                        </div>
                        <Button>Actualizar Contraseña</Button>
                      </div>
                    </div>

                    <Separator />

                    <div>
                      <h3 className="text-lg font-medium mb-4">Preferencias de Notificaciones</h3>
                      <div className="space-y-4">
                        <div className="flex items-center justify-between">
                          <div>
                            <p className="font-medium">Notificaciones por email</p>
                            <p className="text-sm text-gray-500">Recibe actualizaciones sobre nuevos vehículos</p>
                          </div>
                          <div className="flex items-center space-x-2">
                            <Label htmlFor="email-notifications" className="sr-only">
                              Notificaciones por email
                            </Label>
                            <Checkbox id="email-notifications" defaultChecked />
                          </div>
                        </div>
                        <div className="flex items-center justify-between">
                          <div>
                            <p className="font-medium">Notificaciones de ofertas</p>
                            <p className="text-sm text-gray-500">Recibe ofertas y promociones especiales</p>
                          </div>
                          <div className="flex items-center space-x-2">
                            <Label htmlFor="offers-notifications" className="sr-only">
                              Notificaciones de ofertas
                            </Label>
                            <Checkbox id="offers-notifications" defaultChecked />
                          </div>
                        </div>
                      </div>
                    </div>

                    <Separator />

                    <div>
                      <h3 className="text-lg font-medium mb-4">Eliminar Cuenta</h3>
                      <p className="text-sm text-gray-500 mb-4">
                        Al eliminar tu cuenta, todos tus datos serán eliminados permanentemente. Esta acción no se puede
                        deshacer.
                      </p>
                      <Button variant="destructive">Eliminar Cuenta</Button>
                    </div>
                  </CardContent>
                </Card>
              </TabsContent>
            </Tabs>
          </div>
        </div>
      </div>
    </div>
  )
}
