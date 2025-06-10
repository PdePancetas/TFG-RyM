package com.DRCars.dto;

import java.math.BigDecimal;
import java.time.LocalDate;

import com.DRCars.model.Trabajador;

public class ReservaDTO {
	
	private Long idReserva;
	private ClienteDTO cliente;
	private VehiculoDTO vehiculo;
	private LocalDate fechaReserva;
	private BigDecimal precioReserva;
	private Trabajador trabajador;
	private String notas;
	
	
	public Long getIdReserva() {
		return idReserva;
	}
	public void setIdReserva(Long idReserva) {
		this.idReserva = idReserva;
	}
	public ClienteDTO getCliente() {
		return cliente;
	}
	public void setCliente(ClienteDTO cliente) {
		this.cliente = cliente;
	}
	public VehiculoDTO getVehiculo() {
		return vehiculo;
	}
	public void setVehiculo(VehiculoDTO vehiculo) {
		this.vehiculo = vehiculo;
	}
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
	public Trabajador getTrabajador() {
		return trabajador;
	}
	public void setTrabajador(Trabajador trabajador) {
		this.trabajador = trabajador;
	}
	
	public String getNotas() {
		return notas;
	}
	public void setNotas(String notas) {
		this.notas = notas;
	}
	
	
	
}
