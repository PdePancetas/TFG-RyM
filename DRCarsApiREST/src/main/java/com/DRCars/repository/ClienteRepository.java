package com.DRCars.repository;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Cliente;
import com.DRCars.model.Usuario;

@Repository
public interface ClienteRepository extends JpaRepository<Cliente, String> {

	Optional<Cliente> findByDniCliente(String dniCliente);

	Cliente findByUsuario(Usuario usuario);

}