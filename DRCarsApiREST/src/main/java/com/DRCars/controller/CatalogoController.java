package com.DRCars.controller;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.EstadoVehiculoRequest;
import com.DRCars.dto.VehiculoDTO;
import com.DRCars.dto.VehiculoRequest;
import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Vehiculo;
import com.DRCars.model.Vehiculo.Estado;
import com.DRCars.service.impl.VehiculoServiceImpl;

@RestController
@RequestMapping("/catalogo")
public class CatalogoController {

	@Autowired
	private VehiculoServiceImpl vehiculoService;

	@GetMapping("/app")
	public ResponseEntity<List<VehiculoDTO>> obtenerCatalogo() {
		List<Vehiculo> vehiculos = vehiculoService.obtenerVehiculos();
		List<VehiculoDTO> vehiculosDTO = vehiculos.stream().map(VehiculoMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(vehiculosDTO);
	}

	@GetMapping("/web")
	public ResponseEntity<List<VehiculoDTO>> obtenerCatalogoDisponibles() {
		List<Vehiculo> vehiculos = vehiculoService.obtenerVehiculos();
		List<VehiculoDTO> vehiculosDTO = vehiculos.stream().filter(v -> v.getEstado() != Estado.VENDIDO && v.getEstado()!= Estado.GARAJE
				&& v.getEstado() != Estado.VENTA)
				.map(VehiculoMapper.INSTANCE::toDTO).collect(Collectors.toList());
		return ResponseEntity.ok(vehiculosDTO);
	}

	@PostMapping("/crear")
	public ResponseEntity<VehiculoDTO> addVehiculo(@RequestBody VehiculoRequest vehiculo) {
		Vehiculo v = null;
		try {
			v = vehiculoService.anyadirVehiculo(vehiculo);
			return ResponseEntity.ok(VehiculoMapper.INSTANCE.toDTO(v));
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(VehiculoMapper.INSTANCE.toDTO(v));
		}
	}

	@GetMapping("/{id}")
	public ResponseEntity<VehiculoDTO> getVehiculo(@PathVariable Long id) {
		Optional<Vehiculo> v = vehiculoService.obtenerVehiculoPorId(id);
		if (v.isPresent()) {
			VehiculoDTO vdto = VehiculoMapper.INSTANCE.toDTO(v.get());
			return ResponseEntity.ok(vdto);
		} else {
			return ResponseEntity.notFound().build();
		}

	}

	@PostMapping("/act")
	public ResponseEntity<VehiculoDTO> updtVehiculo(@RequestBody Vehiculo v) {
		Optional<Vehiculo> vehiculo = null;
		try {
			vehiculo = vehiculoService.obtenerVehiculoPorId(v.getIdVehiculo());

			if (vehiculo.isEmpty())
				return ResponseEntity.status(HttpStatus.NOT_FOUND).body(null);

			Vehiculo actualizado = vehiculoService.actualizarVehiculo(v);
			return ResponseEntity.ok(VehiculoMapper.INSTANCE.toDTO(actualizado));
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
					.body(VehiculoMapper.INSTANCE.toDTO(vehiculo.get()));
		}
	}

	@PostMapping("/actEstado")
	public ResponseEntity<VehiculoDTO> actEstadoVehiculo(@RequestBody EstadoVehiculoRequest v) {
		Optional<Vehiculo> vehiculo = null;
		try {
			vehiculo = vehiculoService.obtenerVehiculoPorId(v.getIdVehiculo());

			if (vehiculo.isEmpty())
				return ResponseEntity.status(HttpStatus.NOT_FOUND).build();

			Vehiculo actualizado = vehiculoService.actualizarEstado(vehiculo.get(), v.getEstado());
			return ResponseEntity.ok(VehiculoMapper.INSTANCE.toDTO(actualizado));
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR)
					.body(VehiculoMapper.INSTANCE.toDTO(vehiculo.get()));
		}
		
	}

}
