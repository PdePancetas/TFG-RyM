FROM eclipse-temurin:21-jdk-alpine

# Directorio de trabajo para la app
WORKDIR /app

# Copiar solo el .jar construido desde la etapa anterior
COPY target/*.jar /app/drcars-api.jar

# Exponer puerto configurado
EXPOSE 9090

# Ejecutar el JAR
ENTRYPOINT ["java", "-jar", "drcars-api.jar"]
    