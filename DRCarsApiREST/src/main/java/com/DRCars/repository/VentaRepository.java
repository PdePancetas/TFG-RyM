package com.DRCars.repository;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Cliente;
import com.DRCars.model.Vehiculo;
import com.DRCars.model.Venta;

@Repository
public interface VentaRepository extends JpaRepository<Venta, Long> {
    // Custom query methods if needed
    List<Venta> findByCliente(Cliente cliente);
    List<Venta> findByVehiculo(Vehiculo vehiculo);
    List<Venta> findByFechaVentaBetween(LocalDate startDate, LocalDate endDate);
}