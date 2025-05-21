package com.DRCars.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.model.Usuario;
import com.DRCars.service.UsuarioService;

@RestController
@RequestMapping("/users")
public class UserController {

	@Autowired
	UsuarioService userService;
	
	@GetMapping
	public ResponseEntity<List<Usuario>> obtenerUsuarios() {
		return ResponseEntity.ok(userService.obtenerUsuarios());
	}
	
}
