package com.DRCars.service;

import java.util.List;
import java.util.Optional;

import com.DRCars.model.Vehiculo;
import com.DRCars.model.Vehiculo.Estado;

public interface VehiculoService {
	
	List<Vehiculo> obtenerVehiculos();
	
	Vehiculo crearVehiculo(Vehiculo vehiculo);

	Optional<Vehiculo> obtenerVehiculoPorId(Long id);

	Vehiculo obtenerVehiculoPorMatricula(String matricula);

	List<Vehiculo> obtenerVehiculosPorEstado(Estado estado);

	Vehiculo actualizarVehiculo(Vehiculo vehiculo);

	void eliminarVehiculo(Long id);
}
