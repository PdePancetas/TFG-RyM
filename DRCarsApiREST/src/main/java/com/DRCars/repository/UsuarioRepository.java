package com.DRCars.repository;

import com.DRCars.model.Usuario;

import java.time.LocalDateTime;
import java.util.Optional;

import org.hibernate.annotations.SQLUpdate;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;
import org.springframework.stereotype.Repository;
import org.springframework.transaction.annotation.Transactional;

@Repository
public interface UsuarioRepository extends JpaRepository<Usuario, Long> {
    // Custom query methods if needed
    Optional<Usuario> findByUsuario(String usuario);

    @Modifying
    @Transactional
    @Query("UPDATE Usuario u SET u.ultimo_acceso = :ultimo_acceso WHERE u.id = :id")
    void actualizarUltimoAcceso(String id, LocalDateTime ultimo_acceso);
    
}