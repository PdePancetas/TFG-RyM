package com.DRCars.service;

import java.util.List;
import java.util.Optional;

import org.springframework.http.ResponseEntity;

import com.DRCars.model.Usuario;

public interface UsuarioService {
	Usuario crearUsuario(Usuario usuario);

	Optional<Usuario> obtenerUsuarioPorId(Long id);

	Optional<Usuario> obtenerUsuarioPorNombre(String usuario);

	void eliminarUsuario(Usuario usuario);
	
	boolean verificarContraseña(String hashIngresado, String hashGuardado);

	List<Usuario> obtenerUsuarios();

	Usuario actualizarUsuario(Usuario u);
	
}