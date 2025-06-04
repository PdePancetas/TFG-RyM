package com.DRCars.controller;

import java.util.List;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.VentaDTO;
import com.DRCars.mapper.VentaMapper;
import com.DRCars.model.Venta;
import com.DRCars.service.impl.VentaServiceImpl;

@RestController
@RequestMapping("/ventas")
public class VentaController {

	@Autowired
	VentaServiceImpl ventaService;
	
	@GetMapping
	public ResponseEntity<List<VentaDTO>> getVentas(){
		List<Venta> ventas = ventaService.obtenerVentas();
		List<VentaDTO> ventasDTO = ventas.stream().map(VentaMapper.INSTANCE::toDTO)
				.collect(Collectors.toList());
		return ResponseEntity.ok(ventasDTO);
	}
	
}
