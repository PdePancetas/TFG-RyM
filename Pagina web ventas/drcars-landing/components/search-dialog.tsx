"use client"

import type React from "react"

import { useState, useEffect } from "react"
import { Dialog, DialogContent, DialogHeader, DialogTitle, DialogTrigger } from "@/components/ui/dialog"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Search, X } from "lucide-react"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Slider } from "@/components/ui/slider"
import { vehicles, brands, models, years, fuels, transmissions, type Vehicle } from "@/data/vehicles"
import { CarCard } from "@/components/car-card"
import { Badge } from "@/components/ui/badge"
import { motion } from "framer-motion"

interface SearchDialogProps {
  buttonVariant?: "default" | "outline" | "ghost" | "secondary"
  buttonSize?: "default" | "sm" | "lg" | "icon"
  buttonText?: string
  showIcon?: boolean
  children?: React.ReactNode
}

export function SearchDialog({
  buttonVariant = "outline",
  buttonSize = "sm",
  buttonText = "Buscar",
  showIcon = true,
  children,
}: SearchDialogProps) {
  const [open, setOpen] = useState(false)
  const [searchResults, setSearchResults] = useState<Vehicle[]>([])
  const [filters, setFilters] = useState({
    query: "",
    brand: "",
    model: "",
    year: "",
    minPrice: 0,
    maxPrice: 250000,
    fuel: "",
    transmission: "",
    category: "",
  })
  const [activeFilters, setActiveFilters] = useState<string[]>([])
  const [filteredVehicles, setFilteredVehicles] = useState<Vehicle[]>(vehicles)

  // Actualizar resultados cuando cambian los filtros
  useEffect(() => {
    const filtered = vehicles.filter((vehicle) => {
      // Filtro de texto (busca en nombre, marca y modelo)
      const textMatch =
        filters.query === "" ||
        vehicle.name.toLowerCase().includes(filters.query.toLowerCase()) ||
        vehicle.brand.toLowerCase().includes(filters.query.toLowerCase()) ||
        vehicle.model.toLowerCase().includes(filters.query.toLowerCase())

      // Filtros de selección
      const brandMatch = filters.brand === "" || vehicle.brand === filters.brand
      const modelMatch = filters.model === "" || vehicle.model === filters.model
      const yearMatch = filters.year === "" || vehicle.year.toString() === filters.year
      const fuelMatch = filters.fuel === "" || vehicle.fuel === filters.fuel
      const transmissionMatch = filters.transmission === "" || vehicle.transmission === filters.transmission
      const categoryMatch = filters.category === "" || vehicle.category === filters.category

      // Filtro de precio
      const priceMatch = vehicle.price >= filters.minPrice && vehicle.price <= filters.maxPrice

      return (
        textMatch &&
        brandMatch &&
        modelMatch &&
        yearMatch &&
        priceMatch &&
        fuelMatch &&
        transmissionMatch &&
        categoryMatch
      )
    })

    setSearchResults(filtered)
    setFilteredVehicles(filtered)

    // Actualizar filtros activos para mostrar badges
    const newActiveFilters = []
    if (filters.brand) newActiveFilters.push(`Marca: ${filters.brand}`)
    if (filters.model) newActiveFilters.push(`Modelo: ${filters.model}`)
    if (filters.year) newActiveFilters.push(`Año: ${filters.year}`)
    if (filters.fuel) newActiveFilters.push(`Combustible: ${filters.fuel}`)
    if (filters.transmission) newActiveFilters.push(`Transmisión: ${filters.transmission}`)
    if (filters.category) {
      const categoryText = filters.category === "luxury" ? "Lujo" : "Estándar"
      newActiveFilters.push(`Categoría: ${categoryText}`)
    }
    if (filters.minPrice > 0 || filters.maxPrice < 250000) {
      newActiveFilters.push(
        `Precio: ${filters.minPrice.toLocaleString("es-ES")}€ - ${filters.maxPrice.toLocaleString("es-ES")}€`,
      )
    }

    setActiveFilters(newActiveFilters)
  }, [filters])

  const handleFilterChange = (key: string, value: string | number | number[]) => {
    setFilters((prev) => ({
      ...prev,
      [key]: value,
    }))
  }

  const clearFilters = () => {
    setFilters({
      query: "",
      brand: "",
      model: "",
      year: "",
      minPrice: 0,
      maxPrice: 250000,
      fuel: "",
      transmission: "",
      category: "",
    })
  }

  const removeFilter = (filter: string) => {
    const filterType = filter.split(":")[0].trim()

    switch (filterType) {
      case "Marca":
        handleFilterChange("brand", "")
        break
      case "Modelo":
        handleFilterChange("model", "")
        break
      case "Año":
        handleFilterChange("year", "")
        break
      case "Combustible":
        handleFilterChange("fuel", "")
        break
      case "Transmisión":
        handleFilterChange("transmission", "")
        break
      case "Categoría":
        handleFilterChange("category", "")
        break
      case "Precio":
        handleFilterChange("minPrice", 0)
        handleFilterChange("maxPrice", 250000)
        break
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

  return (
    <Dialog open={open} onOpenChange={setOpen}>
      <DialogTrigger asChild>
        <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
          <Button variant={buttonVariant} size={buttonSize}>
            {children || (
              <>
                {showIcon && <Search className="h-4 w-4 mr-2" />}
                {buttonText}
              </>
            )}
          </Button>
        </motion.div>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[900px] max-h-[85vh] overflow-y-auto z-[9000]">
        <DialogHeader className="flex flex-row items-center justify-between">
          <DialogTitle className="text-2xl">Buscar Vehículos</DialogTitle>
        </DialogHeader>

        {/* Barra de búsqueda */}
        <div className="flex gap-2 mt-4">
          <div className="relative flex-1">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
            <Input
              placeholder="Buscar por marca, modelo o nombre..."
              value={filters.query}
              onChange={(e) => handleFilterChange("query", e.target.value)}
              className="pl-10 pr-10"
            />
            {filters.query && (
              <button
                onClick={() => handleFilterChange("query", "")}
                className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400 hover:text-gray-600"
              >
                <X className="h-4 w-4" />
              </button>
            )}
          </div>
          <Button onClick={clearFilters} variant="outline" size="icon">
            <X className="h-4 w-4" />
          </Button>
        </div>

        {/* Filtros activos */}
        {activeFilters.length > 0 && (
          <div className="flex flex-wrap gap-2 mt-4">
            {activeFilters.map((filter, index) => (
              <Badge key={index} variant="secondary" className="flex items-center gap-1">
                {filter}
                <X className="h-3 w-3 cursor-pointer" onClick={() => removeFilter(filter)} />
              </Badge>
            ))}
          </div>
        )}

        <div className="grid gap-6 py-4">
          {/* Filtros */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div>
              <Label htmlFor="brand">Marca</Label>
              <Select value={filters.brand} onValueChange={(value) => handleFilterChange("brand", value)}>
                <SelectTrigger id="brand">
                  <SelectValue placeholder="Todas las marcas" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todas las marcas</SelectItem>
                  {brands.map((brand) => (
                    <SelectItem key={brand} value={brand}>
                      {brand}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div>
              <Label htmlFor="model">Modelo</Label>
              <Select value={filters.model} onValueChange={(value) => handleFilterChange("model", value)}>
                <SelectTrigger id="model">
                  <SelectValue placeholder="Todos los modelos" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todos los modelos</SelectItem>
                  {models.map((model) => (
                    <SelectItem key={model} value={model}>
                      {model}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div>
              <Label htmlFor="year">Año</Label>
              <Select value={filters.year} onValueChange={(value) => handleFilterChange("year", value)}>
                <SelectTrigger id="year">
                  <SelectValue placeholder="Todos los años" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todos los años</SelectItem>
                  {years.map((year) => (
                    <SelectItem key={year} value={year.toString()}>
                      {year}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div>
              <Label htmlFor="fuel">Combustible</Label>
              <Select value={filters.fuel} onValueChange={(value) => handleFilterChange("fuel", value)}>
                <SelectTrigger id="fuel">
                  <SelectValue placeholder="Todos los combustibles" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todos los combustibles</SelectItem>
                  {fuels.map((fuel) => (
                    <SelectItem key={fuel} value={fuel}>
                      {fuel}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div>
              <Label htmlFor="transmission">Transmisión</Label>
              <Select value={filters.transmission} onValueChange={(value) => handleFilterChange("transmission", value)}>
                <SelectTrigger id="transmission">
                  <SelectValue placeholder="Todas las transmisiones" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todas las transmisiones</SelectItem>
                  {transmissions.map((transmission) => (
                    <SelectItem key={transmission} value={transmission}>
                      {transmission}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div>
              <Label htmlFor="category">Categoría</Label>
              <Select value={filters.category} onValueChange={(value) => handleFilterChange("category", value)}>
                <SelectTrigger id="category">
                  <SelectValue placeholder="Todas las categorías" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">Todas las categorías</SelectItem>
                  <SelectItem value="luxury">Vehículos de Lujo</SelectItem>
                  <SelectItem value="standard">Vehículos Estándar</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          {/* Slider de precio */}
          <div>
            <div className="flex justify-between mb-2">
              <Label>Rango de precio</Label>
              <span className="text-sm text-muted-foreground">
                {filters.minPrice.toLocaleString("es-ES")}€ - {filters.maxPrice.toLocaleString("es-ES")}€
              </span>
            </div>
            <Slider
              defaultValue={[filters.minPrice, filters.maxPrice]}
              min={0}
              max={250000}
              step={5000}
              value={[filters.minPrice, filters.maxPrice]}
              onValueChange={(value) => {
                handleFilterChange("minPrice", value[0])
                handleFilterChange("maxPrice", value[1])
              }}
              className="my-4"
            />
          </div>

          {/* Resultados */}
          <div>
            <h3 className="text-lg font-medium mb-4">{filteredVehicles.length} vehículos encontrados</h3>

            {filteredVehicles.length > 0 ? (
              <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                {filteredVehicles.map((vehicle) => (
                  <motion.div
                    key={vehicle.id}
                    initial={{ opacity: 0, y: 20 }}
                    animate={{ opacity: 1, y: 0 }}
                    transition={{ duration: 0.3 }}
                    whileHover={{ y: -5 }}
                  >
                    <CarCard key={vehicle.id} car={vehicle} />
                  </motion.div>
                ))}
              </div>
            ) : (
              <div className="text-center py-8">
                <p className="text-muted-foreground">No se encontraron vehículos con los filtros seleccionados.</p>
                <Button onClick={clearFilters} variant="outline" className="mt-4">
                  Limpiar filtros
                </Button>
              </div>
            )}
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}
