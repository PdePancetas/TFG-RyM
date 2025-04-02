package com.DRCars.serviceImpl;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.DRCars.model.Vehiculo;
import com.DRCars.model.Vehiculo.Estado;
import com.DRCars.repository.VehiculoRepository;
import com.DRCars.service.VehiculoService;

@Service
public class VehiculoServiceImpl implements VehiculoService {
	@Autowired
	private VehiculoRepository vehiculoRepository;

	@Override
	@Transactional
	public Vehiculo crearVehiculo(Vehiculo vehiculo) {
		return vehiculoRepository.save(vehiculo);
	}

	@Override
	@Transactional(readOnly = true)
	public Optional<Vehiculo> obtenerVehiculoPorId(Long id) {
		return vehiculoRepository.findById(id);
	}

	@Override
	@Transactional(readOnly = true)
	public Vehiculo obtenerVehiculoPorMatricula(String matricula) {
		return vehiculoRepository.findByMatricula(matricula);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Vehiculo> obtenerVehiculosPorEstado(Estado estado) {
		return vehiculoRepository.findAll().stream().filter(v -> v.getEstado() == estado).collect(Collectors.toList());
	}

	@Override
	@Transactional
	public Vehiculo actualizarVehiculo(Vehiculo vehiculo) {
		return vehiculoRepository.save(vehiculo);
	}

	@Override
	@Transactional
	public void eliminarVehiculo(Long id) {
		vehiculoRepository.deleteById(id);
	}

	@Override
	public List<Vehiculo> obtenerVehiculos() {
		return vehiculoRepository.findAll();
	}
}