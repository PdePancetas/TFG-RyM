package com.DRCars.mapper;

import org.mapstruct.Mapper;
import org.mapstruct.factory.Mappers;

import com.DRCars.dto.ReservaDTO;
import com.DRCars.model.Reserva;

@Mapper(componentModel = "spring")
public interface ReservaMapper {

	ReservaMapper INSTANCE = Mappers.getMapper(ReservaMapper.class);

	ReservaDTO toDTO(Reserva reserva);
    
	Reserva toEntity(ReservaDTO reservaDto);
}
