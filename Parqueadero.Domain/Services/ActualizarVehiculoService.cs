using Parqueadero.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Domain.Services
{
    [DomainService]
    public class ActualizarVehiculoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ObtenerVehiculoService _service;
        private readonly IGenericRepository<Entities.Parqueadero> _genericRepoParqueadero;
        public ActualizarVehiculoService(IUnitOfWork unitOfWork, ObtenerVehiculoService service, IGenericRepository<Entities.Parqueadero> genericRepoParqueadero)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _genericRepoParqueadero = genericRepoParqueadero;
        }

        public async Task<bool> ActualizarVehiculo(string Placa)
        {
            Domain.Entities.Parqueadero vehiculo = await _service.ObtenerVehiculo(Placa);
            vehiculo.Estado = Enumerations.EstadoVehiculo.Inactivo;
            _genericRepoParqueadero.UpdateAsync(vehiculo);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
