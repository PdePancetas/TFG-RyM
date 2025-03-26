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

//Reserva Entity
@Entity
@Table(name = "RESERVAS")
public class Reserva implements Serializable {
 @Id
 @GeneratedValue(strategy = GenerationType.IDENTITY)
 @Column(name = "id_reserva")
 private Long idReserva;

 @ManyToOne
 @JoinColumn(name = "id_cliente", nullable = false)
 private Cliente cliente;

 @ManyToOne
 @JoinColumn(name = "id_vehiculo", nullable = false)
 private Vehiculo vehiculo;

 @Column(name = "fecha_reserva", nullable = false)
 private LocalDate fechaReserva;

 @Column(name = "precio_reserva", nullable = false)
 private BigDecimal precioReserva;

 // Getters and setters
}