-- =========================================================
-- SCRIPT DE CREACIÓN DE LA BASE DE DATOS DRCars
-- =========================================================

-- 1) Crear la base de datos si no existe:
CREATE DATABASE IF NOT EXISTS DRCars;

-- 2) Seleccionar la base de datos DRCars
USE DRCars;

-- =========================================================
-- 0) TABLA USUARIOS
-- (Permite la autenticación y define el tipo de usuario)
-- =========================================================
CREATE TABLE USUARIOS (
  id_usuario INT AUTO_INCREMENT PRIMARY KEY,
  usuario VARCHAR(50) NOT NULL,
  contraseña VARCHAR(255) NOT NULL,
  tipo_usuario ENUM('admin','user') NOT NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 1) TABLA CLIENTES
-- (id_usuario opcional para vincularlo con USUARIOS)
-- =========================================================
CREATE TABLE CLIENTES (
  id_cliente INT AUTO_INCREMENT PRIMARY KEY,
  id_usuario INT NULL,
  nombre VARCHAR(50) NOT NULL,
  apellidos VARCHAR(50) NOT NULL,
  dni_nif VARCHAR(15) NOT NULL,
  telefono VARCHAR(20),
  email VARCHAR(100),
  direccion VARCHAR(100),
  ciudad VARCHAR(50),
  codigo_postal VARCHAR(10),
  CONSTRAINT fk_cliente_usuario
    FOREIGN KEY (id_usuario)
    REFERENCES USUARIOS (id_usuario)
    ON UPDATE CASCADE
    ON DELETE SET NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 2) TABLA TRABAJADORES
-- (id_usuario opcional para vincularlo con USUARIOS)
-- =========================================================
CREATE TABLE TRABAJADORES (
  id_trabajador INT AUTO_INCREMENT PRIMARY KEY,
  id_usuario INT NULL,
  nombre VARCHAR(50) NOT NULL,
  apellidos VARCHAR(50) NOT NULL,
  dni_nif VARCHAR(15) NOT NULL,
  telefono VARCHAR(20),
  email VARCHAR(100),
  puesto VARCHAR(50),
  fecha_contrato DATE,
  direccion VARCHAR(100),
  ciudad VARCHAR(50),
  codigo_postal VARCHAR(10),
  CONSTRAINT fk_trabajador_usuario
    FOREIGN KEY (id_usuario)
    REFERENCES USUARIOS (id_usuario)
    ON UPDATE CASCADE
    ON DELETE SET NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 3) TABLA PROVEEDORES
-- (Con tipo_proveedor = EMPRESA o PARTICULAR)
-- =========================================================
CREATE TABLE PROVEEDORES (
  id_proveedor INT AUTO_INCREMENT PRIMARY KEY,
  tipo_proveedor ENUM('EMPRESA','PARTICULAR') NOT NULL,
  nombre VARCHAR(50) NOT NULL,
  cif_nif VARCHAR(15) NOT NULL,
  telefono VARCHAR(20),
  email VARCHAR(100),
  direccion VARCHAR(100),
  ciudad VARCHAR(50),
  codigo_postal VARCHAR(10)
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 4) TABLA VEHICULOS
-- (Unifica coches de garaje, en venta y vendidos con un campo "estado")
-- =========================================================
CREATE TABLE VEHICULOS (
  id_vehiculo INT AUTO_INCREMENT PRIMARY KEY,
  marca VARCHAR(50) NOT NULL,
  modelo VARCHAR(50) NOT NULL,
  anno_fabricacion YEAR,
  color VARCHAR(30),
  kilometraje INT,
  matricula VARCHAR(15),
  numero_chasis VARCHAR(30),
  precio_compra DECIMAL(10,2),
  estado ENUM('GARAJE','VENTA','VENDIDO') NOT NULL DEFAULT 'GARAJE',
  id_proveedor INT,
  CONSTRAINT fk_vehiculo_proveedor
    FOREIGN KEY (id_proveedor)
    REFERENCES PROVEEDORES (id_proveedor)
    ON UPDATE CASCADE
    ON DELETE SET NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 5) TABLA PIEZAS
