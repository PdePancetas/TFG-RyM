package com.DRCars.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Trabajador;

@Repository
public interface TrabajadorRepository extends JpaRepository<Trabajador, Long> {
    // Custom query methods if needed
    Trabajador findByDniNif(String dniNif);
    Trabajador findByEmail(String email);
}
