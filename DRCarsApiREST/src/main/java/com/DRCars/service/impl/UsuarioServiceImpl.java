package com.DRCars.service.impl;

import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import com.DRCars.model.Usuario;
import com.DRCars.repository.UsuarioRepository;
import com.DRCars.service.UsuarioService;

@Service
public class UsuarioServiceImpl implements UsuarioService {
	
	@Autowired
	private UsuarioRepository usuarioRepository;

	@Override
	@Transactional
	public Usuario crearUsuario(Usuario usuario) {
		return usuarioRepository.save(usuario);
	}

	@Override
	@Transactional(readOnly = true)
	public Optional<Usuario> obtenerUsuarioPorId(String string) {
		return usuarioRepository.findById(string);
	}

	@Override
	@Transactional
	public void eliminarUsuario(Usuario usuario) {
		usuarioRepository.deleteById(usuario.getUsuario());
	}

	@Override
	public boolean verificarContraseña(String hashIngresado, String hashGuardado) {
		return hashIngresado.equals(hashGuardado);
	}

	@Override
	public List<Usuario> obtenerUsuarios() {
		return usuarioRepository.findAll();
	}

	@Override
	public Usuario actualizarUsuario(Usuario u) {
		return usuarioRepository.save(u);
	}

}