-- =========================================================
CREATE TABLE PIEZAS (
  id_pieza INT AUTO_INCREMENT PRIMARY KEY,
  nombre VARCHAR(100) NOT NULL,
  descripcion VARCHAR(255),
  precio DECIMAL(10,2),
  stock INT
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 6) TABLA PIEZAS_VEHICULOS (Intermedia N:M sin autoincrement)
-- Clave primaria compuesta: (id_pieza, id_vehiculo)
-- =========================================================
CREATE TABLE PIEZAS_VEHICULOS (
  id_pieza INT NOT NULL,
  id_vehiculo INT NOT NULL,
  cantidad INT,
  fecha_instalacion DATE,
  PRIMARY KEY (id_pieza, id_vehiculo),
  CONSTRAINT fk_piezas_vehiculo_pieza
    FOREIGN KEY (id_pieza)
    REFERENCES PIEZAS (id_pieza)
    ON UPDATE CASCADE
    ON DELETE RESTRICT,
  CONSTRAINT fk_piezas_vehiculo_vehiculo
    FOREIGN KEY (id_vehiculo)
    REFERENCES VEHICULOS (id_vehiculo)
    ON UPDATE CASCADE
    ON DELETE RESTRICT
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 7) TABLA VENTAS
-- (Registra la venta efectiva de cada vehículo)
-- =========================================================
CREATE TABLE VENTAS (
  id_venta INT AUTO_INCREMENT PRIMARY KEY,
  id_cliente INT NOT NULL,
  id_vehiculo INT NOT NULL,
  id_trabajador INT NULL,
  fecha_venta DATE NOT NULL,
  precio_venta DECIMAL(10,2) NOT NULL,
  CONSTRAINT fk_venta_cliente
    FOREIGN KEY (id_cliente)
    REFERENCES CLIENTES (id_cliente)
    ON UPDATE CASCADE
    ON DELETE RESTRICT,
  CONSTRAINT fk_venta_vehiculo
    FOREIGN KEY (id_vehiculo)
    REFERENCES VEHICULOS (id_vehiculo)
    ON UPDATE CASCADE
    ON DELETE RESTRICT,
  CONSTRAINT fk_venta_trabajador
    FOREIGN KEY (id_trabajador)
    REFERENCES TRABAJADORES (id_trabajador)
    ON UPDATE CASCADE
    ON DELETE SET NULL
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- 8) TABLA RESERVAS
-- (Permite reservar un vehículo antes de la compra)
-- =========================================================
CREATE TABLE RESERVAS (
  id_reserva INT AUTO_INCREMENT PRIMARY KEY,
  id_cliente INT NOT NULL,
  id_vehiculo INT NOT NULL,
  fecha_reserva DATE NOT NULL,
  precio_reserva DECIMAL(10,2) NOT NULL,
  CONSTRAINT fk_reserva_cliente
    FOREIGN KEY (id_cliente)
    REFERENCES CLIENTES (id_cliente)
    ON UPDATE CASCADE
    ON DELETE RESTRICT,
  CONSTRAINT fk_reserva_vehiculo
    FOREIGN KEY (id_vehiculo)
    REFERENCES VEHICULOS (id_vehiculo)
    ON UPDATE CASCADE
    ON DELETE RESTRICT
)
ENGINE=InnoDB
DEFAULT CHARSET=utf8mb4;

-- =========================================================
-- FIN DEL SCRIPT DRCars.sql
-- =========================================================

/*
   USO:
   1. Ejecutar este archivo en tu cliente de MySQL (por ejemplo: MySQL Workbench).
   2. Se creará la base de datos DRCars (si no existe) y se crearán todas las tablas.
   3. Ajustar políticas de ON DELETE y ON UPDATE si lo deseas, e insertar datos de prueba.
*/
