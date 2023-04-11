using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Exceptions;
using Parqueadero.Domain.Ports;
using System.Text.RegularExpressions;

namespace Parqueadero.Domain.Services
{
    [DomainService]
    public class GuardarVehiculoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParqueaderoRepository _parqueaderoRepository;
        private readonly IGenericRepository<Domain.Entities.Parqueadero> _genericRepoParqueadero;
        private readonly IGenericRepository<Parametrizacion> _genericRepoParametrizacion;


        public GuardarVehiculoService(IUnitOfWork unitOfWork, IParqueaderoRepository parqueaderoRepository, IGenericRepository<Entities.Parqueadero> genericRepoParqueadero, IGenericRepository<Parametrizacion> genericRepoParametrizacion)
        {
            _unitOfWork = unitOfWork;
            _parqueaderoRepository = parqueaderoRepository;
            _genericRepoParqueadero = genericRepoParqueadero;
            _genericRepoParametrizacion = genericRepoParametrizacion;
        }
        public async Task<Domain.Entities.Parqueadero> RecordParqueaderoAsync(Domain.Entities.Parqueadero vehiculo, CancellationToken? cancellationToken = null)
        {
            var token = cancellationToken ?? new CancellationTokenSource().Token;
            var Param = await _genericRepoParametrizacion.GetManyAsync(para => para.TipoVehiculo == vehiculo.TipoVehiculo);
            var ParamVeh = Param.FirstOrDefault();

            await ValidaEspaciosParqueadero(vehiculo.TipoVehiculo, ParamVeh.Capacidad);
            await ValidaExistePlaca(vehiculo.Placa);
            ValidaPicoyPlaca(vehiculo, ParamVeh);

            var returnParqueadero = await _parqueaderoRepository.SaveParqueadero(vehiculo);
            await _unitOfWork.SaveAsync(token);
            return returnParqueadero;
        }

        private void ValidaPicoyPlaca(Domain.Entities.Parqueadero vehiculo, Parametrizacion param)
        {
            var matches = Regex.Matches(vehiculo.Placa, @"\d+");
            if (matches.Count == 0) { throw new CoreBusinessException("Placa no valida"); }
            char UltimoDigito = matches[0].ToString().Substring(matches[0].ToString().Length - 1, 1)[0];

            var pyp = new Dictionary<DayOfWeek, string[]>
             {
                 {DayOfWeek.Monday, param.PicoPlacaLunes.Split(',')},
                 {DayOfWeek.Tuesday, param.PicoPlacaMartes.Split(',')},
                 {DayOfWeek.Wednesday, param.PicoPlacaMiercoles.Split(',')},
                 {DayOfWeek.Thursday, param.PicoPlacaJueves.Split(',')},
                 {DayOfWeek.Friday, param.PicoPlacaViernes.Split(',')}
             };

            DayOfWeek diaDeLaSemana = vehiculo.FechaIngreso.DayOfWeek;
            var ultimoDigito2 = UltimoDigito.ToString();
            if (pyp.ContainsKey(diaDeLaSemana) && pyp[diaDeLaSemana].Any(x => x == UltimoDigito.ToString()))
            {
                throw new CoreBusinessException("El vehiculo tiene pico y placa, no puede ingresar");
            }
        }
        private async Task ValidaEspaciosParqueadero(TipoVehiculo tipoVehiculo, int Capacidad)
        {
            var cantidadVeh = await _genericRepoParqueadero.GetManyAsync(x => x.TipoVehiculo == tipoVehiculo && x.Estado == EstadoVehiculo.Activo);
            if (cantidadVeh.Count() == Capacidad)
            {
                throw new CoreBusinessException("Parqueadero sin espacio disponible");
            }
        }

        private async Task ValidaExistePlaca(string placa)
        {
            var ExisteVeh = await _genericRepoParqueadero.GetManyAsync(x => x.Placa == placa && x.Estado == EstadoVehiculo.Activo);
            if (ExisteVeh.Any()) { throw new CoreBusinessException("Ya existe un vehiculo con esta placa"); }
        }


    }
}
