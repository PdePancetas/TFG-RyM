package com.DRCars.model;

import java.io.Serializable;
import java.util.Objects;

import org.springframework.data.annotation.Transient;

import com.fasterxml.jackson.annotation.JsonIgnore;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;
import jakarta.persistence.Version;

@Entity
@Table(name = "USUARIOS")
public class Usuario implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2626037672387601521L;
	
	@Id
	@Column(name = "usuario", nullable = false)
	private String usuario;

	@Column(name = "contraseña", nullable = false)
	private String contraseña;

	@Enumerated(EnumType.STRING)
	@Column(name = "tipo_usuario", nullable = false)
	private TipoUsuario tipoUsuario;

	@Column(name = "ultimo_acceso", nullable = false)
	private String ultimo_acceso;
	
	@Column(name = "registro_cuenta", nullable = false)
	private String registro_cuenta;

	public enum TipoUsuario {
		ADMIN,MANAGER,SALESAGENT,VIEWER,USER
	}

	public Usuario() {
		super();
	}

	
	public String getUsuario() {
		return usuario;
	}

	public void setUsuario(String usuario) {
		this.usuario = usuario;
	}

	public String getContraseña() {
		return contraseña;
	}

	public void setContraseña(String contraseña) {
		this.contraseña = contraseña;
	}

	public TipoUsuario getTipoUsuario() {
		return tipoUsuario;
	}

	public void setTipoUsuario(TipoUsuario tipoUsuario) {
		this.tipoUsuario = tipoUsuario;
	}

	public String getUltimo_acceso() {
		return ultimo_acceso;
	}

	public void setUltimo_acceso(String ultimo_acceso) {
		this.ultimo_acceso = ultimo_acceso;
	}

	public String getRegistro_cuenta() {
		return registro_cuenta;
	}

	public void setRegistro_cuenta(String registro_cuenta) {
		this.registro_cuenta = registro_cuenta;
	}

	@Override
	public int hashCode() {
		return Objects.hash(usuario);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Usuario other = (Usuario) obj;
		return Objects.equals(usuario, other.usuario);
	}

}
