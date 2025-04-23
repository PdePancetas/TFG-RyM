# -----------------------
# 🛠️ Etapa de construcción
# -----------------------
    FROM maven:3.9.5-eclipse-temurin-21-alpine AS builder

    # Establecer directorio de trabajo
    WORKDIR /build
    
    # Copiar archivos del proyecto (pom.xml + código fuente)
    COPY pom.xml .
    COPY src ./src
    
    # Construir el JAR sin pruebas si lo deseas (-DskipTests)
    RUN mvn clean package -DskipTests
    
    # -----------------------
    # 🚀 Etapa de ejecución
    # -----------------------
    FROM eclipse-temurin:21-jdk-alpine
    
    # Directorio de trabajo para la app
    WORKDIR /app
    
    # Copiar solo el .jar construido desde la etapa anterior
    COPY --from=builder /build/target/*.jar app.jar
    
    # Exponer puerto configurado
    EXPOSE 9090
    
    # Ejecutar el JAR
    ENTRYPOINT ["java", "-jar", "app.jar"]
    