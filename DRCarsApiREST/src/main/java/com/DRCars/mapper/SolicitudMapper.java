package com.DRCars.mapper;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;

import com.DRCars.dto.SolicitudDTO;
import com.DRCars.model.Solicitud;

@Mapper(componentModel = "spring")
public interface SolicitudMapper {
    
	SolicitudMapper INSTANCE = Mappers.getMapper(SolicitudMapper.class);

	SolicitudDTO toDTO(Solicitud solicitud);
}