package com.DRCars.controller;

import java.time.LocalDateTime;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.DRCars.mapper.VehiculoMapper;
import com.DRCars.model.Usuario;
import com.DRCars.model.Vehiculo;
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

	@PostMapping("/crear")
	public ResponseEntity<Usuario> addUsuario(@RequestBody Usuario usuario) {
		Usuario u = null;
		try {
			Optional<Usuario> existe = userService.obtenerUsuarioPorId(usuario.getUsuario());
			if(existe.isPresent()) {
				return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(u);
			}
			String date = LocalDateTime.now().toString();
			usuario.setRegistro_cuenta(date);
			usuario.setUltimo_acceso(date);
			u = userService.crearUsuario(usuario);
			return ResponseEntity.ok(u);
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(u);
		}
	}

	@PostMapping("/delete")
	public ResponseEntity<Usuario> deleteUsuario(@RequestBody Usuario usuario) {
		Optional<Usuario> u = null;
		try {
			u = userService.obtenerUsuarioPorId(usuario.getUsuario());
			if (u.isEmpty())
				return ResponseEntity.status(HttpStatus.NOT_FOUND).body(null);

			userService.eliminarUsuario(usuario);
			return ResponseEntity.ok(u.get());
		} catch (Exception e) {
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(null);
		}
	}

	@PostMapping("/act")
	public ResponseEntity<Usuario> updtUsuario(@RequestBody Usuario u) {
		Optional<Usuario> usuario = null;
		try {
			usuario = userService.obtenerUsuarioPorId(u.getUsuario());
			if (usuario.isEmpty())
				return ResponseEntity.status(HttpStatus.NOT_FOUND).body(null);

			u.setUltimo_acceso(usuario.get().getUltimo_acceso());
			u.setRegistro_cuenta(usuario.get().getRegistro_cuenta());
			Usuario actualizado = userService.actualizarUsuario(u);
			return ResponseEntity.ok(actualizado);
		} catch (Exception e) {
			e.printStackTrace();
			return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).body(null);
		}

	}

}
