package com.DRCars.service.impl;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.DRCars.model.Cliente;
import com.DRCars.model.Reserva;
import com.DRCars.model.Vehiculo;
import com.DRCars.model.Venta;
import com.DRCars.repository.ReservaRepository;
import com.DRCars.repository.VentaRepository;
import com.DRCars.service.ReservaService;

@Service
public class ReservaServiceImpl implements ReservaService {

	@Autowired
	private ReservaRepository reservaRepository;

	@Autowired
	private VentaRepository ventaRepository;

	@Override
	@Transactional
	public Reserva crearReserva(Reserva reserva) {
		return reservaRepository.save(reserva);
	}

	@Override
	@Transactional(readOnly = true)
	public Optional<Reserva> obtenerReservaPorId(Long id) {
		return reservaRepository.findById(id);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Reserva> obtenerReservasPorCliente(Cliente cliente) {
		return reservaRepository.findByCliente(cliente);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Reserva> obtenerReservasPorVehiculo(Vehiculo vehiculo) {
		return reservaRepository.findByVehiculo(vehiculo);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Reserva> obtenerReservasEntreFechas(LocalDate inicio, LocalDate fin) {
		return reservaRepository.findByFechaReservaBetween(inicio, fin);
	}

	@Override
	@Transactional
	public Reserva actualizarReserva(Reserva reserva) {
		return reservaRepository.save(reserva);
	}

	@Override
	@Transactional
	public void eliminarReserva(Long id) {
		reservaRepository.deleteById(id);
	}

	public List<Reserva> obtenerReservas() {
		return reservaRepository.findAll();
	}

	
	@Transactional
	public void procesarReserva(Reserva r) {
		if(r.getVehiculo()!=null) {
			Venta v = new Venta();
			v.setCliente(r.getCliente());
			v.setVehiculo(r.getVehiculo());
			v.setFechaVenta(r.getFechaReserva());
			v.setPrecioVenta(r.getPrecioReserva());
			ventaRepository.save(v);
		}
		reservaRepository.delete(r);
	}
}