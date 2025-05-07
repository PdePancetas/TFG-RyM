package com.DRCars.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import com.DRCars.dto.LoginRequest;
import com.DRCars.model.Usuario;
import com.DRCars.serviceImpl.UsuarioServiceImpl;

import java.util.Optional;

@RestController
@RequestMapping("/auth")
public class AuthController {
    @Autowired
    private UsuarioServiceImpl usuarioService;

    
    @PostMapping("/registro")
    public ResponseEntity<String> registrarUsuario(@RequestBody Usuario usuario) {
        usuarioService.crearUsuario(usuario);
        return ResponseEntity.ok("Usuario registrado con éxito");
    }


    @PostMapping("/login")
    public ResponseEntity<String> login(@RequestBody LoginRequest request) {
        Optional<Usuario> usuario = usuarioService.obtenerUsuarioPorNombre(request.getUsuario());
        if (usuario.isPresent() && usuarioService.verificarContraseña(request.getContraseña(), usuario.get().getContraseña())) {
            usuario.get().setUltimo_acceso(request.getUltimo_acceso());
            usuarioService.actualizarUltimoAcceso(usuario.get().getIdUsuario().toString(), usuario.get().getUltimo_acceso());
        	return ResponseEntity.ok("Autenticación exitosa");
        }
        return ResponseEntity.status(401).body("Credenciales incorrectas");
    }
}

