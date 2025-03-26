package com.RDCars.model;

import java.io.Serializable;
import java.util.Set;

import com.RDCars.model.Proveedor.TipoProveedor;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

//Proveedor Entity
@Entity
@Table(name = "PROVEEDORES")
public class Proveedor implements Serializable {
	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id_proveedor")
	private Long idProveedor;

	@Enumerated(EnumType.STRING)
	@Column(name = "tipo_proveedor", nullable = false)
	private TipoProveedor tipoProveedor;

	@Column(name = "nombre", nullable = false)
	private String nombre;

	@Column(name = "cif_nif", nullable = false)
	private String cifNif;

	@Column(name = "telefono")
	private String telefono;

	@Column(name = "email")
	private String email;

	@Column(name = "direccion")
	private String direccion;

	@Column(name = "ciudad")
	private String ciudad;

	@Column(name = "codigo_postal")
	private String codigoPostal;

	@OneToMany(mappedBy = "proveedor")
	private Set<Vehiculo> vehiculos;

	// Enum for Proveedor type
	public enum TipoProveedor {
		EMPRESA, PARTICULAR
	}

	// Getters and setters
}