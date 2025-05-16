package com.DRCars.service;

import java.util.List;

import org.springframework.stereotype.Service;

import com.DRCars.dto.ReservaDTO;
import com.DRCars.dto.ReservaRequest;
import com.DRCars.model.Reserva;

public interface ReservaService {

	void crearReserva(ReservaRequest reserva);
	
	List<Reserva> obtenerReservas();
	
}
