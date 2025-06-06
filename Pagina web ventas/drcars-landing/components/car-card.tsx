"use client"

import Image from "next/image"
import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardFooter } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { motion } from "framer-motion"
import type { Vehicle } from "@/data/vehicles"

interface CarCardProps {
  car: Vehicle
}

export function CarCard({ car }: CarCardProps) {
  // Usar la imagen del coche o un placeholder con la marca y modelo
  const carImage = `/placeholder.svg?height=600&width=800&text=${car.brand}+${car.model}`

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

  return (
    <Card className="overflow-hidden group">
      <div className="relative h-48 overflow-hidden">
        <Image
          src={carImage || "/placeholder.svg"}
          alt={car.name}
          fill
          className="object-cover transition-transform duration-300 group-hover:scale-105"
        />
        <div className="absolute inset-0 bg-gradient-to-t from-black/60 to-transparent opacity-0 group-hover:opacity-100 transition-opacity duration-300" />

        {/* Badges */}
        <div className="absolute top-2 left-2 flex flex-col gap-1">
          <Badge className={car.category === "luxury" ? "bg-primary" : "bg-secondary text-secondary-foreground"}>
            {car.category === "luxury" ? "Lujo" : "Estándar"}
          </Badge>

          {!car.inStock && <Badge variant="destructive">Agotado</Badge>}

          {/* Mostrar el color si está disponible */}
          {car.color && (
            <Badge variant="outline" className="bg-white/80">
              {car.color}
            </Badge>
          )}
        </div>
      </div>
      <CardContent className="p-4">
        <div className="flex justify-between items-start mb-2">
          <h3 className="font-bold text-lg">{car.name}</h3>
          <span className="text-sm text-gray-500">{car.year}</span>
        </div>
        <div className="flex items-center gap-4 text-sm text-gray-500 mb-2">
          <span>{car.mileage}</span>
          {car.transmission && (
            <>
              <span>•</span>
              <span>{car.transmission}</span>
            </>
          )}
          {car.fuel && (
            <>
              <span>•</span>
              <span>{car.fuel}</span>
            </>
          )}
        </div>
        <div className="font-bold text-xl text-primary">
          {new Intl.NumberFormat("es-ES", {
            style: "currency",
            currency: "EUR",
            maximumFractionDigits: 0,
          }).format(car.price)}
        </div>
      </CardContent>
      <CardFooter className="p-4 pt-0">
        <Link href={`/vehiculos/${car.id}`} className="w-full">
          <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap" className="w-full">
            <Button variant="outline" className="w-full">
              Ver Detalles
            </Button>
          </motion.div>
        </Link>
      </CardFooter>
    </Card>
  )
}
