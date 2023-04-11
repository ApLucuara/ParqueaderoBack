using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace Parqueadero.Application.Parqueadero.Commands;

public record ParqueaderoCreateCommand(
   [Required]TipoVehiculo TipoVehiculo, [Required] string Placa, short? Cilindraje
    ) : IRequest<ParqueaderoDto>;

