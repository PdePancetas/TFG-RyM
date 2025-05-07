package com.DRCars.service;

import java.time.LocalDateTime;
import java.util.Optional;

import com.DRCars.model.Usuario;

public interface UsuarioService {
	Usuario crearUsuario(Usuario usuario);

	Optional<Usuario> obtenerUsuarioPorId(Long id);

	Optional<Usuario> obtenerUsuarioPorNombre(String usuario);

	void eliminarUsuario(Long id);
	
	boolean verificarContraseña(String hashIngresado, String hashGuardado);
	
	void actualizarUltimoAcceso(String id, LocalDateTime ultimoAcceso);
}