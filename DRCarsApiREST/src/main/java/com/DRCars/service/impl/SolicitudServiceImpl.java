package com.DRCars.service.impl;

import java.math.BigDecimal;
import java.util.List;
import java.util.Optional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import com.DRCars.dto.ProcReservaRequest;
import com.DRCars.dto.SolicitudRequest;
import com.DRCars.model.Cliente;
import com.DRCars.model.Solicitud;
import com.DRCars.model.Usuario;
import com.DRCars.model.Venta;
import com.DRCars.repository.ClienteRepository;
import com.DRCars.repository.SolicitudRepository;
import com.DRCars.repository.UsuarioRepository;
import com.DRCars.repository.VehiculoRepository;
import com.DRCars.repository.VentaRepository;
import com.DRCars.service.SolicitudService;

import jakarta.transaction.Transactional;

@Service
public class SolicitudServiceImpl implements SolicitudService {

	@Autowired
	private ClienteRepository clienteRepo;

	@Autowired
	private SolicitudRepository solicitudRepo;

	@Autowired
	private VehiculoRepository vehiculoRepo;

	@Autowired
	private VentaRepository ventasRepo;
	
	@Autowired
	private UsuarioRepository userRepo;

	@Transactional
	public void crearSolicitud(SolicitudRequest solicitudRequest) {

		Cliente cliente = clienteRepo.findById(solicitudRequest.getDni()).orElseGet(() -> {
			Cliente nuevoCliente = new Cliente();
			Optional<Usuario> u = userRepo.findById(solicitudRequest.getEmail());
			nuevoCliente.setUsuario(u.get());
			nuevoCliente.setDniCliente(solicitudRequest.getDni());
			nuevoCliente.setNombre(solicitudRequest.getNombre());
			nuevoCliente.setApellidos(solicitudRequest.getApellidos());
			return clienteRepo.save(nuevoCliente);
		});

		Solicitud solicitud = new Solicitud();
		solicitud.setCliente(cliente);

		if (solicitudRequest.getIdVehiculo() == null)
			solicitud.setVehiculo(null);
		else
			solicitud.setVehiculo(vehiculoRepo.getReferenceById(solicitudRequest.getIdVehiculo()));

		solicitud.setFechaSolicitud(solicitudRequest.getFecha());
		if (solicitudRequest.getPrecio() != null)
			solicitud.setPrecioSolicitud(BigDecimal.valueOf(solicitudRequest.getPrecio()));
		else
			solicitud.setPrecioSolicitud(BigDecimal.ZERO);
		
		solicitud.setMotivo(solicitudRequest.getMotivo());
		solicitud.setDescripcion(solicitudRequest.getDescripcion());

		solicitudRepo.save(solicitud);
	}

	public List<Solicitud> obtenerSolicitudes() {
		return solicitudRepo.findAll();
	}

	//Mover a reservas
	@Transactional
	public void procesarSolicitud(ProcReservaRequest solicitud) {
		Optional<Solicitud> res = null;
		try {
			res = solicitudRepo.findById(solicitud.getIdReserva());
			if(res.isPresent()) {
				if (solicitud.isAceptada()) {
					if (res.get().getVehiculo().getIdVehiculo() != null) {
						Venta venta = new Venta();
						venta.setCliente(clienteRepo.getReferenceById(res.get().getCliente().getDniCliente()));
						venta.setPrecioVenta(res.get().getPrecioSolicitud());
						venta.setFechaVenta(solicitud.getFechaVenta());
						venta.setVehiculo(vehiculoRepo.getReferenceById(res.get().getVehiculo().getIdVehiculo()));
	
						ventasRepo.save(venta);
					}
				}
				solicitudRepo.delete(res.get());
			} else
				throw new Exception();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}
