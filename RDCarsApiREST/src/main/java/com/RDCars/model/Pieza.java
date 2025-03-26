package com.RDCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Set;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

//Pieza Entity
@Entity
@Table(name = "PIEZAS")
public class Pieza implements Serializable {
 @Id
 @GeneratedValue(strategy = GenerationType.IDENTITY)
 @Column(name = "id_pieza")
 private Long idPieza;

 @Column(name = "nombre", nullable = false)
 private String nombre;

 @Column(name = "descripcion")
 private String descripcion;

 @Column(name = "precio")
 private BigDecimal precio;

 @Column(name = "stock")
 private Integer stock;

 @OneToMany(mappedBy = "pieza")
 private Set<PiezaVehiculo> piezasVehiculos;

 // Getters and setters
}
