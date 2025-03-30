package com.DRCars.service;

import java.util.List;
import java.util.Optional;

import com.DRCars.model.Usuario;

public interface UsuarioService {
	Usuario crearUsuario(Usuario usuario);

	Optional<Usuario> obtenerUsuarioPorId(Long id);

	List<Usuario> obtenerUsuarioPorNombre(String usuario);

	void eliminarUsuario(Long id);
}