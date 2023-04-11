using MediatR;
using Parqueadero.Domain.Dtos;

namespace Parqueadero.Application.Parqueadero.Querys;
public record ParqueaderoQuery() : IRequest<IEnumerable<ParqueaderoDto>>;

public record ParqueaderoQueryxPlaca(string Placa) : IRequest<ParqueaderoDto>;