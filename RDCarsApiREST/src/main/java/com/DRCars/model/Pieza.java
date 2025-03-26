package com.DRCars.model;

import java.io.Serializable;
import java.math.BigDecimal;
import java.util.Objects;
import java.util.Set;

import jakarta.persistence.Column;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.OneToMany;
import jakarta.persistence.Table;

@Entity
@Table(name = "PIEZAS")
public class Pieza implements Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = -6253761275860626321L;

	@Id
	@GeneratedValue(strategy = GenerationType.IDENTITY)
	@Column(name = "id_pieza")
	private Long idPieza;

	@Column(name = "nombre", nullable = false)
	private String nombre;

	@Column(name = "descripcion")
	private String descripcion;

	@Column(name = "precio")
	private BigDecimal precio;

	@Column(name = "stock")
	private Integer stock;

	@OneToMany(mappedBy = "pieza")
	private Set<PiezaVehiculo> piezasVehiculos;

	public Pieza() {
		super();
	}

	public Long getIdPieza() {
		return idPieza;
	}

	public void setIdPieza(Long idPieza) {
		this.idPieza = idPieza;
	}

	public String getNombre() {
		return nombre;
	}

	public void setNombre(String nombre) {
		this.nombre = nombre;
	}

	public String getDescripcion() {
		return descripcion;
	}

	public void setDescripcion(String descripcion) {
		this.descripcion = descripcion;
	}

	public BigDecimal getPrecio() {
		return precio;
	}

	public void setPrecio(BigDecimal precio) {
		this.precio = precio;
	}

	public Integer getStock() {
		return stock;
	}

	public void setStock(Integer stock) {
		this.stock = stock;
	}

	public Set<PiezaVehiculo> getPiezasVehiculos() {
		return piezasVehiculos;
	}

	public void setPiezasVehiculos(Set<PiezaVehiculo> piezasVehiculos) {
		this.piezasVehiculos = piezasVehiculos;
	}

	@Override
	public int hashCode() {
		return Objects.hash(idPieza);
	}

	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Pieza other = (Pieza) obj;
		return Objects.equals(idPieza, other.idPieza);
	}

}
