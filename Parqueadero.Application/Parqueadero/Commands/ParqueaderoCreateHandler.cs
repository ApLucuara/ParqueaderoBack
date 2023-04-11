using AutoMapper;
using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Services;

namespace Parqueadero.Application.Parqueadero.Commands
{
    internal class ParqueaderoCreateHandler : IRequestHandler<ParqueaderoCreateCommand, ParqueaderoDto>
    {
        private readonly GuardarVehiculoService _service;
        private readonly IMapper _mapper;

        public ParqueaderoCreateHandler(GuardarVehiculoService service, IMapper mapper)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mapper = mapper;
        }

        public async Task<ParqueaderoDto> Handle(ParqueaderoCreateCommand request, CancellationToken cancellationToken)
        {
            var parqueadero = await _service.RecordParqueaderoAsync(
           new Domain.Entities.Parqueadero(request.TipoVehiculo, request.Placa, request.Cilindraje, DateTime.Now), cancellationToken
       );
            var parqueaderoDto = _mapper.Map<ParqueaderoDto>(parqueadero);
            return parqueaderoDto;
        }

    }
}
