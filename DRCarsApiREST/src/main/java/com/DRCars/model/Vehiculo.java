package com.DRCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Objects;
import java.util.Set;

import com.fasterxml.jackson.annotation.JsonBackReference;
import com.fasterxml.jackson.annotation.JsonIgnore;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.EnumType;
import jakarta.persistence.Enumerated;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.JoinColumn;
import jakarta.persistence.ManyToOne;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

@Entity
@Table(name = "VEHICULOS")
public class Vehiculo implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 7121060819502167123L;

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id_vehiculo")
	private Long idVehiculo;

	@Column(name = "marca", nullable = false)
	private String marca;

	@Column(name = "modelo", nullable = false)
	private String modelo;

	@Column(name = "anno_fabricacion")
	private Integer annoFabricacion;

	@Column(name = "color")
	private String color;

	@Column(name = "kilometraje")
	private Integer kilometraje;

	@Column(name = "matricula")
	private String matricula;

	@Column(name = "numero_chasis")
	private String numeroChasis;

	@Column(name = "precio_compra")
	private BigDecimal precioCompra;

	@Enumerated(EnumType.STRING)
	@Column(name = "estado", nullable = false)
	private Estado estado = Estado.GARAJE;

	@ManyToOne
	@JoinColumn(name = "id_proveedor")
	private Proveedor proveedor;

	@OneToMany(mappedBy = "vehiculo")
	private Set<PiezaVehiculo> piezasVehiculos;

	@OneToMany(mappedBy = "vehiculo")
	private Set<Venta> ventas;

	@OneToMany(mappedBy = "vehiculo")
	private Set<Reserva> reservas;

	public enum Estado {
		GARAJE, VENTA, VENDIDO
	}

	public Vehiculo() {
		super();
	}

	public Long getIdVehiculo() {
		return idVehiculo;
	}

	public void setIdVehiculo(Long idVehiculo) {
		this.idVehiculo = idVehiculo;
	}

	public String getMarca() {
		return marca;
	}

	public void setMarca(String marca) {
		this.marca = marca;
	}

	public String getModelo() {
		return modelo;
	}

	public void setModelo(String modelo) {
		this.modelo = modelo;
	}

	public Integer getAnnoFabricacion() {
		return annoFabricacion;
	}

	public void setAnnoFabricacion(Integer annoFabricacion) {
		this.annoFabricacion = annoFabricacion;
	}

	public String getColor() {
		return color;
	}

	public void setColor(String color) {
		this.color = color;
	}

	public Integer getKilometraje() {
		return kilometraje;
	}

	public void setKilometraje(Integer kilometraje) {
		this.kilometraje = kilometraje;
	}

	public String getMatricula() {
		return matricula;
	}

	public void setMatricula(String matricula) {
		this.matricula = matricula;
	}

	public String getNumeroChasis() {
		return numeroChasis;
	}

	public void setNumeroChasis(String numeroChasis) {
		this.numeroChasis = numeroChasis;
	}

	public BigDecimal getPrecioCompra() {
		return precioCompra;
	}

	public void setPrecioCompra(BigDecimal precioCompra) {
		this.precioCompra = precioCompra;
	}

	public Estado getEstado() {
		return estado;
	}

	public void setEstado(Estado estado) {
		this.estado = estado;
	}

	public Proveedor getProveedor() {
		return proveedor;
	}

	public void setProveedor(Proveedor proveedor) {
		this.proveedor = proveedor;
	}

	public Set<PiezaVehiculo> getPiezasVehiculos() {
		return piezasVehiculos;
	}

	public void setPiezasVehiculos(Set<PiezaVehiculo> piezasVehiculos) {
		this.piezasVehiculos = piezasVehiculos;
	}

	public Set<Venta> getVentas() {
		return ventas;
	}

	public void setVentas(Set<Venta> ventas) {
		this.ventas = ventas;
	}

	public Set<Reserva> getReservas() {
		return reservas;
	}

	public void setReservas(Set<Reserva> reservas) {
		this.reservas = reservas;
	}

	@Override
	public int hashCode() {
		return Objects.hash(idVehiculo);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Vehiculo other = (Vehiculo) obj;
		return Objects.equals(idVehiculo, other.idVehiculo);
	}

}
