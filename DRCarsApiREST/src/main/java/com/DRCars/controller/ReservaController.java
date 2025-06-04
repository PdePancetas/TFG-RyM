package com.DRCars.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestBody;

import com.DRCars.dto.ReservaDTO;
import com.DRCars.dto.ReservasClienteRequest;
import com.DRCars.mapper.ReservaMapper;
import com.DRCars.model.Reserva;
import com.DRCars.service.impl.ReservaServiceImpl;

public class ReservaController {

	@Autowired
	ReservaServiceImpl reservaService;
	
	@GetMapping
	public ResponseEntity<List<ReservaDTO>> getReservas(){
		List<Reserva> reservas = reservaService.obtenerReservas();
		List<ReservaDTO> reservasDTO = reservas.stream().map(ReservaMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(reservasDTO);
	}
	
	@GetMapping("/cliente")
	public ResponseEntity<List<ReservaDTO>> obtenerReservasPorDni(@RequestBody ReservasClienteRequest id) {

		List<Reserva> reservas = reservaService.obtenerReservas();

		if (!reservas.isEmpty()) {
			List<Reserva> reservasCliente = reservas.stream().filter(r -> r.getCliente().getDniCliente().equals(id.getDni()))
					.collect(Collectors.toList());
			if (!reservasCliente.isEmpty()) {
				List<ReservaDTO> reservasDTO = reservasCliente.stream().map(ReservaMapper.INSTANCE::toDTO)
						.collect(Collectors.toList());
				return ResponseEntity.ok(reservasDTO);
			}
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.notFound().build();
	}

}
