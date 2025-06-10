"use client"
import { motion } from "framer-motion"
import Image from "next/image"

export function RotatingCar() {
  // En lugar de usar frames, usaremos una imagen estática de alta calidad
  const heroImage = "https://images.unsplash.com/photo-1503376780353-7e6692767b70?q=80&w=2070&auto=format&fit=crop"

  return (
    <div className="relative w-full h-full">
      <motion.div
        initial={{ opacity: 0, scale: 0.8 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: 1 }}
        className="relative w-full h-full"
      >
        <Image src={heroImage || "/placeholder.svg"} alt="Vehículo de lujo" fill className="object-cover" priority />

        {/* Efecto de reflejo */}
        <div className="absolute bottom-0 left-0 right-0 h-20 bg-gradient-to-t from-black/20 to-transparent" />
      </motion.div>
    </div>
  )
}
