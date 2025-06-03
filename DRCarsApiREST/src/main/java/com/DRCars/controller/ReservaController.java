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
import com.DRCars.dto.ReservaDTO;
import com.DRCars.dto.ReservaRequest;
import com.DRCars.dto.ReservasClienteRequest;
import com.DRCars.dto.VehiculoDTO;
import com.DRCars.mapper.ReservaMapper;
import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Reserva;
import com.DRCars.model.Vehiculo;
import com.DRCars.service.ReservaService;
import com.DRCars.service.impl.ReservaServiceImpl;

@RestController
@RequestMapping("/reservas")
public class ReservaController {

	@Autowired
	private ReservaServiceImpl reservaService;

	@GetMapping
	public ResponseEntity<List<ReservaDTO>> obtenerReservas() {
		List<Reserva> reservas = reservaService.obtenerReservas();
		List<ReservaDTO> reservasDTO = reservas.stream().map(ReservaMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(reservasDTO);
	}

	@PostMapping("/crear")
	public ResponseEntity<String> solicitarReserva(@RequestBody ReservaRequest reserva) {
		try {
			reservaService.crearReserva(reserva);
			return ResponseEntity.ok("Reserva realizada con éxito");
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al crear la reserva");
		}
	}

	@PostMapping("/procesar")
	public ResponseEntity<String> procesarReserva(@RequestBody ProcReservaRequest reserva) {

		try {
			reservaService.procesarReserva(reserva);
			return ResponseEntity.ok("Reserva procesada con éxito");
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al procesar la reserva");
		}

	}

	@GetMapping("/cliente")
	public ResponseEntity<List<ReservaDTO>> obtenerReservasPorDni(@RequestBody ReservasClienteRequest id) {

		List<Reserva> reservas = reservaService.obtenerReservas();

		if (!reservas.isEmpty()) {
			List<Reserva> reservasCliente = reservas.stream().filter(r -> r.getCliente().getDniCliente().equals(id))
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
