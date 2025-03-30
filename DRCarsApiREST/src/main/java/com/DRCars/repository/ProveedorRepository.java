package com.DRCars.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Proveedor;

@Repository
public interface ProveedorRepository extends JpaRepository<Proveedor, Long> {
    // Custom query methods if needed
    Proveedor findByCifNif(String cifNif);
    Proveedor findByNombre(String nombre);
}
