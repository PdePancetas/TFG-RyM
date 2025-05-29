package com.DRCars.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.model.Usuario;
import com.DRCars.service.impl.UsuarioServiceImpl;

@RestController
@RequestMapping("/users")
public class UserController {

	@Autowired
	UsuarioServiceImpl userService;
	
	@GetMapping
	public ResponseEntity<List<Usuario>> obtenerUsuarios() {
		return ResponseEntity.ok(userService.obtenerUsuarios());
	}
	
	@PostMapping("/act")
	public ResponseEntity<Usuario> updtUsuario(@RequestBody Usuario u) {
		Usuario usuario = null;
		try {
			usuario = userService.actualizarUsuario(u);
			return ResponseEntity.ok(usuario);
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(usuario);
		}
		
	}
	
}
