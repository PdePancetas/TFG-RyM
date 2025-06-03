package com.DRCars.dto;

import java.math.BigDecimal;
import java.time.LocalDateTime;

public class SolicitudDTO {
	
    private int idSolicitud;
    private ClienteDTO cliente;
    private VehiculoDTOreserva vehiculo;
    private LocalDateTime fechaSolicitud;
    private BigDecimal precioSolicitud;
    private String descripcion;
    private String motivo;

    public SolicitudDTO() {
    	super();
    }

	public int getIdSolicitud() {
		return idSolicitud;
	}

	public void setIdSolicitud(int idSolicitud) {
		this.idSolicitud = idSolicitud;
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

	public LocalDateTime getFechaSolicitud() {
		return fechaSolicitud;
	}

	public void setFechaSolicitud(LocalDateTime fechaSolicitud) {
		this.fechaSolicitud = fechaSolicitud;
	}

	public BigDecimal getPrecioSolicitud() {
		return precioSolicitud;
	}

	public void setPrecioSolicitud(BigDecimal precioSolicitud) {
		this.precioSolicitud = precioSolicitud;
	}

	public String getDescripcion() {
		return descripcion;
	}

	public void setDescripcion(String descripcion) {
		this.descripcion = descripcion;
	}

	public String getMotivo() {
		return motivo;
	}

	public void setMotivo(String motivo) {
		this.motivo = motivo;
	}

    
    
}
