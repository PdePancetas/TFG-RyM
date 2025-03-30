package com.DRCars.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Cliente;

@Repository
public interface ClienteRepository extends JpaRepository<Cliente, Long> {
    // Custom query methods if needed
    Cliente findByDniNif(String dniNif);
    Cliente findByEmail(String email);
}