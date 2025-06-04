package com.DRCars.repository;

import com.DRCars.model.Usuario;

import java.util.Optional;

import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface UsuarioRepository extends JpaRepository<Usuario, String> {
    // Custom query methods if needed
    Optional<Usuario> findByUsuario(String usuario);

}