package com.RDCars.model;

import java.time.LocalDate;
import java.util.Set;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.OneToMany;
import jakarta.persistence.PrimaryKeyJoinColumn;
import jakarta.persistence.Table;

//Trabajador Entity
@Entity
@Table(name = "TRABAJADORES")
@PrimaryKeyJoinColumn(name = "id_trabajador")
public class Trabajador extends Usuario {
 @Column(name = "nombre", nullable = false)
 private String nombre;

 @Column(name = "apellidos", nullable = false)
 private String apellidos;

 @Column(name = "dni_nif", nullable = false)
 private String dniNif;

 @Column(name = "puesto")
 private String puesto;

 @Column(name = "fecha_contrato")
 private LocalDate fechaContrato;

 @Column(name = "direccion")
 private String direccion;

 @Column(name = "ciudad")
 private String ciudad;

 @Column(name = "codigo_postal")
 private String codigoPostal;

 @OneToMany(mappedBy = "trabajador")
 private Set<Venta> ventas;

 // Getters and setters
}
