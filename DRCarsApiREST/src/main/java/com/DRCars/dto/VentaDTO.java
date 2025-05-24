package com.DRCars.dto;

import java.math.BigDecimal;
import java.time.LocalDate;

import com.DRCars.model.Trabajador;

public class VentaDTO {

	private Long idVenta;
	private ClienteDTO cliente;
	private VehiculoDTO vehiculo;
	private LocalDate fechaVenta;
	private BigDecimal precioVenta;
	private Trabajador trabajador;
	public Long getIdVenta() {
		return idVenta;
	}
	public void setIdVenta(Long idVenta) {
		this.idVenta = idVenta;
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
	public LocalDate getFechaVenta() {
		return fechaVenta;
	}
	public void setFechaVenta(LocalDate fechaVenta) {
		this.fechaVenta = fechaVenta;
	}
	public BigDecimal getPrecioVenta() {
		return precioVenta;
	}
	public void setPrecioVenta(BigDecimal precioVenta) {
		this.precioVenta = precioVenta;
	}
	public Trabajador getTrabajador() {
		return trabajador;
	}
	public void setTrabajador(Trabajador trabajador) {
		this.trabajador = trabajador;
	}
	
	
	
}
