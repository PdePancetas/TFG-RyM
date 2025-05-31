package com.DRCars.service.impl;

import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.DRCars.dto.ProcReservaRequest;
import com.DRCars.dto.ReservaRequest;
import com.DRCars.model.Cliente;
import com.DRCars.model.Reserva;
import com.DRCars.model.Venta;
import com.DRCars.repository.ClienteRepository;
import com.DRCars.repository.ReservaRepository;
import com.DRCars.repository.VehiculoRepository;
import com.DRCars.repository.VentaRepository;
import com.DRCars.service.ReservaService;

import jakarta.transaction.Transactional;

@Service
public class ReservaServiceImpl implements ReservaService {

	@Autowired
	private ClienteRepository clienteRepo;

	@Autowired
	private ReservaRepository reservaRepo;

	@Autowired
	private VehiculoRepository vehiculoRepo;

	@Autowired
	private VentaRepository ventasRepo;

	@Transactional
	public void crearReserva(ReservaRequest reservaRequest) {

		Cliente cliente = clienteRepo.findByDniCliente(reservaRequest.getDni()).orElseGet(() -> {
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
		if (reservaRequest.getPrecio() != null)
			reserva.setPrecioReserva(BigDecimal.valueOf(reservaRequest.getPrecio()));
		else
			reserva.setPrecioReserva(BigDecimal.ZERO);
		reserva.setDescripcion(reservaRequest.getDescripcion());

		reservaRepo.save(reserva);
	}

	public List<Reserva> obtenerReservas() {
		return reservaRepo.findAll();
	}

	@Transactional
	public void procesarReserva(ProcReservaRequest reserva) {
		Optional<Reserva> res = null;
		try {
			res = reservaRepo.findById(reserva.getIdReserva());
			if(res.isPresent()) {
				if (reserva.isAceptada()) {
					if (res.get().getVehiculo().getIdVehiculo() != null) {
						Venta venta = new Venta();
						venta.setCliente(clienteRepo.getReferenceById(res.get().getCliente().getDniCliente()));
						venta.setPrecioVenta(res.get().getPrecioReserva());
						venta.setVehiculo(vehiculoRepo.getReferenceById(res.get().getVehiculo().getIdVehiculo()));
	
						ventasRepo.save(venta);
					}
				}
				reservaRepo.delete(res.get());
			} else
				throw new Exception();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
