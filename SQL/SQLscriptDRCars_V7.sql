-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: 192.168.1.100    Database: drcars
-- ------------------------------------------------------
-- Server version	5.7.44
USE drcars;


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `clientes`
--

DROP TABLE IF EXISTS `clientes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `clientes` (
  `dni_cliente` varchar(9) NOT NULL,
  `usuario` varchar(255) NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `apellidos` varchar(255) NOT NULL,
  `telefono` varchar(255) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `ciudad` varchar(255) DEFAULT NULL,
  `codigo_postal` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`dni_cliente`),
  KEY `claveForaneaUsuarios_idx` (`usuario`),
  CONSTRAINT `fk_cliente_usuario` FOREIGN KEY (`usuario`) REFERENCES `usuarios` (`usuario`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `clientes`
--

LOCK TABLES `clientes` WRITE;
/*!40000 ALTER TABLE `clientes` DISABLE KEYS */;
INSERT INTO `clientes` VALUES ('1232321B','blancomiguel.bj@gmail.com','Miguel','Blanco JimÃ©nez',NULL,NULL,NULL,NULL),('12345678A','richi@gmail.com','Ricardo','Romero',NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `clientes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `piezas`
--

DROP TABLE IF EXISTS `piezas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `piezas` (
  `id_pieza` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  `precio` decimal(10,2) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_pieza`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `piezas`
--

