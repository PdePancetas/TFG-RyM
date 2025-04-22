package com.DRCars.config;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

@Configuration
public class CorsConfig {
    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                // Permitir todas las peticiones desde cualquier origen (ideal para desarrollo)
                registry.addMapping("/**")
                    .allowedOrigins("https://v0-pagina-de-venta-drc-ars.vercel.app/") // o especifica la URL de tu frontend: "https://mi-frontend.com"
                    .allowedMethods("*")
                    .allowedHeaders("*");
            }
        };
    }
}