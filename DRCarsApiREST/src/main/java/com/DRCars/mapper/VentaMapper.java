package com.DRCars.mapper;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;

import com.DRCars.dto.VentaDTO;
import com.DRCars.model.Venta;

@Mapper(componentModel = "spring")
public interface VentaMapper {

	VentaMapper INSTANCE = Mappers.getMapper(VentaMapper.class);

    VentaDTO toDTO(Venta venta);
    
    Venta toEntity(VentaDTO ventaDto);
}
