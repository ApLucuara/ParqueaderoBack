using AutoMapper;
using Parqueadero.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Application.Parqueadero
{
    public class ParqueaderoProfile : Profile
    {
        public ParqueaderoProfile()
        {
            CreateMap<Domain.Entities.Parqueadero, ParqueaderoDto>();
            CreateMap<ParqueaderoDto, Domain.Entities.Parqueadero>();

        }
    }
}
