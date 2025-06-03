package com.DRCars.service;

import java.util.List;

import org.springframework.stereotype.Service;

import com.DRCars.dto.ProcReservaRequest;
import com.DRCars.dto.SolicitudRequest;
import com.DRCars.model.Solicitud;

public interface SolicitudService {

	void crearSolicitud(SolicitudRequest solicitud);
	
	List<Solicitud> obtenerSolicitudes();
	
}
