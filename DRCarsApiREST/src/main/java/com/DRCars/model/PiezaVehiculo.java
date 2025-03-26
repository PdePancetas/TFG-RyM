package com.DRCars.model;

import java.io.Serializable;
import java.time.LocalDate;
import java.util.Objects;

import jakarta.persistence.Column;
import jakarta.persistence.EmbeddedId;
import jakarta.persistence.Entity;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.MapsId;
import jakarta.persistence.Table;

@Entity
@Table(name = "PIEZAS_VEHICULOS")
public class PiezaVehiculo implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -1998368630371880371L;

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

	public PiezaVehiculo() {
		super();
	}

	public PiezaVehiculoId getId() {
		return id;
	}

	public void setId(PiezaVehiculoId id) {
		this.id = id;
	}

	public Integer getCantidad() {
		return cantidad;
	}

	public void setCantidad(Integer cantidad) {
		this.cantidad = cantidad;
	}

	public LocalDate getFechaInstalacion() {
		return fechaInstalacion;
	}

	public void setFechaInstalacion(LocalDate fechaInstalacion) {
		this.fechaInstalacion = fechaInstalacion;
	}

	public Pieza getPieza() {
		return pieza;
	}

	public void setPieza(Pieza pieza) {
		this.pieza = pieza;
	}

	public Vehiculo getVehiculo() {
		return vehiculo;
	}

	public void setVehiculo(Vehiculo vehiculo) {
		this.vehiculo = vehiculo;
	}

	@Override
	public int hashCode() {
		return Objects.hash(id);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		PiezaVehiculo other = (PiezaVehiculo) obj;
		return Objects.equals(id, other.id);
	}

}
