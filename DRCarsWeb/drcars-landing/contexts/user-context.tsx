"use client"

import { createContext, useContext, useState, useEffect, type ReactNode } from "react"

// Modificar la interfaz UserData para incluir el campo dni
export interface UserData {
  id: string
  name: string
  email: string
  avatar?: string
  address?:string
  phone?:string
  totalSpent: number
  joinedDate: string
  lastLogin: string
  favorites: number[]
  recentViews: number[]
  dni?: string // Añadir el campo dni como opcional
}

// Modificar la interfaz UserContextType para incluir un nuevo estado
interface UserContextType {
  user: UserData | null
  isLoading: boolean
  isAuthenticated: boolean
  login: (email: string, password: string, remember?: boolean, responseString?: string) => Promise<boolean>
  logout: (showNotification?: boolean) => void
  updateUserData: (data: Partial<UserData>) => void
  getSavedCredentials: () => { email: string; password: string } | null
  lastAuthAction: "login" | "logout" | null
  resetAuthAction: () => void
}

const UserContext = createContext<UserContextType | undefined>(undefined)

// Función para guardar credenciales en localStorage
const saveCredentials = (email: string, password: string, remember: boolean) => {
  if (remember) {
    localStorage.setItem("drcars-credentials", JSON.stringify({ email, password, remember }))
  } else {
    localStorage.removeItem("drcars-credentials")
  }
}

// Función para obtener credenciales guardadas
const getSavedCredentials = () => {
  const saved = localStorage.getItem("drcars-credentials")
  if (saved) {
    return JSON.parse(saved)
  }
  return null
}

// Función para generar un avatar basado en las iniciales del email
const generateAvatar = (email: string) => {
  const name = email.split("@")[0]
  const initials = name.substring(0, 2).toUpperCase()
  return `/placeholder.svg?height=200&width=200&text=${initials}`
}

// Dummy function to satisfy the linter.  Replace with actual implementation.
const setupInactivityDetection = () => {
  console.warn("setupInactivityDetection is a placeholder function. Implement actual inactivity detection.")
}

export function UserProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<UserData | null>(null)
  const [isLoading, setIsLoading] = useState<boolean>(true)
  const [lastAuthAction, setLastAuthAction] = useState<"login" | "logout" | null>(null)

  // Comprobar si hay un usuario en localStorage al cargar
  useEffect(() => {
    const storedUserLocal = localStorage.getItem("drcars-user")
    const storedUserSession = sessionStorage.getItem("drcars-user")

    if (storedUserLocal) {
      setUser(JSON.parse(storedUserLocal))
    } else if (storedUserSession) {
      setUser(JSON.parse(storedUserSession))
    }

    setIsLoading(false)
    setupInactivityDetection()
  }, [])

  // Función login simplificada
  const login = async (
    email: string,
    password: string,
    remember: boolean = false,
    responseString?: string,
  ): Promise<boolean> => {
    setIsLoading(true)

    
    let extractedDNI: string = "" ;
    if (responseString && responseString.toLowerCase().includes("cliente")) {
      const words = responseString.trim().split(/\s+/)
      if (words.length > 0) {
        const lastWord = words[words.length - 1]
        extractedDNI = lastWord.replace(/[^a-zA-Z0-9]/g, "")
        console.log("🆔 DNI extraído:", extractedDNI)
      }
    }

    // Generamos un nombre a partir del email
    const name = email
      .split("@")[0]
      .split(".")
      .map((part) => part.charAt(0).toUpperCase() + part.slice(1))
      .join(" ")

    // Creamos un usuario con los datos proporcionados
    const userData: UserData = {
      id: `user_${Date.now()}`,
      name: name,
      email: email,
      avatar: generateAvatar(email),
      totalSpent: Math.floor(Math.random() * 200000),
      joinedDate: new Date().toISOString().split("T")[0],
      lastLogin: new Date().toISOString().split("T")[0],
      favorites: [1, 3, 8],
      recentViews: [2, 5, 7, 12],
      dni: extractedDNI, // Usar el DNI extraído o undefined
    }

    setUser(userData)
    setLastAuthAction("login")

    // Guardar en sessionStorage o localStorage según corresponda
    if (remember) {
      localStorage.setItem("drcars-user", JSON.stringify(userData))
      saveCredentials(email, password, remember)
    } else {
      sessionStorage.setItem("drcars-user", JSON.stringify(userData))
      localStorage.removeItem("drcars-credentials")
    }

    setIsLoading(false)
    return true
  }

  // Implementación de la función logout
  const logout = (showNotification?: boolean) => {
    setUser(null)
    localStorage.removeItem("drcars-user")
    sessionStorage.removeItem("drcars-user")
    setLastAuthAction("logout")
  }

  // Función updateUserData
  const updateUserData = (data: Partial<UserData>) => {
    if (user) {
      const updatedUser = { ...user, ...data }
      setUser(updatedUser)

      // Actualizar en localStorage o sessionStorage según corresponda
      if (localStorage.getItem("drcars-user")) {
        localStorage.setItem("drcars-user", JSON.stringify(updatedUser))
      } else if (sessionStorage.getItem("drcars-user")) {
        sessionStorage.setItem("drcars-user", JSON.stringify(updatedUser))
      }
    }
  }

  const resetAuthAction = () => {
    setLastAuthAction(null)
  }

  return (
    <UserContext.Provider
      value={{
        user,
        isLoading,
        isAuthenticated: !!user,
        login,
        logout,
        updateUserData,
        getSavedCredentials,
        lastAuthAction,
        resetAuthAction,
      }}
    >
      {children}
    </UserContext.Provider>
  )
}

export function useUser() {
  const context = useContext(UserContext)
  if (context === undefined) {
    throw new Error("useUser must be used within a UserProvider")
  }
  return context
}
