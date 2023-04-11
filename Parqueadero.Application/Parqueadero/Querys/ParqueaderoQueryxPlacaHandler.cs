using AutoMapper;
using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Ports;
using Parqueadero.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Application.Parqueadero.Querys
{
    public class ParqueaderoQueryxPlacaHandler : IRequestHandler<ParqueaderoQueryxPlaca, ParqueaderoDto>
    {
        private readonly IMapper _mapper;
        private readonly ObtenerVehiculoService _service;
        public ParqueaderoQueryxPlacaHandler(IMapper mapper, ObtenerVehiculoService service)
        {
            _mapper = mapper;
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public async Task<ParqueaderoDto> Handle(ParqueaderoQueryxPlaca request, CancellationToken cancellationToken)
        {
            var vehiculo = await _service.ObtenerVehiculo(request.Placa);
            var vehiculoDto = _mapper.Map<ParqueaderoDto>(vehiculo);
            return vehiculoDto;
        }
    }
}
