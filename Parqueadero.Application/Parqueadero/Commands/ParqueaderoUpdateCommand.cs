using MediatR;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Application.Parqueadero.Commands;


public record ParqueaderoUpdateCommand(string Placa) : IRequest<bool>;


