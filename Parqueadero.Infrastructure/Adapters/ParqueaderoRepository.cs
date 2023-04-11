using Parqueadero.Domain.Ports;
using Parqueadero.Infrastructure.Ports;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.Adapters
{
    [Repository]
    public class ParqueaderoRepository : IParqueaderoRepository
    {
        private readonly IGenericRepository<Domain.Entities.Parqueadero> _repositoryParqueadero;

        public ParqueaderoRepository(IGenericRepository<Domain.Entities.Parqueadero> repositoryParqueadero)
        {
            _repositoryParqueadero = repositoryParqueadero;
        }
        public async Task<Domain.Entities.Parqueadero> SaveParqueadero(Domain.Entities.Parqueadero vehiculo) => await _repositoryParqueadero.AddAsync(vehiculo);
    }
}
