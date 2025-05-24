package com.DRCars.service.impl;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.DRCars.model.Cliente;
import com.DRCars.repository.ClienteRepository;
import com.DRCars.service.ClienteService;

@Service
public class ClienteServiceImpl implements ClienteService {
	@Autowired
	private ClienteRepository clienteRepository;

	@Override
	@Transactional
	public Cliente crearCliente(Cliente cliente) {
		return clienteRepository.save(cliente);
	}

	@Override
	@Transactional(readOnly = true)
	public Optional<Cliente> obtenerClientePorDni(String dniNif) {
		return clienteRepository.findByDniCliente(dniNif);
	}

	@Override
	@Transactional(readOnly = true)
	public List<Cliente> obtenerTodosClientes() {
		return clienteRepository.findAll();
	}

	@Override
	@Transactional
	public Cliente actualizarCliente(Cliente cliente) {
		return clienteRepository.save(cliente);
	}

	@Override
	@Transactional
	public void eliminarCliente(String dni) {
		clienteRepository.deleteById(dni);
	}
}
