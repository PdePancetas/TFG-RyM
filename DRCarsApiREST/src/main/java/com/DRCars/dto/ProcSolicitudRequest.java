package com.DRCars.dto;

import java.time.LocalDate;

public class ProcSolicitudRequest {

	private Long idSolicitud;
	private boolean aceptada;
	private LocalDate fechaSolicitud;
	private double precioSolicitud;
	private String notas;

	public Long getIdSolicitud() {
		return idSolicitud;
	}

	public void setIdSolicitud(Long idSolicitud) {
		this.idSolicitud = idSolicitud;
	}

	public boolean isAceptada() {
		return aceptada;
	}

	public void setAceptada(boolean aceptada) {
		this.aceptada = aceptada;
	}

	public LocalDate getFechaSolicitud() {
		return fechaSolicitud;
	}

	public void setFechaSolicitud(LocalDate fechaSolicitud) {
		this.fechaSolicitud = fechaSolicitud;
	}

	public double getPrecioSolicitud() {
		return precioSolicitud;
	}

	public void setPrecioSolicitud(double precioSolicitud) {
		this.precioSolicitud = precioSolicitud;
	}

	public String getNotas() {
		return notas;
	}

	public void setNotas(String notas) {
		this.notas = notas;
	}

}
