package com.DRCars.controller;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.ProcSolicitudRequest;
import com.DRCars.dto.SolicitudDTO;
import com.DRCars.dto.SolicitudRequest;
import com.DRCars.mapper.SolicitudMapper;
import com.DRCars.model.Reserva;
import com.DRCars.model.Solicitud;
import com.DRCars.service.impl.SolicitudServiceImpl;

@RestController
@RequestMapping("/solicitudes")
public class SolicitudController {

	@Autowired
	private SolicitudServiceImpl solicitudService;

	@GetMapping
	public ResponseEntity<List<SolicitudDTO>> obtenerSolicitudes() {
		List<Solicitud> solicitudes = solicitudService.obtenerSolicitudes();
		List<SolicitudDTO> solicitudesDTO = solicitudes.stream().map(SolicitudMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(solicitudesDTO);
	}

	@PostMapping("/crear")
	public ResponseEntity<String> solicitarSolicitud(@RequestBody SolicitudRequest solicitud) {
		try {
			solicitudService.crearSolicitud(solicitud);
			return ResponseEntity.ok("Solicitud realizada con éxito");
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al crear la solicitud");
		}
	}
	
	@PostMapping("/delete")
	public ResponseEntity<String> eliminarSolicitud(@RequestBody Solicitud r) {
		Optional<Solicitud> reserva = solicitudService.obtenerSolicitudPorId(r.getIdSolicitud());

		if (!reserva.isEmpty()) {
			solicitudService.eliminarSolicitud(r.getIdSolicitud());
			return ResponseEntity.ok("La reserva se ha eliminado correctamente");
		}
		return ResponseEntity.notFound().build();

	}

	@PostMapping("/procesar")
	public ResponseEntity<String> procesarSolicitud(@RequestBody ProcSolicitudRequest solicitud) {

		try {
			solicitudService.procesarSolicitud(solicitud);
			return ResponseEntity.ok("Solicitud procesada con éxito");
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al procesar la solicitud");
		}

	}
	
}
