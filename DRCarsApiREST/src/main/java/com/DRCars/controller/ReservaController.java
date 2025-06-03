package com.DRCars.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.ProcReservaRequest;
import com.DRCars.dto.SolicitudDTO;
import com.DRCars.dto.SolicitudRequest;
import com.DRCars.dto.ReservasClienteRequest;
import com.DRCars.dto.VehiculoDTO;
import com.DRCars.mapper.SolicitudMapper;
import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Solicitud;
import com.DRCars.model.Vehiculo;
import com.DRCars.service.SolicitudService;
import com.DRCars.service.impl.SolicitudServiceImpl;

@RestController
@RequestMapping("/reservas")
public class ReservaController {

	@Autowired
	private SolicitudServiceImpl solicitudService;

	@GetMapping
	public ResponseEntity<List<SolicitudDTO>> obtenerReservas() {
		List<Solicitud> solicitudes = solicitudService.obtenerSolicitudes();
		List<SolicitudDTO> solicitudesDTO = solicitudes.stream().map(SolicitudMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(solicitudesDTO);
	}

	@PostMapping("/crear")
	public ResponseEntity<String> solicitarSolicitud(@RequestBody SolicitudRequest solicitud) {
		try {
			solicitudService.crearSolicitud(solicitud);
			return ResponseEntity.ok("Reserva realizada con éxito");
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al crear la reserva");
		}
	}
/*
	//Mover a reservas
	@PostMapping("/procesar")
	public ResponseEntity<String> procesarReserva(@RequestBody ProcReservaRequest reserva) {

		try {
			solicitudService.procesarReserva(reserva);
			return ResponseEntity.ok("Reserva procesada con éxito");
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al procesar la reserva");
		}

	}

	//mover a reservas
	@GetMapping("/cliente")
	public ResponseEntity<List<SolicitudDTO>> obtenerReservasPorDni(@RequestBody ReservasClienteRequest id) {

		List<Reserva> reservas = solicitudService.obtenerReservas();

		if (!reservas.isEmpty()) {
			List<Reserva> reservasCliente = reservas.stream().filter(r -> r.getCliente().getDniCliente().equals(id))
					.collect(Collectors.toList());
			if (!reservasCliente.isEmpty()) {
				List<SolicitudDTO> reservasDTO = reservasCliente.stream().map(ReservaMapper.INSTANCE::toDTO)
						.collect(Collectors.toList());
				return ResponseEntity.ok(reservasDTO);
			}
			return ResponseEntity.notFound().build();
		}
		return ResponseEntity.notFound().build();
	}*/
}
