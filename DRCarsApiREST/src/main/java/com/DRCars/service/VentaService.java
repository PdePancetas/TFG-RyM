package com.DRCars.service;

import java.time.LocalDate;
import java.util.List;
import java.util.Optional;

import com.DRCars.model.Cliente;
import com.DRCars.model.Vehiculo;
import com.DRCars.model.Venta;

public interface VentaService {
	Venta crearVenta(Venta venta);

	Optional<Venta> obtenerVentaPorId(Long id);

	List<Venta> obtenerVentasPorCliente(Cliente cliente);

	List<Venta> obtenerVentasPorVehiculo(Vehiculo vehiculo);

	List<Venta> obtenerVentasEntreFechas(LocalDate inicio, LocalDate fin);

	Venta actualizarVenta(Venta venta);

	void eliminarVenta(Long id);
}
