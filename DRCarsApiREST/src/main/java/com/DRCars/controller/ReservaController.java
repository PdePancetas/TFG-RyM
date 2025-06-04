package com.DRCars.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;

import com.DRCars.dto.ReservaDTO;
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

}
