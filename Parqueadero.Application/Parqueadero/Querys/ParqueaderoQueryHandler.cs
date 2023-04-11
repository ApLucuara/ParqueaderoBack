using AutoMapper;
using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Ports;

namespace Parqueadero.Application.Parqueadero.Querys
{
    public class ParqueaderoQueryHandler : IRequestHandler<ParqueaderoQuery, IEnumerable<ParqueaderoDto>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Entities.Parqueadero> _genericRepoParqueado;
        public ParqueaderoQueryHandler(IMapper mapper, IGenericRepository<Domain.Entities.Parqueadero> genericRepoParqueado)
        {
            _mapper = mapper;
            _genericRepoParqueado = genericRepoParqueado;
        }

        public async Task<IEnumerable<ParqueaderoDto>> Handle(ParqueaderoQuery request, CancellationToken cancellationToken)
        {
            var Lista = await _genericRepoParqueado.GetManyAsync(x => x.Estado == Domain.Enumerations.EstadoVehiculo.Activo);
            var listaMapeadaDTO = _mapper.Map<IEnumerable<ParqueaderoDto>>(Lista);
            return listaMapeadaDTO;
        }
    }
}
