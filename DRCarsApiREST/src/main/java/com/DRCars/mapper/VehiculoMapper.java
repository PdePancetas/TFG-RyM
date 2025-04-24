package com.DRCars.mapper;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;

import com.DRCars.dto.ProveedorDTO;
import com.DRCars.dto.VehiculoDTO;
import com.DRCars.model.Proveedor;
import com.DRCars.model.Vehiculo;

@Mapper
public interface VehiculoMapper {
	
    VehiculoMapper INSTANCE = Mappers.getMapper(VehiculoMapper.class);

    VehiculoDTO toDTO(Vehiculo vehiculo);

    ProveedorDTO toDTO(Proveedor proveedor);

    // Tambien se puede mapeo inverso
    // Vehiculo toEntity(VehiculoDTO vehiculoDTO);
}
