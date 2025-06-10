package com.DRCars.repository;

import java.time.LocalDate;
import java.util.List;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import com.DRCars.model.Cliente;
import com.DRCars.model.Reserva;
import com.DRCars.model.Vehiculo;

@Repository
public interface ReservaRepository extends JpaRepository<Reserva, Long> {
    
	
    List<Reserva> findByCliente(Cliente cliente);
    List<Reserva> findByVehiculo(Vehiculo vehiculo);
    List<Reserva> findByFechaReservaBetween(LocalDate startDate, LocalDate endDate);
}