"use client"

import { useState, useEffect } from "react"
import { ChevronDown, Search, X, Home, RefreshCw, AlertTriangle, ExternalLink } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Slider } from "@/components/ui/slider"
import { Badge } from "@/components/ui/badge"
import { CarCard } from "@/components/car-card"
import type { Vehicle } from "@/data/vehicles"
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Separator } from "@/components/ui/separator"
import { Checkbox } from "@/components/ui/checkbox"
import { useSearchParams } from "next/navigation"
import Link from "next/link"
import { RequestCustomVehicleDialog } from "@/components/request-custom-vehicle-dialog"
import { useCatalogo } from "@/contexts/catalogo-context"
import { Alert, AlertDescription, AlertTitle } from "@/components/ui/alert"

export default function Catalogo() {
  const searchParams = useSearchParams()
  const initialCategory = searchParams ? searchParams.get("category") || "all" : "all"

  // Usar el contexto de catálogo
  const { vehicles, isLoading, error, loadCatalogo, isApiConnected, testConnection, ngrokWarningDetected, ngrokUrl } =
    useCatalogo()

  const [filters, setFilters] = useState({
    query: "",
    brand: "",
    model: "",
    year: "",
    minPrice: 0,
    maxPrice: 250000,
    fuel: "",
    transmission: "",
    category: initialCategory,
    inStock: true,
  })

  const [activeFilters, setActiveFilters] = useState<string[]>([])
  const [filteredVehicles, setFilteredVehicles] = useState<Vehicle[]>([])
  const [showFilters, setShowFilters] = useState(false)

  // Estados para almacenar los datos del catálogo
  const [brands, setBrands] = useState<string[]>([])
  const [models, setModels] = useState<string[]>([])
  const [years, setYears] = useState<number[]>([])
  const [fuels, setFuels] = useState<string[]>([])
  const [transmissions, setTransmissions] = useState<string[]>([])
  const [connectionStatus, setConnectionStatus] = useState<"checking" | "connected" | "disconnected">("checking")

  // Verificar la conexión al montar el componente
  useEffect(() => {
    const checkConnection = async () => {
      setConnectionStatus("checking")
      const isConnected = await testConnection()
      setConnectionStatus(isConnected ? "connected" : "disconnected")

      if (isConnected) {
        loadCatalogo()
      }
    }

    checkConnection()
  }, [testConnection, loadCatalogo])

  // Extraer valores únicos para los filtros cuando cambian los vehículos
  useEffect(() => {
    if (vehicles.length === 0) return

    // Extraer valores únicos para los filtros
    const uniqueBrands = Array.from(new Set(vehicles.map((v) => v.brand))).sort()
    const uniqueModels = Array.from(new Set(vehicles.map((v) => v.model))).sort()
    const uniqueYears = Array.from(new Set(vehicles.map((v) => v.year))).sort((a, b) => b - a)
    const uniqueFuels = Array.from(new Set(vehicles.map((v) => v.fuel)))
      .filter(Boolean)
      .sort()
    const uniqueTransmissions = Array.from(new Set(vehicles.map((v) => v.transmission)))
      .filter(Boolean)
      .sort()

    setBrands(uniqueBrands)
    setModels(uniqueModels)
    setYears(uniqueYears)
    setFuels(uniqueFuels)
    setTransmissions(uniqueTransmissions)
  }, [vehicles])

  // Actualizar resultados cuando cambian los filtros o los vehículos
  useEffect(() => {
    if (vehicles.length === 0) {
      setFilteredVehicles([])
      return
    }

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
      const categoryMatch = filters.category === "all" || vehicle.category === filters.category

      // Filtro de precio
      const priceMatch = vehicle.price >= filters.minPrice && vehicle.price <= filters.maxPrice

      // Filtro de stock
      const stockMatch = !filters.inStock || vehicle.inStock

      return (
        textMatch &&
        brandMatch &&
        modelMatch &&
        yearMatch &&
        priceMatch &&
        fuelMatch &&
        transmissionMatch &&
        categoryMatch &&
        stockMatch
      )
    })

    setFilteredVehicles(filtered)

    // Actualizar filtros activos para mostrar badges
    const newActiveFilters = []
    if (filters.brand) newActiveFilters.push(`Marca: ${filters.brand}`)
    if (filters.model) newActiveFilters.push(`Modelo: ${filters.model}`)
    if (filters.year) newActiveFilters.push(`Año: ${filters.year}`)
    if (filters.fuel) newActiveFilters.push(`Combustible: ${filters.fuel}`)
    if (filters.transmission) newActiveFilters.push(`Transmisión: ${filters.transmission}`)
    if (filters.category !== "all") {
      const categoryText = filters.category === "luxury" ? "Lujo" : "Estándar"
      newActiveFilters.push(`Categoría: ${categoryText}`)
    }
    if (filters.minPrice > 0 || filters.maxPrice < 250000) {
      newActiveFilters.push(
        `Precio: ${filters.minPrice.toLocaleString("es-ES")}€ - ${filters.maxPrice.toLocaleString("es-ES")}€`,
      )
    }
    if (filters.inStock) {
      newActiveFilters.push("Solo en stock")
    }

    setActiveFilters(newActiveFilters)
  }, [filters, vehicles])

  // Inicializar filtros basados en la URL - solo se ejecuta una vez
  useEffect(() => {
    if (searchParams && initialCategory !== "all" && initialCategory !== "") {
      setFilters((prev) => ({
        ...prev,
        category: initialCategory,
      }))
    }
  }, [initialCategory, searchParams])

  const handleFilterChange = (key: string, value: string | number | number[] | boolean) => {
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
      category: "all",
      inStock: true,
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
        handleFilterChange("category", "all")
        break
      case "Precio":
        handleFilterChange("minPrice", 0)
        handleFilterChange("maxPrice", 250000)
        break
      case "Solo en stock":
        handleFilterChange("inStock", false)
        break
    }
  }

  // Modificar la función handleRetryConnection para manejar mejor los errores
  const handleRetryConnection = async () => {
    try {
      setConnectionStatus("checking")
      console.log("🔄 Intentando reconectar...")
      const isConnected = await testConnection()
      console.log(`🔄 Resultado del reintento: ${isConnected ? "Conectado" : "Desconectado"}`)
      setConnectionStatus(isConnected ? "connected" : "disconnected")

      if (isConnected) {
        console.log("✅ Conexión establecida, cargando catálogo...")
        await loadCatalogo()
      } else {
        console.log("❌ No se pudo establecer conexión")
      }
    } catch (err) {
      console.error("❌ Error inesperado al reintentar la conexión:", err)
      setConnectionStatus("disconnected")
    }
  }

  // Función para abrir la URL de ngrok en una nueva pestaña
  const openNgrokUrl = () => {
    window.open(ngrokUrl, "_blank")
  }

  return (
    <div className="min-h-screen bg-gray-50 pt-24 pb-16">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex flex-col md:flex-row justify-between items-start md:items-center mb-8">
          <div>
            <h1 className="text-3xl md:text-4xl font-bold text-gray-900">Catálogo de Vehículos</h1>
            <p className="text-gray-600 mt-2">Explora nuestra selección de vehículos importados</p>
          </div>

          <div className="flex flex-wrap items-center gap-4 mt-4 md:mt-0">
            <Link href="/">
              <Button variant="outline" className="flex items-center gap-2">
                <Home className="h-4 w-4" />
                Volver al inicio
              </Button>
            </Link>

            <Button
              variant="outline"
              className="flex items-center gap-2"
              onClick={() => loadCatalogo()}
              disabled={isLoading || connectionStatus !== "connected"}
            >
              <RefreshCw className={`h-4 w-4 ${isLoading ? "animate-spin" : ""}`} />
              Actualizar
            </Button>

            <RequestCustomVehicleDialog buttonVariant="default" />

            <Button
              onClick={() => setShowFilters(!showFilters)}
              variant="outline"
              className="flex items-center gap-2 md:hidden"
            >
              <Search className="h-4 w-4" />
              Filtros
              <ChevronDown className={`h-4 w-4 transition-transform ${showFilters ? "rotate-180" : ""}`} />
            </Button>
          </div>
        </div>

        {/* Alerta de advertencia de ngrok */}
        {ngrokWarningDetected && (
          <Alert variant="destructive" className="mb-6">
            <AlertTriangle className="h-4 w-4" />
            <AlertTitle>Advertencia de seguridad de ngrok detectada</AlertTitle>
            <AlertDescription className="space-y-2">
              <p>
                Para acceder a los datos del catálogo, primero debes aceptar la advertencia de seguridad de ngrok. Sigue
                estos pasos:
              </p>
              <ol className="list-decimal pl-5 space-y-1">
                <li>Haz clic en el botón "Abrir ngrok" a continuación</li>
                <li>En la nueva pestaña, haz clic en el botón "Visit Site" para aceptar la advertencia</li>
                <li>Vuelve a esta página y haz clic en "Reintentar conexión"</li>
              </ol>
              <div className="flex gap-4 mt-4">
                <Button onClick={openNgrokUrl} className="flex items-center gap-2">
                  <ExternalLink className="h-4 w-4" />
                  Abrir ngrok
                </Button>
                <Button variant="outline" onClick={handleRetryConnection} className="flex items-center gap-2">
                  <RefreshCw className="h-4 w-4" />
                  Reintentar conexión
                </Button>
              </div>
            </AlertDescription>
          </Alert>
        )}

        {/* Alerta de estado de conexión */}
        {connectionStatus === "disconnected" && !ngrokWarningDetected && (
          <Alert variant="destructive" className="mb-6">
            <AlertTriangle className="h-4 w-4" />
            <AlertTitle>Error de conexión</AlertTitle>
            <AlertDescription>
              No se pudo conectar con el servidor. Verifica tu conexión a internet o inténtalo más tarde.
              <Button variant="outline" size="sm" className="ml-2 mt-2" onClick={handleRetryConnection}>
                Reintentar conexión
              </Button>
            </AlertDescription>
          </Alert>
        )}

        {connectionStatus === "checking" && (
          <Alert className="mb-6">
            <RefreshCw className="h-4 w-4 animate-spin" />
            <AlertTitle>Verificando conexión</AlertTitle>
            <AlertDescription>Estamos verificando la conexión con el servidor...</AlertDescription>
          </Alert>
        )}

        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          {/* Filtros (escritorio) */}
          <div className="hidden md:block">
            <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 sticky top-24">
              <div className="flex justify-between items-center mb-6">
                <h2 className="text-lg font-bold">Filtros</h2>
                <Button
                  onClick={clearFilters}
                  variant="ghost"
                  size="sm"
                  className="text-sm text-gray-500 hover:text-primary"
                >
                  Limpiar
                </Button>
              </div>

              <div className="space-y-6">
                <div>
                  <Label htmlFor="desktop-search">Buscar</Label>
                  <div className="relative mt-1">
                    <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                    <Input
                      id="desktop-search"
                      placeholder="Marca, modelo..."
                      value={filters.query}
                      onChange={(e) => handleFilterChange("query", e.target.value)}
                      className="pl-10"
                    />
                  </div>
                </div>

                <div>
                  <Label htmlFor="desktop-category">Categoría</Label>
                  <Select value={filters.category} onValueChange={(value) => handleFilterChange("category", value)}>
                    <SelectTrigger id="desktop-category" className="mt-1">
                      <SelectValue placeholder="Todas las categorías" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value="all">Todas las categorías</SelectItem>
                      <SelectItem value="luxury">Vehículos de Lujo</SelectItem>
                      <SelectItem value="standard">Vehículos Estándar</SelectItem>
                    </SelectContent>
                  </Select>
                </div>

                <div>
                  <Label htmlFor="desktop-brand">Marca</Label>
                  <Select value={filters.brand} onValueChange={(value) => handleFilterChange("brand", value)}>
                    <SelectTrigger id="desktop-brand" className="mt-1">
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
                  <Label htmlFor="desktop-model">Modelo</Label>
                  <Select value={filters.model} onValueChange={(value) => handleFilterChange("model", value)}>
                    <SelectTrigger id="desktop-model" className="mt-1">
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
                  <Label htmlFor="desktop-year">Año</Label>
                  <Select value={filters.year} onValueChange={(value) => handleFilterChange("year", value)}>
                    <SelectTrigger id="desktop-year" className="mt-1">
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
                  <Label htmlFor="desktop-fuel">Combustible</Label>
                  <Select value={filters.fuel} onValueChange={(value) => handleFilterChange("fuel", value)}>
                    <SelectTrigger id="desktop-fuel" className="mt-1">
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
                  <Label htmlFor="desktop-transmission">Transmisión</Label>
                  <Select
                    value={filters.transmission}
                    onValueChange={(value) => handleFilterChange("transmission", value)}
                  >
                    <SelectTrigger id="desktop-transmission" className="mt-1">
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

                <div className="flex items-center space-x-2">
                  <Checkbox
                    id="desktop-stock"
                    checked={filters.inStock}
                    onCheckedChange={(checked) => handleFilterChange("inStock", !!checked)}
                  />
                  <label
                    htmlFor="desktop-stock"
                    className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                  >
                    Solo vehículos en stock
                  </label>
                </div>
              </div>
            </div>
          </div>

          {/* Filtros (móvil) */}
          {showFilters && (
            <div className="md:hidden col-span-1 mb-6">
              <div className="bg-white p-6 rounded-lg shadow-sm border border-gray-100">
                <div className="flex justify-between items-center mb-6">
                  <h2 className="text-lg font-bold">Filtros</h2>
                  <Button
                    onClick={clearFilters}
                    variant="ghost"
                    size="sm"
                    className="text-sm text-gray-500 hover:text-primary"
                  >
                    Limpiar
                  </Button>
                </div>

                <div className="space-y-6">
                  <div>
                    <Label htmlFor="mobile-search">Buscar</Label>
                    <div className="relative mt-1">
                      <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
                      <Input
                        id="mobile-search"
                        placeholder="Marca, modelo..."
                        value={filters.query}
                        onChange={(e) => handleFilterChange("query", e.target.value)}
                        className="pl-10"
                      />
                    </div>
                  </div>

                  <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
                    <div>
                      <Label htmlFor="mobile-category">Categoría</Label>
                      <Select value={filters.category} onValueChange={(value) => handleFilterChange("category", value)}>
                        <SelectTrigger id="mobile-category" className="mt-1">
                          <SelectValue placeholder="Todas las categorías" />
                        </SelectTrigger>
                        <SelectContent>
                          <SelectItem value="all">Todas las categorías</SelectItem>
                          <SelectItem value="luxury">Vehículos de Lujo</SelectItem>
                          <SelectItem value="standard">Vehículos Estándar</SelectItem>
                        </SelectContent>
                      </Select>
                    </div>

                    <div>
                      <Label htmlFor="mobile-brand">Marca</Label>
                      <Select value={filters.brand} onValueChange={(value) => handleFilterChange("brand", value)}>
                        <SelectTrigger id="mobile-brand" className="mt-1">
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
                      <Label htmlFor="mobile-model">Modelo</Label>
                      <Select value={filters.model} onValueChange={(value) => handleFilterChange("model", value)}>
                        <SelectTrigger id="mobile-model" className="mt-1">
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
                      <Label htmlFor="mobile-year">Año</Label>
                      <Select value={filters.year} onValueChange={(value) => handleFilterChange("year", value)}>
                        <SelectTrigger id="mobile-year" className="mt-1">
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
                  </div>

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

                  <div className="flex items-center space-x-2">
                    <Checkbox
                      id="mobile-stock"
                      checked={filters.inStock}
                      onCheckedChange={(checked) => handleFilterChange("inStock", !!checked)}
                    />
                    <label
                      htmlFor="mobile-stock"
                      className="text-sm font-medium leading-none peer-disabled:cursor-not-allowed peer-disabled:opacity-70"
                    >
                      Solo vehículos en stock
                    </label>
                  </div>
                </div>
              </div>
            </div>
          )}

          {/* Resultados */}
          <div className="md:col-span-3">
            {/* Filtros activos */}
            {activeFilters.length > 0 && (
              <div className="mb-6">
                <div className="flex flex-wrap gap-2">
                  {activeFilters.map((filter, index) => (
                    <Badge key={index} variant="secondary" className="flex items-center gap-1 px-3 py-1.5">
                      {filter}
                      <X className="h-3 w-3 cursor-pointer ml-1" onClick={() => removeFilter(filter)} />
                    </Badge>
                  ))}
                  {activeFilters.length > 1 && (
                    <Button variant="ghost" size="sm" onClick={clearFilters} className="text-sm">
                      Limpiar todos
                    </Button>
                  )}
                </div>
              </div>
            )}

            {/* Tabs de categoría */}
            <div className="mb-6">
              <Tabs
                value={filters.category}
                onValueChange={(value) => handleFilterChange("category", value)}
                className="w-full"
              >
                <TabsList className="grid w-full grid-cols-3">
                  <TabsTrigger value="all">Todos</TabsTrigger>
                  <TabsTrigger value="luxury">Lujo</TabsTrigger>
                  <TabsTrigger value="standard">Estándar</TabsTrigger>
                </TabsList>
              </Tabs>
            </div>

            {/* Contador de resultados */}
            <div className="mb-6">
              <div className="flex justify-between items-center">
                <h2 className="text-lg font-medium">
                  {isLoading ? "Cargando vehículos..." : `${filteredVehicles.length} vehículos encontrados`}
                </h2>
                <Select defaultValue="newest">
                  <SelectTrigger className="w-[180px]">
                    <SelectValue placeholder="Ordenar por" />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="newest">Más recientes</SelectItem>
                    <SelectItem value="price-asc">Precio: menor a mayor</SelectItem>
                    <SelectItem value="price-desc">Precio: mayor a menor</SelectItem>
                    <SelectItem value="name-asc">Nombre: A-Z</SelectItem>
                    <SelectItem value="name-desc">Nombre: Z-A</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <Separator className="my-4" />
            </div>

            {/* Estado de carga y errores */}
            {isLoading && (
              <div className="text-center py-12">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary mx-auto mb-4"></div>
                <h3 className="text-xl font-medium text-gray-900 mb-2">Cargando vehículos</h3>
                <p className="text-gray-600">Por favor, espera mientras obtenemos los datos del catálogo...</p>
              </div>
            )}

            {error && !isLoading && !ngrokWarningDetected && (
              <div className="text-center py-12">
                <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded mb-4">
                  <p>{error}</p>
                </div>
                <Button onClick={() => loadCatalogo()}>Reintentar</Button>
              </div>
            )}

            {/* Lista de vehículos */}
            {!isLoading && !error && !ngrokWarningDetected && (
              <>
                {filteredVehicles.length > 0 ? (
                  <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
                    {filteredVehicles.map((vehicle) => (
                      <div key={vehicle.id}>
                        <CarCard car={vehicle} />
                      </div>
                    ))}
                  </div>
                ) : (
                  <div className="text-center py-12">
                    <h3 className="text-xl font-medium text-gray-900 mb-2">No se encontraron vehículos</h3>
                    <p className="text-gray-600 mb-6">Prueba a cambiar los filtros de búsqueda</p>
                    <Button onClick={clearFilters}>Limpiar filtros</Button>
                  </div>
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  )
}
