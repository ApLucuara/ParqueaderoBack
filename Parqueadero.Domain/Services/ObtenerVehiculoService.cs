using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Exceptions;
using Parqueadero.Domain.Ports;

namespace Parqueadero.Domain.Services
{
    [DomainService]
    public class ObtenerVehiculoService
    {
        private readonly IGenericRepository<Domain.Entities.Parqueadero> _genericRepoParqueadero;
        private readonly IGenericRepository<Parametrizacion> _genericRepoParametrizacion;

        public ObtenerVehiculoService(IGenericRepository<Entities.Parqueadero> genericRepoParqueadero, IGenericRepository<Parametrizacion> genericRepoParametrizacion)
        {
            _genericRepoParqueadero = genericRepoParqueadero;
            _genericRepoParametrizacion = genericRepoParametrizacion;
        }

        public async Task<Entities.Parqueadero> ObtenerVehiculo(string Placa)
        {
            if (string.IsNullOrEmpty(Placa)){ throw new BussinesExceptions("La placa no puede ser nula o vacía.");}
            var vehiculo = await _genericRepoParqueadero.GetOneAsync(x => x.Placa == Placa && x.Estado == Enumerations.EstadoVehiculo.Activo);
            if (vehiculo == null) { throw new BussinesExceptions("El vehiculo no se encuentra en el parqueadero"); }
            await CalcularValorVehiculo(vehiculo);
            return vehiculo;
        }



        private async Task CalcularValorVehiculo(Entities.Parqueadero vehiculo)
        {
            var respuesta = await _genericRepoParametrizacion.GetManyAsync(para => para.TipoVehiculo == vehiculo.TipoVehiculo);
            var parametrizacion = respuesta.FirstOrDefault();
            if (parametrizacion == null) { throw new BussinesExceptions("No se encontro parametrización"); }

            (int DiasParqueado, int HorasParqueadeo) = GetTotalTiempoParqueadero(vehiculo, parametrizacion.HorasDia);

            if (DiasParqueado > 0 && parametrizacion.ValorDia == 0) {throw new BussinesExceptions("No se encontro parametrizado el valor del día");}
            var ValorDias = parametrizacion.ValorDia * DiasParqueado;
            //if(HorasParqueadeo > 0 && parametrizacion.ValorHora ==0 ) { throw new BussinesExceptions("No se encontro parametrizado el valor de la hora"); }
            var ValorHoras = parametrizacion.ValorHora * HorasParqueadeo;
            vehiculo.Valor = ValorDias + ValorHoras;
            if (vehiculo.TipoVehiculo == Enumerations.TipoVehiculo.Moto && vehiculo.Cilindraje > parametrizacion.CilindrajeSobrecargo)
            {
                vehiculo.Valor = vehiculo.Valor + parametrizacion.ValorSobrecargo;
            }
        }

        private (int Dias, int Horas) GetTotalTiempoParqueadero(Domain.Entities.Parqueadero vehiculo, int horasMaximasDia)
        {
            try
            {
                DateTime fechaSalida = vehiculo.FechaSalida ?? DateTime.Now;
                TimeSpan tiempoTotal = fechaSalida - vehiculo.FechaIngreso;
                int totalHoras = (int)Math.Ceiling(tiempoTotal.TotalHours);
                vehiculo.HorasParqueo = (short)totalHoras;
                int dias = totalHoras / 24;
                int horas = totalHoras % 24;
                if (horas > horasMaximasDia)
                {
                    dias++;
                    horas = 0;
                }
                return (dias, horas);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
