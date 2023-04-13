using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Ports;
using Parqueadero.Infrastructure.Ports;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.Adapters
{
    [Repository]
    public class ParametrizacionRepository : IParametrizacionRepository
    {
        private readonly IGenericRepository<Parametrizacion> _parametrizacionRepository;

        public ParametrizacionRepository(IGenericRepository<Parametrizacion> parametrizacionRepository)
        {
            _parametrizacionRepository = parametrizacionRepository;
        }
    }
}
