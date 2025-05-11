package com.DRCars.service;

import java.time.LocalDateTime;
import java.util.Optional;

import org.springframework.stereotype.Service;

import com.DRCars.model.Usuario;

@Service
public interface UsuarioService {
	Usuario crearUsuario(Usuario usuario);

	Optional<Usuario> obtenerUsuarioPorId(Long id);

	Optional<Usuario> obtenerUsuarioPorNombre(String usuario);

	void eliminarUsuario(Long id);
	
	boolean verificarContraseña(String hashIngresado, String hashGuardado);
	
}