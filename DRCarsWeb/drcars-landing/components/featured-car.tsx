"use client"

import Image from "next/image"
import Link from "next/link"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardFooter } from "@/components/ui/card"
import { Badge } from "@/components/ui/badge"
import { Check } from "lucide-react"
import { motion } from "framer-motion"
import type { Vehicle } from "@/data/vehicles"
import { getCarMainImage } from "@/data/car-images"

interface FeaturedCarProps {
  car: Vehicle
}

export function FeaturedCar({ car }: FeaturedCarProps) {
  // Obtener la imagen real del coche
  const carImage = getCarMainImage(car.name)

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
    <Card className="overflow-hidden h-full flex flex-col">
      <div className="relative h-64 overflow-hidden">
        <Badge className="absolute top-4 left-4 z-10">Destacado</Badge>
        {car.category === "luxury" && <Badge className="absolute top-4 right-4 z-10 bg-primary">Lujo</Badge>}
        {car.category === "standard" && (
          <Badge className="absolute top-4 right-4 z-10 bg-secondary text-secondary-foreground">Estándar</Badge>
        )}
        {!car.inStock && (
          <Badge variant="destructive" className="absolute top-12 left-4 z-10">
            Agotado
          </Badge>
        )}
        <Image
          src={carImage || "/placeholder.svg"}
          alt={car.name}
          fill
          className="object-cover transition-transform duration-500 hover:scale-105"
        />
      </div>
      <CardContent className="p-6 flex-grow">
        <div className="flex justify-between items-start mb-4">
          <h3 className="font-bold text-xl">{car.name}</h3>
          <span className="text-sm text-gray-500">{car.year}</span>
        </div>
        <ul className="space-y-2 mb-4">
          {car.features.slice(0, 4).map((feature, index) => (
            <li key={index} className="flex items-center text-sm">
              <Check className="h-4 w-4 text-primary mr-2" />
              {feature}
            </li>
          ))}
        </ul>
        <div className="font-bold text-2xl text-primary mt-auto">
          {new Intl.NumberFormat("es-ES", {
            style: "currency",
            currency: "EUR",
            maximumFractionDigits: 0,
          }).format(car.price)}
        </div>
      </CardContent>
      <CardFooter className="p-6 pt-0">
        <div className="flex flex-col sm:flex-row gap-3 w-full">
          <Link href={`/vehiculos/${car.id}`} className="flex-1">
            <motion.div
              variants={buttonVariants}
              initial="initial"
              whileHover="hover"
              whileTap="tap"
              className="w-full"
            >
              <Button variant="default" className="w-full">
                Ver Detalles
              </Button>
            </motion.div>
          </Link>
          <Link href={`/contacto?vehiculo=${car.id}`} className="flex-1">
            <motion.div
              variants={buttonVariants}
              initial="initial"
              whileHover="hover"
              whileTap="tap"
              className="w-full"
            >
              <Button variant="outline" className="w-full">
                Solicitar Info
              </Button>
            </motion.div>
          </Link>
        </div>
      </CardFooter>
    </Card>
  )
}
