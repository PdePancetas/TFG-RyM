FROM eclipse-temurin:21-jdk-alpine

WORKDIR /app

COPY target/*.jar /app/drcars-api.jar

EXPOSE 9090

ENTRYPOINT ["java", "-jar", "drcars-api.jar"]
    