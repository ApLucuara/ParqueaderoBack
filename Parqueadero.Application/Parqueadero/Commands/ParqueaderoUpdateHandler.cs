using AutoMapper;
using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Application.Parqueadero.Commands
{
    internal class ParqueaderoUpdateHandler : IRequestHandler<ParqueaderoUpdateCommand, bool>
    {
        private readonly ActualizarVehiculoService _service;
        private readonly IMapper _mapper;

        public ParqueaderoUpdateHandler(ActualizarVehiculoService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper;
        }

        public async Task<bool> Handle(ParqueaderoUpdateCommand request, CancellationToken cancellationToken)
        {
           // var vehiculo = _mapper.Map<Domain.Entities.Parqueadero>(request.Placa);
            return await _service.ActualizarVehiculo(request.Placa);
        }

  
    }
}
