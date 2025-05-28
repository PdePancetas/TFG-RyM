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
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.VehiculoDTO;
import com.DRCars.dto.VehiculoRequest;
import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Vehiculo;
import com.DRCars.service.impl.VehiculoServiceImpl;

@RestController
@RequestMapping("/catalogo")
public class CatalogoController {

	@Autowired
	private VehiculoServiceImpl vehiculoService;

	@GetMapping
	public ResponseEntity<List<VehiculoDTO>> obtenerCatalogo() {
		List<Vehiculo> vehiculos = vehiculoService.obtenerVehiculos();
		List<VehiculoDTO> vehiculosDTO = vehiculos.stream().map(VehiculoMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(vehiculosDTO);
	}

	@PostMapping("/crear")
	public ResponseEntity<Vehiculo> addVehiculo(@RequestBody VehiculoRequest vehiculo) {
		Vehiculo v = null;
		try {
			v = vehiculoService.anyadirVehiculo(vehiculo);
			//return ResponseEntity.ok("Vehiculo añadido con éxito");
			return ResponseEntity.ok(v);
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(v);
			//return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al añadir el vehiculo");
		}
	}

	@GetMapping("/{id}")
	public ResponseEntity<Vehiculo> getVehiculo(@PathVariable Long id) {
		return ResponseEntity.of(vehiculoService.obtenerVehiculoPorId(id));
	}

	@PostMapping("/act")
	public ResponseEntity<Vehiculo> updtVehiculo(@RequestBody Vehiculo v) {
		Vehiculo vehiculo = null;
		try {
			vehiculo = vehiculoService.actualizarVehiculo(v);
			return ResponseEntity.ok(vehiculo);
//			return ResponseEntity.ok("El vehiculo con id "+v.getIdVehiculo()+" se ha actualizado correctamente");
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(vehiculo);
//			return ResponseEntity.ok("El vehiculo con id "+v.getIdVehiculo()+" no se pudo actualizar");
		}
		
	}

}
