package com.DRCars.repository;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Cliente;
import com.DRCars.model.Solicitud;
import com.DRCars.model.Vehiculo;

@Repository
public interface SolicitudRepository extends JpaRepository<Solicitud, Long> {
    // Custom query methods if needed
    List<Solicitud> findByCliente(Cliente cliente);
    List<Solicitud> findByVehiculo(Vehiculo vehiculo);
    List<Solicitud> findByFechaSolicitudBetween(LocalDate startDate, LocalDate endDate);
}