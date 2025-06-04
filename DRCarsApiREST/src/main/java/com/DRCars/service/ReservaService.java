package com.DRCars.service;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import com.DRCars.model.Cliente;
import com.DRCars.model.Reserva;
import com.DRCars.model.Vehiculo;

public interface ReservaService {
	Reserva crearReserva(Reserva reserva);

	Optional<Reserva> obtenerReservaPorId(Long id);

	List<Reserva> obtenerReservasPorCliente(Cliente cliente);

	List<Reserva> obtenerReservasPorVehiculo(Vehiculo vehiculo);

	List<Reserva> obtenerReservasEntreFechas(LocalDate inicio, LocalDate fin);

	Reserva actualizarReserva(Reserva reserva);

	void eliminarReserva(Long id);
}
