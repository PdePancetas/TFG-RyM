package com.DRCars.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Vehiculo;

@Repository
public interface VehiculoRepository extends JpaRepository<Vehiculo, Long> {
    // Custom query methods if needed
    Vehiculo findByMatricula(String matricula);
    Vehiculo findByNumeroChasis(String numeroChasis);
}