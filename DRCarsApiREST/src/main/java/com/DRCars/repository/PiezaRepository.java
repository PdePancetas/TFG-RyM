package com.DRCars.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Pieza;

@Repository
public interface PiezaRepository extends JpaRepository<Pieza, Long> {
    // Custom query methods if needed
    Pieza findByNombre(String nombre);
    
}