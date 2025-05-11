package com.DRCars.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.dto.ReservaRequest;
import com.DRCars.service.ReservaService;

@RestController
@RequestMapping("/crearReserva")
public class ReservaController {

    @Autowired
    private ReservaService reservaService;

    @PostMapping
    public ResponseEntity<String> solicitarReserva(@RequestBody ReservaRequest reserva) {
        try {
            reservaService.crearReserva(reserva);
            return ResponseEntity.ok("Reserva realizada con éxito");
        } catch (Exception e) {
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body("Error al procesar la reserva");
        }
    }
}
