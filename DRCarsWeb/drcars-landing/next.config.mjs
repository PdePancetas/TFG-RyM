/** @type {import('next').NextConfig} */
const nextConfig = {
  reactStrictMode: true,
  images: {
    domains: ['images.unsplash.com', 'v0.blob.com'],
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'images.unsplash.com',
      },
      {
        protocol: 'https',
        hostname: 'v0.blob.com',
      },
    ],
    unoptimized: true,
  },
  // Configuración para manejar problemas con paquetes específicos
  transpilePackages: ['camelcase-css'],
  // Optimización para reducir dependencias problemáticas
  experimental: {
    // Remove or disable optimizeCss
    // optimizeCss: true,
    optimizePackageImports: ['tailwindcss']
  },
  // Configuraciones para ignorar errores durante el despliegue
  eslint: {
    ignoreDuringBuilds: true,
  },
  typescript: {
    ignoreBuildErrors: true,
  },
};

export default nextConfig;
