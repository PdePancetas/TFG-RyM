"use client"

import { useState, useEffect } from "react"
import { motion } from "framer-motion"
import Link from "next/link"
import { ChevronRight, ArrowRight, Menu } from "lucide-react"
import { Button } from "@/components/ui/button"
import { cn } from "@/lib/utils"
import { FeaturedCar } from "@/components/featured-car"
import { MobileNav } from "@/components/mobile-nav"
import { vehicles } from "@/data/vehicles"
import { AnimatedLink } from "@/components/animated-link"
import { RotatingCar } from "@/components/rotating-car"
import { RequestAppointmentDialog } from "@/components/request-appointment-dialog"
import { SearchDialog } from "@/components/search-dialog"
import { FloatingActionButtons } from "@/components/floating-action-buttons"
import { UserDropdown } from "@/components/user-dropdown"
import Image from "next/image"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { RequestSearchDialog } from "@/components/request-search-dialog"

export function MobileView() {
  const [isScrolled, setIsScrolled] = useState(false)
  const [mobileNavOpen, setMobileNavOpen] = useState(false)

  // Filtrar vehículos por categoría
  const luxuryVehicles = vehicles.filter((vehicle) => vehicle.category === "luxury")

  // Seleccionar vehículos destacados (3 de lujo)
  const featuredLuxuryVehicles = luxuryVehicles.slice(0, 3)

  useEffect(() => {
    const handleScroll = () => {
      setIsScrolled(window.scrollY > 10)
    }
    window.addEventListener("scroll", handleScroll)
    return () => window.removeEventListener("scroll", handleScroll)
  }, [])

  // Animaciones para botones
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
    <div className="min-h-screen bg-white">
      {/* Header */}
      <header
        className={cn(
          "fixed top-0 left-0 right-0 z-50 transition-all duration-300 py-4 px-4",
          isScrolled ? "bg-white shadow-md" : "bg-transparent",
        )}
      >
        <div className="flex items-center justify-between">
          <Link href="/" className="flex items-center space-x-2">
            <motion.div
              initial={{ opacity: 0, x: -20 }}
              animate={{ opacity: 1, x: 0 }}
              transition={{ duration: 0.5 }}
              whileHover={{ scale: 1.05 }}
            >
              <h1 className="text-2xl font-bold text-primary">DRCars</h1>
            </motion.div>
          </Link>

          <div className="flex items-center space-x-2">
            <motion.div whileHover={{ scale: 1.1 }} whileTap={{ scale: 0.9 }}>
              <Button variant="ghost" size="icon" onClick={() => setMobileNavOpen(true)}>
                <Menu className="h-6 w-6" />
              </Button>
            </motion.div>
            <UserDropdown />
          </div>
        </div>
      </header>

      {/* Mobile Navigation */}
      {mobileNavOpen && <MobileNav onClose={() => setMobileNavOpen(false)} />}

      {/* Hero Section with Rotating Ferrari */}
      <section className="relative h-screen">
        <div className="absolute inset-0 bg-gradient-to-r from-black/50 to-black/30 z-10" />
        <div className="absolute inset-0">
          <div className="relative w-full h-full">
            <RotatingCar />
          </div>
        </div>
        <div className="relative z-20 h-full flex flex-col justify-center items-center text-center px-6">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.3 }}
          >
            <h1 className="text-4xl font-bold text-white mb-4">Vehículos de Importación</h1>
          </motion.div>
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.5 }}
          >
            <p className="text-xl text-white/90 mb-8">
              Descubre nuestra exclusiva selección de automóviles premium importados.
            </p>
          </motion.div>
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.8, delay: 0.7 }}
            className="flex flex-col gap-4"
          >
            <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
              <SearchDialog buttonVariant="default" buttonSize="lg" buttonText="Buscar Vehículos" />
            </motion.div>
            <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
              <RequestAppointmentDialog
                buttonVariant="outline"
                buttonSize="lg"
                className="bg-white/10 text-white border-white/20 hover:bg-white/20"
              />
            </motion.div>
          </motion.div>
        </div>
        <div className="absolute bottom-10 left-0 right-0 z-20 flex justify-center">
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 1, delay: 1.2 }}
            className="animate-bounce"
          >
            <motion.div whileHover={{ scale: 1.2 }} whileTap={{ scale: 0.9 }}>
              <Button variant="ghost" size="icon" className="rounded-full bg-white/10 text-white hover:bg-white/20">
                <ChevronRight className="h-5 w-5 rotate-90" />
              </Button>
            </motion.div>
          </motion.div>
        </div>
      </section>

      {/* Vehículos de Lujo */}
      <section className="py-16 px-4 bg-gray-50">
        <div className="max-w-7xl mx-auto">
          <div className="flex justify-between items-center mb-8">
            <motion.div
              initial={{ opacity: 0, x: -20 }}
              whileInView={{ opacity: 1, x: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5 }}
            >
              <h2 className="text-3xl font-bold text-gray-900">Vehículos de Lujo</h2>
              <p className="text-gray-600 mt-2">Nuestra selección premium</p>
            </motion.div>
            <motion.div
              initial={{ opacity: 0, x: 20 }}
              whileInView={{ opacity: 1, x: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5 }}
              whileHover={{ x: 5 }}
            >
              <AnimatedLink href="/catalogo?category=luxury" className="text-primary flex items-center">
                Ver todos
                <ArrowRight className="ml-2 h-4 w-4" />
              </AnimatedLink>
            </motion.div>
          </div>

          <div className="grid grid-cols-1 gap-6">
            {featuredLuxuryVehicles.slice(0, 2).map((car, index) => (
              <motion.div
                key={car.id}
                initial={{ opacity: 0, y: 20 }}
                whileInView={{ opacity: 1, y: 0 }}
                viewport={{ once: true }}
                transition={{ duration: 0.5, delay: index * 0.1 }}
                whileHover={{ y: -5 }}
              >
                <FeaturedCar car={car} />
              </motion.div>
            ))}
          </div>
        </div>
      </section>

      {/* Why Choose Us */}
      <section className="py-16 px-4 relative">
        <div className="absolute inset-0 z-0 opacity-10">
          <Image
            src="https://images.unsplash.com/photo-1492144534655-ae79c964c9d7?q=80&w=1983&auto=format&fit=crop"
            alt="Background"
            fill
            className="object-cover"
          />
        </div>
        <div className="max-w-7xl mx-auto relative z-10">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.5 }}
            className="text-center mb-10"
          >
            <h2 className="text-3xl font-bold text-gray-900 mb-4">¿Por qué elegirnos?</h2>
            <p className="text-gray-600">
              En DRCars nos especializamos en la importación de vehículos de lujo, ofreciendo un servicio personalizado.
            </p>
          </motion.div>

          <div className="grid grid-cols-1 gap-6">
            <motion.div
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5, delay: 0.1 }}
              whileHover={{ y: -5, boxShadow: "0 10px 25px -5px rgba(0, 0, 0, 0.1)" }}
              className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 bg-primary/10 rounded-full flex items-center justify-center mb-4">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  className="h-6 w-6 text-primary"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"
                  />
                </svg>
              </div>
              <h3 className="text-xl font-bold mb-2">Garantía de Calidad</h3>
              <p className="text-gray-600">
                Todos nuestros vehículos pasan por rigurosos controles de calidad y ofrecemos garantía en todos nuestros
                servicios.
              </p>
            </motion.div>

            <motion.div
              initial={{ opacity: 0, y: 20 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5, delay: 0.2 }}
              whileHover={{ y: -5, boxShadow: "0 10px 25px -5px rgba(0, 0, 0, 0.1)" }}
              className="bg-white p-6 rounded-lg shadow-sm border border-gray-100 hover:shadow-md transition-all"
            >
              <div className="w-12 h-12 bg-primary/10 rounded-full flex items-center justify-center mb-4">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  className="h-6 w-6 text-primary"
                  fill="none"
                  viewBox="0 0 24 24"
                  stroke="currentColor"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                  />
                </svg>
              </div>
              <h3 className="text-xl font-bold mb-2">Precios Competitivos</h3>
              <p className="text-gray-600">
                Trabajamos directamente con proveedores internacionales para ofrecerte los mejores precios del mercado.
              </p>
            </motion.div>
          </div>
        </div>
      </section>

      {/* Sección de Contacto */}
      <section className="py-16 px-4 bg-primary text-white relative">
        <div className="absolute inset-0 z-0 opacity-20">
          <Image
            src="https://images.unsplash.com/photo-1552519507-da3b142c6e3d?q=80&w=2070&auto=format&fit=crop"
            alt="Background"
            fill
            className="object-cover"
          />
        </div>
        <div className="max-w-7xl mx-auto relative z-10">
          <motion.div
            initial={{ opacity: 0, y: 20 }}
            whileInView={{ opacity: 1, y: 0 }}
            viewport={{ once: true }}
            transition={{ duration: 0.5 }}
            className="text-center mb-8"
          >
            <h2 className="text-3xl font-bold mb-4">¿Buscas un vehículo específico?</h2>
            <p className="text-white/90 mb-6">
              Si no encuentras el vehículo que buscas en nuestro catálogo, podemos localizarlo para ti.
            </p>
            <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
              <RequestSearchDialog
                buttonVariant="default"
                buttonSize="lg"
                className="bg-white text-primary hover:bg-white/90"
              />
            </motion.div>
          </motion.div>

          <div className="bg-white/10 p-6 rounded-lg backdrop-blur-sm mt-8">
            <h3 className="text-xl font-bold mb-4">Formulario de Contacto</h3>
            <form className="space-y-4">
              <div>
                <Label htmlFor="name-mobile" className="block text-sm font-medium mb-1">
                  Nombre
                </Label>
                <Input
                  id="name-mobile"
                  className="bg-white/10 border-white/20 text-white placeholder:text-white/60 focus:border-white"
                  placeholder="Tu nombre"
                />
              </div>
              <div>
                <Label htmlFor="email-mobile" className="block text-sm font-medium mb-1">
                  Email
                </Label>
                <Input
                  id="email-mobile"
                  type="email"
                  className="bg-white/10 border-white/20 text-white placeholder:text-white/60 focus:border-white"
                  placeholder="Tu email"
                />
              </div>
              <div>
                <Label htmlFor="message-mobile" className="block text-sm font-medium mb-1">
                  Mensaje
                </Label>
                <textarea
                  id="message-mobile"
                  rows={4}
                  className="w-full rounded-md bg-white/10 border-white/20 text-white placeholder:text-white/60 focus:border-white p-2"
                  placeholder="Cuéntanos qué vehículo estás buscando"
                ></textarea>
              </div>
              <motion.div variants={buttonVariants} initial="initial" whileHover="hover" whileTap="tap">
                <Button className="w-full bg-white text-primary hover:bg-white/90">Enviar Solicitud</Button>
              </motion.div>
            </form>
          </div>
        </div>
      </section>

      {/* Floating Action Buttons for Mobile */}
      <FloatingActionButtons />

      {/* Footer */}
      <footer className="bg-gray-900 text-white py-12 px-4">
        <div className="max-w-7xl mx-auto">
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-8">
            <div>
              <h3 className="text-xl font-bold mb-4">DRCars</h3>
              <p className="text-gray-400">
                Especialistas en importación de vehículos de lujo con más de 10 años de experiencia en el sector.
              </p>
              <div className="flex space-x-4 mt-6">
                <motion.a
                  href="#"
                  className="text-gray-400 hover:text-white transition-colors"
                  whileHover={{ scale: 1.2, color: "#ffffff" }}
                >
                  <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M22 12c0-5.523-4.477-10-10-10S2 6.477 2 12c0 4.991 3.657 9.128 8.438 9.878v-6.987h-2.54V12h2.54V9.797c0-2.506 1.492-3.89 3.777-3.89 1.094 0 2.238.195 2.238.195v2.46h-1.26c-1.243 0-1.63.771-1.63 1.562V12h2.773l-.443 2.89h-2.33v6.988C18.343 21.128 22 16.991 22 12z" />
                  </svg>
                </motion.a>
                <motion.a
                  href="#"
                  className="text-gray-400 hover:text-white transition-colors"
                  whileHover={{ scale: 1.2, color: "#ffffff" }}
                >
                  <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M12.315 2c2.43 0 2.784.013 3.808.06 1.064.049 1.791.218 2.427.465a4.902 4.902 0 011.772 1.153 4.902 4.902 0 011.153 1.772c.247.636.416 1.363.465 2.427.048 1.067.06 1.407.06 4.123v.08c0 2.643-.012 2.987-.06 4.043-.049 1.064-.218 1.791-.465 2.427a4.902 4.902 0 01-1.153 1.772 4.902 4.902 0 01-1.772 1.153c-.636.247-1.363.416-2.427.465-1.067.048-1.407.06-4.123.06h-.08c-2.643 0-2.987-.012-4.043-.06-1.064-.049-1.791-.218-2.427-.465a4.902 4.902 0 01-1.772-1.153 4.902 4.902 0 01-1.153-1.772c-.247-.636-.416-1.363-.465-2.427-.047-1.024-.06-1.379-.06-3.808v-.63c0-2.43.013-2.784.06-3.808.049-1.064.218-1.791.465-2.427a4.902 4.902 0 011.153-1.772A4.902 4.902 0 015.45 2.525c.636-.247 1.363-.416 2.427-.465C8.901 2.013 9.256 2 11.685 2h.63zm-.081 1.802h-.468c-2.456 0-2.784.011-3.807.058-.975.045-1.504.207-1.857.344-.467.182-.8.398-1.15.748-.35.35-.566.683-.748 1.15-.137.353-.3.882-.344 1.857-.047 1.023-.058 1.351-.058 3.807v.468c0 2.456.011 2.784.058 3.807.045.975.207 1.504.344 1.857.182.466.399.8.748 1.15.35.35.683.566 1.15.748.353.137.882.3 1.857.344 1.054.048 1.37.058 4.041.058h.08c2.597 0 2.917-.01 3.96-.058.976-.045 1.505-.207 1.858-.344.466-.182.8-.398 1.15-.748.35-.35.566-.683.748-1.15.137-.353.3-.882.344-1.857.048-1.055.058-1.37.058-4.041v-.08c0-2.597-.01-2.917-.058-3.96-.045-.976-.207-1.505-.344-1.858a3.097 3.097 0 00-.748-1.15 3.098 3.098 0 00-1.15-.748c-.353-.137-.882-.3-1.857-.344-1.023-.047-1.351-.058-3.807-.058zM12 6.865a5.135 5.135 0 110 10.27 5.135 5.135 0 010-10.27zm0 1.802a3.333 3.333 0 100 6.666 3.333 3.333 0 000-6.666zm5.338-3.205a1.2 1.2 0 110 2.4 1.2 1.2 0 010-2.4z" />
                  </svg>
                </motion.a>
                <motion.a
                  href="#"
                  className="text-gray-400 hover:text-white transition-colors"
                  whileHover={{ scale: 1.2, color: "#ffffff" }}
                >
                  <svg className="h-6 w-6" fill="currentColor" viewBox="0 0 24 24">
                    <path d="M8.29 20.251c7.547 0 11.675-6.253 11.675-11.675 0-.178 0-.355-.012-.53A8.348 8.348 0 0022 5.92a8.19 8.19 0 01-2.357.646 4.118 4.118 0 001.804-2.27 8.224 8.224 0 01-2.605.996 4.107 4.107 0 00-6.993 3.743 11.65 11.65 0 01-8.457-4.287 4.106 4.106 0 001.27 5.477A4.072 4.072 0 012.8 9.713v.052a4.105 4.105 0 003.292 4.022 4.095 4.095 0 01-1.853.07 4.108 4.108 0 003.834 2.85A8.233 8.233 0 012 18.407a11.616 11.616 0 006.29 1.84" />
                  </svg>
                </motion.a>
              </div>
            </div>

            <div>
              <h3 className="text-lg font-semibold mb-4">Contacto</h3>
              <ul className="space-y-2">
                <li className="flex items-start">
                  <svg
                    className="h-5 w-5 text-gray-400 mr-2 mt-0.5"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"
                    />
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"
                    />
                  </svg>
                  <span className="text-gray-400">Calle Principal 123, Madrid, España</span>
                </li>
                <li className="flex items-start">
                  <svg
                    className="h-5 w-5 text-gray-400 mr-2 mt-0.5"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M3 5a2 2 0 012-2h3.28a1 1 0 01.948.684l1.498 4.493a1 1 0 01-.502 1.21l-2.257 1.13a11.042 11.042 0 005.516 5.516l1.13-2.257a1 1 0 011.21-.502l4.493 1.498a1 1 0 01.684.949V19a2 2 0 01-2 2h-1C9.716 21 3 14.284 3 6V5z"
                    />
                  </svg>
                  <span className="text-gray-400">+34 912 345 678</span>
                </li>
                <li className="flex items-start">
                  <svg
                    className="h-5 w-5 text-gray-400 mr-2 mt-0.5"
                    fill="none"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      strokeWidth={2}
                      d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z"
                    />
                  </svg>
                  <span className="text-gray-400">info@drcars.com</span>
                </li>
              </ul>
            </div>
          </div>

          <div className="border-t border-gray-800 mt-8 pt-6">
            <p className="text-gray-400 text-sm text-center">© 2023 DRCars. Todos los derechos reservados.</p>
          </div>
        </div>
      </footer>
    </div>
  )
}
