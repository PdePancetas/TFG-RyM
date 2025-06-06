"use client"

import type React from "react"

import { useState, useEffect, useRef } from "react"
import { motion } from "framer-motion"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Eye, EyeOff, Lock, Mail } from "lucide-react"
import { Checkbox } from "@/components/ui/checkbox"
// Importar el contexto de usuario
import { useUser } from "@/contexts/user-context"
import { API_BASE_URL } from "@/utils/auth-constants"
// Importar nuestro sistema de alertas personalizado
import { useAlert } from "@/components/custom-alert"

// Función para generar un valor aleatorio para el header de ngrok
function generateRandomValue() {
  return Math.random().toString(36).substring(2, 15)
}

// Función para enviar datos de registro
async function enviarDatosRegistro(email: string, password: string): Promise<{ success: boolean; error?: string }> {
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

    // Crear el objeto JSON con el formato requerido
    const datosRegistro = {
      usuario: email,
      contraseña: password,
      tipoUsuario: "USER",
      ultimo_acceso: currentTimestamp,
      registro_cuenta: currentTimestamp,
    }

    console.log("Enviando datos de registro:", datosRegistro)

    // Enviar al endpoint
    try {
      const response = await fetch(`${API_BASE_URL}/auth/registro`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "ngrok-skip-browser-warning": generateRandomValue(),
        },
        body: JSON.stringify(datosRegistro),
      })

      // Imprimir la respuesta completa para depuración
      console.log("Respuesta del servidor:", response.status, await response.text().catch(() => "No body"))

      if (response.ok) {
        return { success: true }
      } else {
        const errorData = await response.json().catch(() => ({ message: "Error desconocido del servidor" }))
        return { success: false, error: errorData.message || "Error en el servidor" }
      }
    } catch (error) {
      console.error("Error al conectar con el endpoint:", error)
      return {
        success: false,
        error: "Error de conexión con el servidor. Verifica que el servidor esté en funcionamiento.",
      }
    }
  } catch (error) {
    console.error("Error al enviar datos de registro:", error)
    return { success: false, error: "Error al procesar los datos de registro" }
  }
}

