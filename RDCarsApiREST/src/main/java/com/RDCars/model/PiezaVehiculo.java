package com.RDCars.model;

import java.io.Serializable;
import java.time.LocalDate;

import jakarta.persistence.Column;
import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.MapsId;
import jakarta.persistence.Table;

//PiezaVehiculo Entity (Composite Key)
@Entity
@Table(name = "PIEZAS_VEHICULOS")
public class PiezaVehiculo implements Serializable {
 @EmbeddedId
 private PiezaVehiculoId id;

 @Column(name = "cantidad")
 private Integer cantidad;

 @Column(name = "fecha_instalacion")
 private LocalDate fechaInstalacion;

 @ManyToOne
 @MapsId("idPieza")
 @JoinColumn(name = "id_pieza")
 private Pieza pieza;

 @ManyToOne
 @MapsId("idVehiculo")
 @JoinColumn(name = "id_vehiculo")
 private Vehiculo vehiculo;

 // Getters and setters
}
