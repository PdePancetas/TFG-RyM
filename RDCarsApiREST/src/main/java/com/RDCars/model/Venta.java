package com.RDCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

//Venta Entity
@Entity
@Table(name = "VENTAS")
public class Venta implements Serializable {
 @Id
 @GeneratedValue(strategy = GenerationType.IDENTITY)
 @Column(name = "id_venta")
 private Long idVenta;

 @ManyToOne
 @JoinColumn(name = "id_cliente", nullable = false)
 private Cliente cliente;

 @ManyToOne
 @JoinColumn(name = "id_vehiculo", nullable = false)
 private Vehiculo vehiculo;

 @ManyToOne
 @JoinColumn(name = "id_trabajador")
 private Trabajador trabajador;

 @Column(name = "fecha_venta", nullable = false)
 private LocalDate fechaVenta;

 @Column(name = "precio_venta", nullable = false)
 private BigDecimal precioVenta;

 // Getters and setters
}
