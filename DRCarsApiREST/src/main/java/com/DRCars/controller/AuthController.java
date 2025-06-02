package com.DRCars.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import com.DRCars.dto.LoginRequest;
import com.DRCars.model.Cliente;
import com.DRCars.model.Usuario;
import com.DRCars.service.impl.ClienteServiceImpl;
import com.DRCars.service.impl.UsuarioServiceImpl;

import java.util.Optional;

@RestController
@RequestMapping("/auth")
public class AuthController {
	@Autowired
	private UsuarioServiceImpl usuarioService;

	@Autowired
	private ClienteServiceImpl clientService;

	@PostMapping("/registro")
	public ResponseEntity<String> registrarUsuario(@RequestBody Usuario usuario) {
		usuarioService.crearUsuario(usuario);
		return ResponseEntity.ok("Usuario registrado con éxito");
	}

	@PostMapping("/login")
	public ResponseEntity<String> login(@RequestBody LoginRequest request) {
		Optional<Usuario> usuario = usuarioService.obtenerUsuarioPorId(request.getUsuario());
		if (!usuario.isEmpty()) {
			Optional<Cliente> cliente = Optional.empty();
			if (usuario.isPresent()
					&& usuarioService.verificarContraseña(request.getContraseña(), usuario.get().getContraseña())) {
				usuario.get().setUltimo_acceso(request.getUltimo_acceso());
				usuarioService.crearUsuario(usuario.get());

				cliente = clientService.obtenerClientePorEmail(request.getUsuario());
				if (cliente.isPresent())
					return ResponseEntity
							.ok("Autenticación exitosa, cliente ha iniciado sesión: " + cliente.get().getDniCliente());
				else
					return ResponseEntity
							.ok("Autenticación exitosa, usuario ha iniciado sesión: " + usuario.get().getUsuario());
			}
		}
		return ResponseEntity.status(401)
				.body("Credenciales incorrectas, no ha podido iniciar sesión");
	}
}
