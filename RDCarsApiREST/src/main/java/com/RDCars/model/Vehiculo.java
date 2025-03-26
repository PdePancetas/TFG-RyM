package com.RDCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Set;

import com.RDCars.model.Vehiculo.Estado;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

//Vehiculo Entity
@Entity
@Table(name = "VEHICULOS")
public class Vehiculo implements Serializable {
 @Id
 @GeneratedValue(strategy = GenerationType.IDENTITY)
 @Column(name = "id_vehiculo")
 private Long idVehiculo;

 @Column(name = "marca", nullable = false)
 private String marca;

 @Column(name = "modelo", nullable = false)
 private String modelo;

 @Column(name = "anno_fabricacion")
 private Integer annoFabricacion;

 @Column(name = "color")
 private String color;

 @Column(name = "kilometraje")
 private Integer kilometraje;

 @Column(name = "matricula")
 private String matricula;

 @Column(name = "numero_chasis")
 private String numeroChasis;

 @Column(name = "precio_compra")
 private BigDecimal precioCompra;

 @Enumerated(EnumType.STRING)
 @Column(name = "estado", nullable = false)
 private Estado estado = Estado.GARAJE;

 @ManyToOne
 @JoinColumn(name = "id_proveedor")
 private Proveedor proveedor;

 @OneToMany(mappedBy = "vehiculo")
 private Set<PiezaVehiculo> piezasVehiculos;

 @OneToMany(mappedBy = "vehiculo")
 private Set<Venta> ventas;

 @OneToMany(mappedBy = "vehiculo")
 private Set<Reserva> reservas;

 // Enum for Vehiculo estado
 public enum Estado {
     GARAJE, VENTA, VENDIDO
 }

 // Getters and setters
}
