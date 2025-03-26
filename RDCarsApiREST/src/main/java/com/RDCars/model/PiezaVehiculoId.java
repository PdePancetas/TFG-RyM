package com.RDCars.model;

import java.io.Serializable;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;

//Composite Key for PiezaVehiculo
@Embeddable
public class PiezaVehiculoId implements Serializable {
 @Column(name = "id_pieza")
 private Long idPieza;

 @Column(name = "id_vehiculo")
 private Long idVehiculo;

 // Constructors, equals, hashCode
}
