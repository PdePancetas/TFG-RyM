package com.DRCars.service;

import java.util.List;
import java.util.Optional;

import com.DRCars.model.Usuario;

public interface UsuarioService {
	Usuario crearUsuario(Usuario usuario);

	Optional<Usuario> obtenerUsuarioPorId(String usuario);


	void eliminarUsuario(Usuario usuario);
	
	boolean verificarContraseña(String hashIngresado, String hashGuardado);

	List<Usuario> obtenerUsuarios();

	Usuario actualizarUsuario(Usuario u);
	
}