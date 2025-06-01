package com.DRCars.dto;

import java.time.Year;

public class VehiculoDTOreserva {

	private Long idVehiculo;
	private String marca;
	private String modelo;
	private String estado;
	private Year annoFabricacion;
	
	public VehiculoDTOreserva() {
		super();
	}

	public Long getIdVehiculo() {
		return idVehiculo;
	}

	public void setIdVehiculo(Long idVehiculo) {
		this.idVehiculo = idVehiculo;
	}

	public String getMarca() {
		return marca;
	}

	public void setMarca(String marca) {
		this.marca = marca;
	}

	public String getModelo() {
		return modelo;
	}

	public void setModelo(String modelo) {
		this.modelo = modelo;
	}

	public String getEstado() {
		return estado;
	}

	public void setEstado(String estado) {
		this.estado = estado;
	}

	public Year getAnnoFabricacion() {
		return annoFabricacion;
	}

	public void setAnnoFabricacion(Year annoFabricacion) {
		this.annoFabricacion = annoFabricacion;
	}
	
	
	
}
