package com.DRCars.serviceImpl;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.DRCars.model.Cliente;
import com.DRCars.model.Vehiculo;
import com.DRCars.model.Venta;
import com.DRCars.repository.VentaRepository;
import com.DRCars.service.VentaService;

@Service
public class VentaServiceImpl implements VentaService {
	@Autowired
	private VentaRepository ventaRepository;

	@Override
	@Transactional
	public Venta crearVenta(Venta venta) {
		return ventaRepository.save(venta);
	}

	@Override
	@Transactional(readOnly = true)
	public Optional<Venta> obtenerVentaPorId(Long id) {
		return ventaRepository.findById(id);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Venta> obtenerVentasPorCliente(Cliente cliente) {
		return ventaRepository.findByCliente(cliente);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Venta> obtenerVentasPorVehiculo(Vehiculo vehiculo) {
		return ventaRepository.findByVehiculo(vehiculo);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Venta> obtenerVentasEntreFechas(LocalDate inicio, LocalDate fin) {
		return ventaRepository.findByFechaVentaBetween(inicio, fin);
	}

	@Override
	@Transactional
	public Venta actualizarVenta(Venta venta) {
		return ventaRepository.save(venta);
	}

	@Override
	@Transactional
	public void eliminarVenta(Long id) {
		ventaRepository.deleteById(id);
	}
}