package com.DRCars.dto;

import java.time.Year;

import com.DRCars.model.Vehiculo.Combustible;
import com.DRCars.model.Vehiculo.Estado;
import com.DRCars.model.Vehiculo.Transmision;

public class VehiculoRequest {

	private String marca;
	private String modelo;
	private Year annoFabricacion;
	private String color;
	private Integer kilometraje;
	private String matricula;
	private String numero_chasis;
	private Double precioCompra;
	private Estado estado;
	private Combustible combustible;
	private Transmision transmision;
	

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

	public Year getAnnoFabricacion() {
		return annoFabricacion;
	}

	public void setAnnoFabricacion(Year annoFabricacion) {
		this.annoFabricacion = annoFabricacion;
	}

	public String getColor() {
		return color;
	}

	public void setColor(String color) {
		this.color = color;
	}

	public Integer getKilometraje() {
		return kilometraje;
	}

	public void setKilometraje(Integer kilometraje) {
		this.kilometraje = kilometraje;
	}

	public String getMatricula() {
		return matricula;
	}

	public void setMatricula(String matricula) {
		this.matricula = matricula;
	}

	public String getNumero_chasis() {
		return numero_chasis;
	}

	public void setNumero_chasis(String numero_chasis) {
		this.numero_chasis = numero_chasis;
	}

	public Double getPrecioCompra() {
		return precioCompra;
	}

	public void setPrecioCompra(Double precioCompra) {
		this.precioCompra = precioCompra;
	}

	public Estado getEstado() {
		return estado;
	}

	public void setEstado(Estado estado) {
		this.estado = estado;
	}

	public Combustible getCombustible() {
		return combustible;
	}

	public void setCombustible(Combustible combustible) {
		this.combustible = combustible;
	}

	public Transmision getTransmision() {
		return transmision;
	}

	public void setTransmision(Transmision transmision) {
		this.transmision = transmision;
	}
	
	
	
}
