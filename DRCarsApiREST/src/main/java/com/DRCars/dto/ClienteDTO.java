package com.DRCars.dto;

public class ClienteDTO {
	
    private String dniCliente;
    private String nombre;
    private String apellidos;
    private String usuario;

    public ClienteDTO() {
    	super();
    }

    public ClienteDTO(String dniCliente, String nombre, String apellidos, String usuario) {
        this.dniCliente = dniCliente;
        this.nombre = nombre;
        this.apellidos = apellidos;
        this.usuario = usuario;
    }

    public String getDniCliente() {
        return dniCliente;
    }

    public void setDniCliente(String dniCliente) {
        this.dniCliente = dniCliente;
    }

    public String getNombre() {
        return nombre;
    }

    public void setNombre(String nombre) {
        this.nombre = nombre;
    }

    public String getApellidos() {
        return apellidos;
    }

    public void setApellidos(String apellidos) {
        this.apellidos = apellidos;
    }

    public String getUsuario() {
        return usuario;
    }

    public void setUsuario(String usuario) {
        this.usuario = usuario;
    }
}
