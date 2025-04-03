package com.DRCars.dto;

public class LoginRequest {
	
	private String usuario;
	private String contraseña;
	private String ultimo_acceso;

	public LoginRequest() {
	}

	public LoginRequest(String usuario, String contraseña, String ultimo_acceso) {
		this.usuario = usuario;
		this.contraseña = contraseña;
		this.ultimo_acceso = ultimo_acceso;
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

	public String getUltimo_acceso() {
		return ultimo_acceso;
	}

	public void setUltimo_acceso(String ultimo_acceso) {
		this.ultimo_acceso = ultimo_acceso;
	}
}
