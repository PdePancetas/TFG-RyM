package com.DRCars.service;

import com.DRCars.model.Cliente;

import java.util.List;
import java.util.Optional;

public interface ClienteService {
	Cliente crearCliente(Cliente cliente);

	Optional<Cliente> obtenerClientePorDni(String dniNif);

	List<Cliente> obtenerTodosClientes();

	Cliente actualizarCliente(Cliente cliente);

	void eliminarCliente(String dni);
}