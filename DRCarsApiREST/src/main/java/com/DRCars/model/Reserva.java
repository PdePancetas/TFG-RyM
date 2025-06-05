package com.DRCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.time.LocalDate;
import java.util.Objects;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.Table;

@Entity
@Table(name = "RESERVAS")
public class Reserva implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 8044411992315177605L;

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id_reserva")
	private Long idReserva;

	@ManyToOne
	@JoinColumn(name = "dni_cliente", nullable = false)
	private Cliente cliente;

	@ManyToOne
	@JoinColumn(name = "id_vehiculo", nullable = true)
	private Vehiculo vehiculo;

	/*@ManyToOne
	@JoinColumn(name = "id_trabajador")
	private Trabajador trabajador;*/

	@Column(name = "fecha_programada", nullable = true)
	private LocalDate fechaReserva;

	@Column(name = "precio_reserva", nullable = true)
	private BigDecimal precioReserva;

	@Column(name = "notas", nullable = true)
	private String notas;
	
	public Reserva() {
		super();
	}

	public Long getIdReserva() {
		return idReserva;
	}

	public void setIdReserva(Long idReserva) {
		this.idReserva = idReserva;
	}

	public Cliente getCliente() {
		return cliente;
	}

	public void setCliente(Cliente cliente) {
		this.cliente = cliente;
	}

	public Vehiculo getVehiculo() {
		return vehiculo;
	}

	public void setVehiculo(Vehiculo vehiculo) {
		this.vehiculo = vehiculo;
	}

//	public Trabajador getTrabajador() {
//		return trabajador;
//	}
//
//	public void setTrabajador(Trabajador trabajador) {
//		this.trabajador = trabajador;
//	}

	public LocalDate getFechaReserva() {
		return fechaReserva;
	}

	public void setFechaReserva(LocalDate fechaReserva) {
		this.fechaReserva = fechaReserva;
	}

	public BigDecimal getPrecioReserva() {
		return precioReserva;
	}

	public void setPrecioReserva(BigDecimal precioReserva) {
		this.precioReserva = precioReserva;
	}
	
	public String getNotas() {
		return notas;
	}
	public void setNotas(String notas) {
		this.notas = notas;
	}

	@Override
	public int hashCode() {
		return Objects.hash(idReserva);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Reserva other = (Reserva) obj;
		return Objects.equals(idReserva, other.idReserva);
	}

}
