package com.DRCars.dto;

import java.math.BigDecimal;
import java.time.LocalDateTime;
import java.time.Year;

public class ReservaDTO {
	
    private int idReserva;
    private ClienteDTO cliente;
    private VehiculoDTOreserva vehiculo;
    private LocalDateTime fechaReserva;
    private BigDecimal precioReserva;
    private Year annoFabricacion;
    private String descripcion;

    public ReservaDTO() {
    	super();
    }

	public int getIdReserva() {
		return idReserva;
	}

	public void setIdReserva(int idReserva) {
		this.idReserva = idReserva;
	}

	public ClienteDTO getCliente() {
		return cliente;
	}

	public void setCliente(ClienteDTO cliente) {
		this.cliente = cliente;
	}

	public VehiculoDTOreserva getVehiculo() {
		return vehiculo;
	}

	public void setVehiculo(VehiculoDTOreserva vehiculo) {
		this.vehiculo = vehiculo;
	}

	public LocalDateTime getFechaReserva() {
		return fechaReserva;
	}

	public void setFechaReserva(LocalDateTime fechaReserva) {
		this.fechaReserva = fechaReserva;
	}

	public BigDecimal getPrecioReserva() {
		return precioReserva;
	}

	public void setPrecioReserva(BigDecimal precioReserva) {
		this.precioReserva = precioReserva;
	}

	public Year getAnnoFabricacion() {
		return annoFabricacion;
	}

	public void setAnnoFabricacion(Year annoFabricacion) {
		this.annoFabricacion = annoFabricacion;
	}

	public String getDescripcion() {
		return descripcion;
	}

	public void setDescripcion(String descripcion) {
		this.descripcion = descripcion;
	}

    
    
}
