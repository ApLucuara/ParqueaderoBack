using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Exceptions;
using Parqueadero.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var resultado = await _genericRepoParqueadero.GetManyAsync(x => x.Placa == Placa && x.Estado == Enumerations.EstadoVehiculo.Activo);
            var vehiculo = resultado.FirstOrDefault();
            if (vehiculo == null) { throw new BussinesExceptions("El vehiculo no se encuentra en el parqueadero"); }
            var Param = await _genericRepoParametrizacion.GetManyAsync(para => para.TipoVehiculo == vehiculo.TipoVehiculo);
            var ParamVeh = Param.FirstOrDefault();
            (short DiasParqueado, short HorasParqueadeo) = GetTotalTempoParqueadero(vehiculo, ParamVeh.HorasDia);
            var ValorDias = ParamVeh.ValorDia * DiasParqueado;
            var ValorHoras = ParamVeh.ValorHora * HorasParqueadeo;
            vehiculo.Valor = ValorDias + ValorHoras;
            if (vehiculo.TipoVehiculo == Enumerations.TipoVehiculo.Moto && vehiculo.Cilindraje > ParamVeh.CilindrajeSobrecargo)
            {
                vehiculo.Valor = vehiculo.Valor + ParamVeh.ValorSobrecargo;
            }
            return vehiculo;
        }

        private (short, short) GetTotalTempoParqueadero(Domain.Entities.Parqueadero Vehiculo, byte HorasDia)
        {
            if (Vehiculo.FechaSalida == null) Vehiculo.FechaSalida = DateTime.Now;
            decimal TotalHoras = (decimal)(Vehiculo.FechaSalida.Value - Vehiculo.FechaIngreso).TotalHours;
            Vehiculo.HorasParqueo = (short)Math.Ceiling(TotalHoras);
            short Dias;
            decimal RestanteHoras;
            short Horas = 0;

            if (TotalHoras >= HorasDia && TotalHoras <= 24)
            {
                Dias = 1;
            }
            else
            {
                Dias = (short)Math.Floor(Convert.ToDecimal(TotalHoras / 24));
                RestanteHoras = (TotalHoras % 24);
                if (RestanteHoras >= HorasDia && RestanteHoras <= 24)
                {
                    Dias += 1;
                    Horas = 0;
                }
                else
                {
                    Horas = (short)Math.Ceiling(Convert.ToDecimal(RestanteHoras % 60));
                }
            }
            return (Dias, Horas);
        }


    }
}