// Función para enviar datos de login - LA ORIGINAL QUE FUNCIONABA
async function enviarDatosLogin(
  email: string,
  password: string,
): Promise<{
  success: boolean
  error?: string
  errorType?: "user_not_found" | "invalid_password" | "server_error"
  responseString?: string
}> {
  try {
    // Generar timestamp en el formato exacto requerido
    const now = new Date()
    const year = now.getFullYear()
    const month = String(now.getMonth() + 1).padStart(2, "0")
    const day = String(now.getDate()).padStart(2, "0")
    const hours = String(now.getHours()).padStart(2, "0")
    const minutes = String(now.getMinutes()).padStart(2, "0")
    const seconds = String(now.getSeconds()).padStart(2, "0")

    const currentTimestamp = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`

    // Crear el objeto JSON con el formato requerido
    const datosLogin = {
      usuario: email,
      contraseña: password,
      tipoUsuario: "USER",
      ultimo_acceso: currentTimestamp,
      registro_cuenta: "",
    }

    console.log("Enviando datos de login:", datosLogin)

    // Enviar al endpoint de login
    try {
      const response = await fetch(`${API_BASE_URL}/auth/login`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          "ngrok-skip-browser-warning": generateRandomValue(),
        },
        body: JSON.stringify(datosLogin),
      })

      // Obtener la respuesta como string
      const responseString = await response.text()
      console.log("Respuesta del servidor:", responseString)

      if (response.ok) {
        return { success: true, responseString: responseString }
      } else {
        // Determinar el tipo de error basado en el mensaje
        let errorType: "user_not_found" | "invalid_password" | "server_error" = "server_error"

        if (
          responseString.toLowerCase().includes("usuario no encontrado") ||
          responseString.toLowerCase().includes("user not found")
        ) {
          errorType = "user_not_found"
        } else if (
          responseString.toLowerCase().includes("contraseña incorrecta") ||
          responseString.toLowerCase().includes("invalid password") ||
          responseString.toLowerCase().includes("password incorrect")
        ) {
          errorType = "invalid_password"
        }

        return {
          success: false,
          error: responseString,
          errorType: errorType,
        }
      }
    } catch (error) {
      console.error("Error al conectar con el endpoint de login:", error)
      return {
        success: false,
        error: "Error de conexión con el servidor. Verifica que el servidor esté en funcionamiento.",
        errorType: "server_error",
      }
    }
  } catch (error) {
    console.error("Error al enviar datos de login:", error)
    return {
      success: false,
      error: "Error al procesar los datos de login",
      errorType: "server_error",
    }
  }
}

interface AuthDialogProps {
  buttonVariant?: "default" | "outline" | "ghost" | "link"
  buttonSize?: "default" | "sm" | "lg" | "icon"
  isOpen?: boolean
  onOpenChange?: (open: boolean) => void
  redirectAfterLogin?: boolean
  showSuccessPopup?: boolean
}

export function AuthDialog({
  buttonVariant = "outline",
  buttonSize = "default",
  isOpen,
  onOpenChange,
  redirectAfterLogin = true,
  showSuccessPopup = true,
}: AuthDialogProps) {
  const [showPassword, setShowPassword] = useState(false)
  const [isLoading, setIsLoading] = useState(false)
  const [open, setOpen] = useState(isOpen || false)
  const [loginData, setLoginData] = useState({ email: "", password: "" })
  const [registerData, setRegisterData] = useState({ email: "", password: "" })
  const [rememberMe, setRememberMe] = useState(false)
  const [fieldErrors, setFieldErrors] = useState<{
    email?: string
    password?: string
  }>({})

  const shouldShowPopupRef = useRef(showSuccessPopup)
  const { login, isAuthenticated, getSavedCredentials, lastAuthAction, resetAuthAction } = useUser()
  const { showAlert } = useAlert()

  // Actualizar la referencia cuando cambia el prop
  useEffect(() => {
    shouldShowPopupRef.current = showSuccessPopup
  }, [showSuccessPopup])

  // Sincronizar el estado open con la prop isOpen
  useEffect(() => {
    if (isOpen !== undefined) {
      setOpen(isOpen)
    }
  }, [isOpen])

  // Notificar cambios en el estado open
  useEffect(() => {
    if (onOpenChange) {
      onOpenChange(open)
    }
  }, [open, onOpenChange])

  // Cerrar el diálogo si el usuario ya está autenticado
  useEffect(() => {
    if (isAuthenticated && redirectAfterLogin) {
      setOpen(false)
    }
  }, [isAuthenticated, redirectAfterLogin])

  // Cargar credenciales guardadas al montar el componente
  useEffect(() => {
    const savedCredentials = getSavedCredentials()
    if (savedCredentials) {
      setLoginData({
        email: savedCredentials.email,
        password: savedCredentials.password,
      })
      setRememberMe(true)
    } else {
      setLoginData({ email: "", password: "" })
      setRememberMe(false)
    }
  }, [getSavedCredentials])

  // Mostrar alerta de éxito cuando lastAuthAction cambia a "login"
  useEffect(() => {
    if (lastAuthAction === "login" && redirectAfterLogin) {
      if (shouldShowPopupRef.current) {
        showAlert({
          type: "success",
          title: "¡Inicio de sesión exitoso!",
          message: "Has iniciado sesión correctamente.",
          autoClose: true,
          autoCloseDuration: 1500,
        })
      }
      resetAuthAction()
    }
  }, [lastAuthAction, showAlert, resetAuthAction, redirectAfterLogin])

  const handleLoginChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setLoginData((prev) => ({ ...prev, [name]: value }))
  }

  const handleRegisterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target
    setRegisterData((prev) => ({ ...prev, [name]: value }))
  }

  // Función handleLogin simplificada
  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault()
    console.log("🟢 ACCIÓN: Botón de iniciar sesión pulsado")

    setFieldErrors({})

    if (!loginData.email || !loginData.password) {
      showAlert({
        type: "error",
        title: "Error",
        message: "Por favor, completa todos los campos.",
        autoClose: true,
        autoCloseDuration: 1500,
      })
      return
    }

    setIsLoading(true)

    try {
      // Llamar a la API con la función original
      const result = await enviarDatosLogin(loginData.email, loginData.password)

      if (!result.success) {
        // Manejar errores como antes
        if (result.errorType === "invalid_password") {
          setFieldErrors({ password: "La contraseña no es correcta" })
        } else if (result.errorType === "user_not_found") {
          setFieldErrors({ email: "Usuario no encontrado" })
        } else {
          showAlert({
            type: "error",
            title: "Error",
            message: result.error || "No se pudo iniciar sesión. Por favor, inténtalo de nuevo.",
            autoClose: true,
            autoCloseDuration: 1500,
          })
        }
        setIsLoading(false)
        return
      }

      // Login exitoso - llamar al contexto con la respuesta
      await login(loginData.email, loginData.password, rememberMe, result.responseString)
    } catch (error) {
      console.error("Error durante el inicio de sesión:", error)
      showAlert({
        type: "error",
        title: "Error",
        message: "Ha ocurrido un error. Inténtalo de nuevo más tarde.",
        autoClose: true,
        autoCloseDuration: 1500,
      })
    } finally {
      setIsLoading(false)
    }
  }

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault()

    if (!registerData.email || !registerData.password) {
      showAlert({
        type: "error",
        title: "Error",
        message: "Por favor, completa todos los campos.",
        autoClose: true,
        autoCloseDuration: 1500,
      })
      return
    }

    setIsLoading(true)

    try {
      const result = await enviarDatosRegistro(registerData.email, registerData.password)

      if (!result.success) {
        showAlert({
          type: "error",
          title: "Error",
          message: result.error || "No se pudo registrar el usuario en el sistema. Por favor, inténtalo de nuevo.",
          autoClose: true,
          autoCloseDuration: 1500,
        })
        setIsLoading(false)
        return
      }

      if (redirectAfterLogin) {
        setOpen(false)
      }

      await login(registerData.email, registerData.password, false)
    } catch (error) {
      console.error("Error durante el registro:", error)
      showAlert({
        type: "error",
        title: "Error",
        message: "Error de conexión con el servidor. Verifica que el servidor esté en funcionamiento.",
        autoClose: true,
        autoCloseDuration: 1500,
      })
    } finally {
      setIsLoading(false)
    }
  }

  const buttonVariants = {
    initial: { scale: 1 },
    hover: {
      scale: 1.05,
      transition: {
        type: "spring",
        stiffness: 400,
        damping: 10,
      },
    },
    tap: { scale: 0.95 },
  }

  if (isAuthenticated && isOpen === undefined) {
    return null
  }

  if (isOpen !== undefined) {
    return (
      <Dialog open={open} onOpenChange={setOpen}>
        <DialogContent className="sm:max-w-[425px] max-h-[85vh] overflow-y-auto">
          <DialogHeader>
            <DialogTitle className="text-2xl font-bold text-center">Bienvenido a DRCars</DialogTitle>
          </DialogHeader>

          <Tabs defaultValue="login" className="mt-6">
            <TabsList className="grid w-full grid-cols-2">
              <TabsTrigger value="login">Iniciar Sesión</TabsTrigger>
              <TabsTrigger value="register">Registrarse</TabsTrigger>
            </TabsList>

            <TabsContent value="login">
              <form onSubmit={handleLogin} className="space-y-4 mt-4">
                <div className="space-y-2">
                  <Label htmlFor="email">Email</Label>
                  <div className="relative">
                    <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <Input
                      id="email"
                      name="email"
                      type="email"
                      placeholder="tu@email.com"
                      className={`pl-10 ${fieldErrors.email ? "border-red-500" : ""}`}
                      required
                      value={loginData.email}
                      onChange={handleLoginChange}
                    />
                  </div>
                  {fieldErrors.email && <p className="text-sm text-red-500">{fieldErrors.email}</p>}
                </div>

                <div className="space-y-2">
                  <div className="flex justify-between items-center">
                    <Label htmlFor="password">Contraseña</Label>
                    <Button variant="link" size="sm" className="p-0 h-auto text-xs">
                      ¿Olvidaste tu contraseña?
                    </Button>
                  </div>
                  <div className="relative">
                    <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <Input
                      id="password"
                      name="password"
                      type={showPassword ? "text" : "password"}
                      placeholder="••••••••"
                      className={`pl-10 ${fieldErrors.password ? "border-red-500" : ""}`}
                      required
                      value={loginData.password}
                      onChange={handleLoginChange}
                    />
                    <button
                      type="button"
                      className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                    </button>
                  </div>
                  {fieldErrors.password && <p className="text-sm text-red-500">{fieldErrors.password}</p>}
                  <p className="text-xs text-gray-500 mt-1">Para probar: Ingresa cualquier email y contraseña</p>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="remember"
                    checked={rememberMe}
                    onCheckedChange={(checked) => setRememberMe(!!checked)}
                  />
                  <label
                    htmlFor="remember"
                    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                  >
                    Recordarme
                  </label>
                </div>

                <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
                  <Button type="submit" className="w-full" disabled={isLoading}>
                    {isLoading ? "Iniciando sesión..." : "Iniciar Sesión"}
                  </Button>
                </motion.div>
              </form>
            </TabsContent>

            <TabsContent value="register">
              <form onSubmit={handleRegister} className="space-y-4 mt-4">
                <div className="space-y-2">
                  <Label htmlFor="email-register">Email</Label>
                  <div className="relative">
                    <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <Input
                      id="email-register"
                      name="email"
                      type="email"
                      placeholder="tu@email.com"
                      className="pl-10"
                      required
                      value={registerData.email}
                      onChange={handleRegisterChange}
                    />
                  </div>
                </div>

                <div className="space-y-2">
                  <Label htmlFor="password-register">Contraseña</Label>
                  <div className="relative">
                    <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <Input
                      id="password-register"
                      name="password"
                      type={showPassword ? "text" : "password"}
                      placeholder="••••••••"
                      className="pl-10"
                      required
                      value={registerData.password}
                      onChange={handleRegisterChange}
                    />
                    <button
                      type="button"
                      className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400"
                      onClick={() => setShowPassword(!showPassword)}
                    >
                      {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                    </button>
                  </div>
                  <p className="text-xs text-gray-500 mt-1">Para probar: Ingresa cualquier email y contraseña</p>
                </div>

                <div className="flex items-center space-x-2">
                  <Checkbox id="terms" required />
                  <label
                    htmlFor="terms"
                    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                  >
                    Acepto los{" "}
                    <a href="/terminos" className="text-primary hover:underline">
                      términos y condiciones
                    </a>
                  </label>
                </div>

                <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
                  <Button type="submit" className="w-full" disabled={isLoading}>
                    {isLoading ? "Registrando..." : "Crear Cuenta"}
                  </Button>
                </motion.div>
              </form>
            </TabsContent>
          </Tabs>
        </DialogContent>
      </Dialog>
    )
  }

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
          <Button variant={buttonVariant} size={buttonSize}>
            Iniciar Sesión
          </Button>
        </motion.div>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px] max-h-[85vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle className="text-2xl font-bold text-center">Bienvenido a DRCars</DialogTitle>
        </DialogHeader>

        <Tabs defaultValue="login" className="mt-6">
          <TabsList className="grid w-full grid-cols-2">
            <TabsTrigger value="login">Iniciar Sesión</TabsTrigger>
            <TabsTrigger value="register">Registrarse</TabsTrigger>
          </TabsList>

          <TabsContent value="login">
            <form onSubmit={handleLogin} className="space-y-4 mt-4">
              <div className="space-y-2">
                <Label htmlFor="email">Email</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                    id="email"
                    name="email"
                    type="email"
                    placeholder="tu@email.com"
                    className={`pl-10 ${fieldErrors.email ? "border-red-500" : ""}`}
                    required
                    value={loginData.email}
                    onChange={handleLoginChange}
                  />
                </div>
                {fieldErrors.email && <p className="text-sm text-red-500">{fieldErrors.email}</p>}
              </div>

              <div className="space-y-2">
                <div className="flex justify-between items-center">
                  <Label htmlFor="password">Contraseña</Label>
                  <Button variant="link" size="sm" className="p-0 h-auto text-xs">
                    ¿Olvidaste tu contraseña?
                  </Button>
                </div>
                <div className="relative">
                  <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                    id="password"
                    name="password"
                    type={showPassword ? "text" : "password"}
                    placeholder="••••••••"
                    className={`pl-10 ${fieldErrors.password ? "border-red-500" : ""}`}
                    required
                    value={loginData.password}
                    onChange={handleLoginChange}
                  />
                  <button
                    type="button"
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                  </button>
                </div>
                {fieldErrors.password && <p className="text-sm text-red-500">{fieldErrors.password}</p>}
                <p className="text-xs text-gray-500 mt-1">Para probar: Ingresa cualquier email y contraseña</p>
              </div>

              <div className="flex items-center space-x-2">
                <Checkbox id="remember" checked={rememberMe} onCheckedChange={(checked) => setRememberMe(!!checked)} />
                <label
                  htmlFor="remember"
                  className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                >
                  Recordarme
                </label>
              </div>

              <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
                <Button type="submit" className="w-full" disabled={isLoading}>
                  {isLoading ? "Iniciando sesión..." : "Iniciar Sesión"}
                </Button>
              </motion.div>
            </form>
          </TabsContent>

          <TabsContent value="register">
            <form onSubmit={handleRegister} className="space-y-4 mt-4">
              <div className="space-y-2">
                <Label htmlFor="email-register">Email</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                    id="email-register"
                    name="email"
                    type="email"
                    placeholder="tu@email.com"
                    className="pl-10"
                    required
                    value={registerData.email}
                    onChange={handleRegisterChange}
                  />
                </div>
              </div>

              <div className="space-y-2">
                <Label htmlFor="password-register">Contraseña</Label>
                <div className="relative">
                  <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                  <Input
                    id="password-register"
                    name="password"
                    type={showPassword ? "text" : "password"}
                    placeholder="••••••••"
                    className="pl-10"
                    required
                    value={registerData.password}
                    onChange={handleRegisterChange}
                  />
                  <button
                    type="button"
                    className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400"
                    onClick={() => setShowPassword(!showPassword)}
                  >
                    {showPassword ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
                  </button>
                </div>
                <p className="text-xs text-gray-500 mt-1">Para probar: Ingresa cualquier email y contraseña</p>
              </div>

              <div className="flex items-center space-x-2">
                <Checkbox id="terms" required />
                <label
                  htmlFor="terms"
                  className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                >
                  Acepto los{" "}
                  <a href="/terminos" className="text-primary hover:underline">
                    términos y condiciones
                  </a>
                </label>
              </div>

              <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
                <Button type="submit" className="w-full" disabled={isLoading}>
                  {isLoading ? "Registrando..." : "Crear Cuenta"}
                </Button>
              </motion.div>
            </form>
          </TabsContent>
        </Tabs>
      </DialogContent>
    </Dialog>
  )
}
