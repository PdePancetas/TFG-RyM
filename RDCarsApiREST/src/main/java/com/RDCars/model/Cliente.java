package com.RDCars.model;

import java.util.Set;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.OneToMany;
import jakarta.persistence.PrimaryKeyJoinColumn;
import jakarta.persistence.Table;

//Cliente Entity
@Entity
@Table(name = "CLIENTES")
@PrimaryKeyJoinColumn(name = "id_cliente")
public class Cliente extends Usuario {
 @Column(name = "nombre", nullable = false)
 private String nombre;

 @Column(name = "apellidos", nullable = false)
 private String apellidos;

 @Column(name = "dni_nif", nullable = false)
 private String dniNif;

 @Column(name = "direccion")
 private String direccion;

 @Column(name = "ciudad")
 private String ciudad;

 @Column(name = "codigo_postal")
 private String codigoPostal;

 @OneToMany(mappedBy = "cliente")
 private Set<Venta> ventas;

 @OneToMany(mappedBy = "cliente")
 private Set<Reserva> reservas;

 // Getters and setters
}