LOCK TABLES `piezas` WRITE;
/*!40000 ALTER TABLE `piezas` DISABLE KEYS */;
/*!40000 ALTER TABLE `piezas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `piezas_vehiculos`
--

DROP TABLE IF EXISTS `piezas_vehiculos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `piezas_vehiculos` (
  `id_pieza` int(11) NOT NULL,
  `id_vehiculo` int(11) NOT NULL,
  `cantidad` int(11) DEFAULT NULL,
  `fecha_instalacion` date DEFAULT NULL,
  PRIMARY KEY (`id_pieza`,`id_vehiculo`),
  KEY `fk_piezas_vehiculo_vehiculo` (`id_vehiculo`),
  CONSTRAINT `fk_piezas_vehiculo_pieza` FOREIGN KEY (`id_pieza`) REFERENCES `piezas` (`id_pieza`) ON UPDATE CASCADE,
  CONSTRAINT `fk_piezas_vehiculo_vehiculo` FOREIGN KEY (`id_vehiculo`) REFERENCES `vehiculos` (`id_vehiculo`) ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `piezas_vehiculos`
--

LOCK TABLES `piezas_vehiculos` WRITE;
/*!40000 ALTER TABLE `piezas_vehiculos` DISABLE KEYS */;
/*!40000 ALTER TABLE `piezas_vehiculos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `proveedores`
--

DROP TABLE IF EXISTS `proveedores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `proveedores` (
  `id_proveedor` int(11) NOT NULL AUTO_INCREMENT,
  `tipo_proveedor` enum('EMPRESA','PARTICULAR') NOT NULL,
  `nombre` varchar(255) NOT NULL,
  `cif_nif` varchar(255) NOT NULL,
  `telefono` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `ciudad` varchar(255) DEFAULT NULL,
  `codigo_postal` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_proveedor`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `proveedores`
--

LOCK TABLES `proveedores` WRITE;
/*!40000 ALTER TABLE `proveedores` DISABLE KEYS */;
INSERT INTO `proveedores` VALUES (1,'EMPRESA','Proveedores del Motor S.A.','B12345678','912345678','contacto@proveedoresmotor.com','Calle Ficticia 123','Madrid','28080');
/*!40000 ALTER TABLE `proveedores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservas`
--

DROP TABLE IF EXISTS `reservas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reservas` (
  `id_reserva` int(11) NOT NULL AUTO_INCREMENT,
  `dni_cliente` varchar(9) NOT NULL,
  `id_vehiculo` int(11) NOT NULL,
  `fecha_programada` datetime DEFAULT NULL,
  `precio_reserva` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id_reserva`),
  KEY `fk_reserva_cliente_idx` (`dni_cliente`),
  KEY `fk_reserva_vehiculo_idx` (`id_vehiculo`),
  CONSTRAINT `fk_reserva_cliente` FOREIGN KEY (`dni_cliente`) REFERENCES `clientes` (`dni_cliente`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_reserva_vehiculo` FOREIGN KEY (`id_vehiculo`) REFERENCES `vehiculos` (`id_vehiculo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservas`
--

LOCK TABLES `reservas` WRITE;
/*!40000 ALTER TABLE `reservas` DISABLE KEYS */;
/*!40000 ALTER TABLE `reservas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `solicitudes`
--

DROP TABLE IF EXISTS `solicitudes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `solicitudes` (
  `id_solicitud` int(11) NOT NULL AUTO_INCREMENT,
  `dni_cliente` varchar(9) NOT NULL,
  `id_vehiculo` int(11) DEFAULT NULL,
  `fecha_solicitud` datetime NOT NULL,
  `precio_solicitud` decimal(10,2) NOT NULL,
  `motivo` varchar(255) DEFAULT NULL,
  `descripcion` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_solicitud`),
  KEY `fk_solicitud_cliente_idx` (`dni_cliente`),
  KEY `fk_solicitud_vehiculo_idx` (`id_vehiculo`),
  CONSTRAINT `fk_solicitud_cliente` FOREIGN KEY (`dni_cliente`) REFERENCES `clientes` (`dni_cliente`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_solicitud_vehiculo` FOREIGN KEY (`id_vehiculo`) REFERENCES `vehiculos` (`id_vehiculo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `solicitudes`
--

LOCK TABLES `solicitudes` WRITE;
/*!40000 ALTER TABLE `solicitudes` DISABLE KEYS */;
/*!40000 ALTER TABLE `solicitudes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `trabajadores`
--

DROP TABLE IF EXISTS `trabajadores`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `trabajadores` (
  `id_trabajador` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) DEFAULT NULL,
  `nombre` varchar(255) NOT NULL,
  `apellidos` varchar(255) NOT NULL,
  `dni_nif` varchar(255) NOT NULL,
  `telefono` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `puesto` varchar(255) DEFAULT NULL,
  `fecha_contrato` date DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `ciudad` varchar(255) DEFAULT NULL,
  `codigo_postal` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`id_trabajador`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `trabajadores`
--

LOCK TABLES `trabajadores` WRITE;
/*!40000 ALTER TABLE `trabajadores` DISABLE KEYS */;
/*!40000 ALTER TABLE `trabajadores` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `usuario` varchar(255) NOT NULL,
  `contraseña` varchar(255) NOT NULL,
  `tipo_usuario` enum('ADMIN','USER') NOT NULL,
  `ultimo_acceso` datetime NOT NULL,
  `registro_cuenta` datetime NOT NULL,
  PRIMARY KEY (`usuario`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES ('algo@gmail.com','aa','USER','2025-06-02 20:20:15','2025-06-02 08:14:35'),('blancomiguel.bj@gmail.com','maikol','ADMIN','2025-06-03 19:05:41','2025-05-07 20:56:12'),('richi@gmail.com','richi','ADMIN','2025-06-03 10:40:18','2025-06-01 20:49:34'),('superadmin@gmail.com','admin','ADMIN','2025-06-02 09:19:43','2023-05-21 18:26:00');
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vehiculos`
--

DROP TABLE IF EXISTS `vehiculos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vehiculos` (
  `id_vehiculo` int(11) NOT NULL AUTO_INCREMENT,
  `marca` varchar(50) NOT NULL,
  `modelo` varchar(50) NOT NULL,
  `anno_fabricacion` year(4) DEFAULT NULL,
  `color` varchar(30) DEFAULT NULL,
  `kilometraje` int(11) DEFAULT NULL,
  `matricula` varchar(15) DEFAULT NULL,
  `numero_chasis` varchar(30) DEFAULT NULL,
  `precio_compra` decimal(10,2) DEFAULT NULL,
  `estado` enum('STOCK','GARAJE','VENTA','VENDIDO') NOT NULL DEFAULT 'STOCK',
  `id_proveedor` int(11) DEFAULT NULL,
  `combustible` enum('DIESEL','ELECTRICO','GASOLINA','HIBRIDO') DEFAULT NULL,
  `transmision` enum('MANUAL','AUTOMATICA') DEFAULT NULL,
  PRIMARY KEY (`id_vehiculo`),
  KEY `fk_vehiculo_proveedor` (`id_proveedor`),
  CONSTRAINT `fk_vehiculo_proveedor` FOREIGN KEY (`id_proveedor`) REFERENCES `proveedores` (`id_proveedor`) ON DELETE SET NULL ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vehiculos`
--

LOCK TABLES `vehiculos` WRITE;
/*!40000 ALTER TABLE `vehiculos` DISABLE KEYS */;
INSERT INTO `vehiculos` VALUES (1,'Toyota','Corolla',2027,'Verde',50000,'1234ABC','CHS1234567890',15000.00,'VENTA',1,'DIESEL','MANUAL'),(2,'Ford','Focus',2019,'Azul',60000,'5678DEF','CHS2345678901',10500.00,'GARAJE',1,'HIBRIDO','MANUAL'),(3,'Tesla','Model 3',2022,'Negro',15000,'9012GHI','CHS3456789012',35000.00,'VENTA',1,'ELECTRICO','AUTOMATICA'),(4,'Seat','IbizaMeLaComeDepriza',2018,'Blanco',72000,'3456JKL','CHS4567890123',8900.00,'VENDIDO',1,'DIESEL','MANUAL'),(5,'Renault','Clio',2021,'Gris',20000,'7890MNO','CHS5678901234',14000.00,'VENTA',1,'HIBRIDO','AUTOMATICA'),(6,'Ferrari','LaFerrari',2020,'Rojo',45000,'1234ABC','JH4KA8260MC000000',15000.50,'VENDIDO',1,'GASOLINA','AUTOMATICA'),(7,'Renault','Laguna',2010,NULL,117000,NULL,NULL,10000.00,'STOCK',1,'DIESEL','MANUAL'),(8,'aaa','aaa',2020,NULL,234,NULL,NULL,323.00,'STOCK',1,'GASOLINA','MANUAL'),(9,'Ferrari','Arkana',2020,NULL,20000,NULL,NULL,15000.00,'STOCK',NULL,'GASOLINA','MANUAL');
/*!40000 ALTER TABLE `vehiculos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vehiculos_favoritos`
--

DROP TABLE IF EXISTS `vehiculos_favoritos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `vehiculos_favoritos` (
  `id_usuario` int(11) NOT NULL,
  `id_vehiculo` int(11) NOT NULL,
  PRIMARY KEY (`id_usuario`,`id_vehiculo`),
  KEY `fk_id_vehiculo_idx` (`id_vehiculo`),
  CONSTRAINT `fk_id_vehiculo` FOREIGN KEY (`id_vehiculo`) REFERENCES `vehiculos` (`id_vehiculo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vehiculos_favoritos`
--

LOCK TABLES `vehiculos_favoritos` WRITE;
/*!40000 ALTER TABLE `vehiculos_favoritos` DISABLE KEYS */;
/*!40000 ALTER TABLE `vehiculos_favoritos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ventas`
--

DROP TABLE IF EXISTS `ventas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ventas` (
  `id_venta` int(11) NOT NULL AUTO_INCREMENT,
  `dni_cliente` varchar(9) NOT NULL,
  `id_vehiculo` int(11) NOT NULL,
  `id_trabajador` int(11) DEFAULT NULL,
  `fecha_venta` datetime DEFAULT NULL,
  `precio_venta` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`id_venta`),
  KEY `fk_venta_cliente_idx` (`dni_cliente`),
  KEY `fk_venta_vehiculo_idx` (`id_vehiculo`),
  KEY `fk_venta_trabajador_idx` (`id_trabajador`),
  CONSTRAINT `fk_venta_cliente` FOREIGN KEY (`dni_cliente`) REFERENCES `clientes` (`dni_cliente`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_venta_trabajador` FOREIGN KEY (`id_trabajador`) REFERENCES `trabajadores` (`id_trabajador`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_venta_vehiculo` FOREIGN KEY (`id_vehiculo`) REFERENCES `vehiculos` (`id_vehiculo`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ventas`
--

LOCK TABLES `ventas` WRITE;
/*!40000 ALTER TABLE `ventas` DISABLE KEYS */;
/*!40000 ALTER TABLE `ventas` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-04  0:41:55
