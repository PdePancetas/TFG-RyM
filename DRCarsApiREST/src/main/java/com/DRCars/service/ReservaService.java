package com.DRCars.service;

import java.math.BigDecimal;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.DRCars.dto.ReservaRequest;
import com.DRCars.model.Cliente;
import com.DRCars.model.Reserva;
import com.DRCars.repository.ClienteRepository;
import com.DRCars.repository.ReservaRepository;
import com.DRCars.repository.VehiculoRepository;

import jakarta.transaction.Transactional;

@Service
public class ReservaService {

	@Autowired
	private ClienteRepository clienteRepo;

	@Autowired
	private ReservaRepository reservaRepo;

	@Autowired
	private VehiculoRepository vehiculoRepo;

	@Transactional
	public void crearReserva(ReservaRequest reservaRequest) {

		Cliente cliente = clienteRepo.findByDniNif(reservaRequest.getDni()).orElseGet(() -> {
			Cliente nuevoCliente = new Cliente();
			nuevoCliente.setDniCliente(reservaRequest.getDni());
			nuevoCliente.setNombre(reservaRequest.getNombre());
			nuevoCliente.setApellidos(reservaRequest.getApellidos());
			nuevoCliente.setEmail(reservaRequest.getEmail());
			return clienteRepo.save(nuevoCliente);
		});

		Reserva reserva = new Reserva();
		reserva.setCliente(cliente);

		if (reservaRequest.getIdVehiculo() == null)
			reserva.setVehiculo(null);
		else
			reserva.setVehiculo(vehiculoRepo.getReferenceById(reservaRequest.getIdVehiculo()));

		reserva.setFechaReserva(reservaRequest.getFecha());
		reserva.setPrecioReserva(BigDecimal.valueOf(reservaRequest.getPrecio()));
		reserva.setDescripcion(reservaRequest.getDescripcion());

		reservaRepo.save(reserva);
	}
}
