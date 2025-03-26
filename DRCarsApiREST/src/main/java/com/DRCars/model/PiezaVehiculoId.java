package com.DRCars.model;

import java.io.Serializable;
import java.util.Objects;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;

//Composite Key for PiezaVehiculo
@Embeddable
public class PiezaVehiculoId implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = -335026211207178807L;

	@Column(name = "id_pieza")
	private Long idPieza;

	@Column(name = "id_vehiculo")
	private Long idVehiculo;

	public PiezaVehiculoId() {
		super();
	}

	public Long getIdPieza() {
		return idPieza;
	}

	public void setIdPieza(Long idPieza) {
		this.idPieza = idPieza;
	}

	public Long getIdVehiculo() {
		return idVehiculo;
	}

	public void setIdVehiculo(Long idVehiculo) {
		this.idVehiculo = idVehiculo;
	}

	@Override
	public int hashCode() {
		return Objects.hash(idPieza, idVehiculo);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		PiezaVehiculoId other = (PiezaVehiculoId) obj;
		return Objects.equals(idPieza, other.idPieza) && Objects.equals(idVehiculo, other.idVehiculo);
	}

}
