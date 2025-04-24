package com.DRCars.controller;

import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.VehiculoDTO;
import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Vehiculo;
import com.DRCars.serviceImpl.VehiculoServiceImpl;

@RestController
@RequestMapping("/catalogo")
public class CatalogoController {
	
    @Autowired
    private VehiculoServiceImpl vehiculoService;

    
    @GetMapping
    public ResponseEntity<List<VehiculoDTO>> obtenerCatalogo() {
        List<Vehiculo> vehiculos = vehiculoService.obtenerVehiculos();
        List<VehiculoDTO> vehiculosDTO = vehiculos.stream().map(VehiculoMapper.INSTANCE::toDTO).collect(Collectors.toList());
        return ResponseEntity.ok(vehiculosDTO);
    }
}
