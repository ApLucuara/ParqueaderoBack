using Parqueadero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Domain.Ports
{
    public interface IParqueaderoRepository
    {
        Task<Domain.Entities.Parqueadero> SaveParqueadero(Domain.Entities.Parqueadero vehiculo);
    }
}
