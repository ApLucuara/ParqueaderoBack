using NSubstitute;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Exceptions;
using Parqueadero.Domain.Ports;
using Parqueadero.Domain.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Domain.Tests.Parqueadero
{
    public class ObtenerVehiculoTest
    {
        readonly ObtenerVehiculoService _service = default!;

        private readonly IGenericRepository<Entities.Parqueadero> _genericRepoParqueadero;
        private readonly IGenericRepository<Parametrizacion> _genericRepoParametrizacion;

        public ObtenerVehiculoTest()
        {
            _genericRepoParqueadero = Substitute.For<IGenericRepository<Entities.Parqueadero>>();
            _genericRepoParametrizacion = Substitute.For<IGenericRepository<Parametrizacion>>();
            _service = new ObtenerVehiculoService(_genericRepoParqueadero, _genericRepoParametrizacion);
        }

        [Fact]
        public async void ObtenerVehiculoPlacaVacia()
        {
            try
            {
                var resultado = await _service.ObtenerVehiculo("");
                Assert.Fail("No deberia");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"La placa no puede ser nula o vacía."));
            }
        }

        [Fact]
        public async void ObtenerVehiculoExitoso()
        {
            try
            {
                ConfiguracionParametros();
                ConfiguracionVehiculo();
                var resultado = await _service.ObtenerVehiculo("ABC123");
                Assert.True(resultado is Entities.Parqueadero);
            }
            catch (CoreBusinessException)
            {
                Assert.Fail("No deberia");
            }
        }

        [Fact]
        public async void ObtenerVehiculoFallo()
        {
            try
            {
                await _service.ObtenerVehiculo("XXXXXX");
                Assert.Fail("No deberia llegar");
            }
            catch (CoreBusinessException wce)
            {
                Assert.True(wce.Message.Equals($"El vehiculo no se encuentra en el parqueadero"));
            }
        }

        [Fact]
        public async void ValidaNoHayParametrizacion()
        {
            try
            {
                ConfiguracionVehiculo();
                var resultado = await _service.ObtenerVehiculo("ABC123");
                Assert.Fail("No deberia");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"No se encontro parametrización"));
            }
        }


        [Fact]
        public async void ValidaNoHayParametrizacionDeValorDia()
        {
            try
            {
                ConfiguracionParametrosConValorDiaCero();
                ConfiguracionVehiculo();
                var resultado = await _service.ObtenerVehiculo("ABC123");
                Assert.Fail("No deberia");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"No se encontro parametrizado el valor del día"));
            }
        }


        private void ConfiguracionVehiculo()
        {
            var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "ABC123", null, new DateTime(2023, 03, 31, 12, 00, 00));
            _genericRepoParqueadero.GetOneAsync(Arg.Any<Expression<Func<Entities.Parqueadero, bool>>>()).Returns(Task.FromResult(vehiculo));
        }

        private void ConfiguracionParametros()
        {
            //IEnumerable<Parametrizacion> Lista = new List<Parametrizacion>()
            //{
            //    new Parametrizacion() { TipoVehiculo=TipoVehiculo.Carro,ValorHora=1000,ValorDia=8000, Capacidad = 20,PicoPlacaLunes ="1,2",PicoPlacaMartes="3,4", PicoPlacaMiercoles = "5,6",PicoPlacaJueves="7,8",PicoPlacaViernes="9,0" },
            //    new Parametrizacion() { TipoVehiculo=TipoVehiculo.Moto,ValorHora=500,Capacidad = 10, PicoPlacaLunes ="1,2,3,4",PicoPlacaMartes="5,6,7,8", PicoPlacaMiercoles = "9,0,1,2",PicoPlacaJueves="3,4,5,6",PicoPlacaViernes="7,8,9,0",CilindrajeSobrecargo=500,ValorSobrecargo=200,HorasDia=9 }
            // };
            var Parametrizacion = new Parametrizacion { TipoVehiculo = TipoVehiculo.Carro, ValorHora = 1000, ValorDia = 8000, Capacidad = 20, PicoPlacaLunes = "1,2", PicoPlacaMartes = "3,4", PicoPlacaMiercoles = "5,6", PicoPlacaJueves = "7,8", PicoPlacaViernes = "9,0" };
            _genericRepoParametrizacion.GetOneAsync(Arg.Any<Expression<Func<Parametrizacion, bool>>>()).Returns(Task.FromResult(Parametrizacion));
        }
        
        private void ConfiguracionParametrosConValorDiaCero()
        {
            var ParamCarro = new Parametrizacion { TipoVehiculo = TipoVehiculo.Carro, ValorHora = 1000, ValorDia = 0, Capacidad = 20, PicoPlacaLunes = "1,2", PicoPlacaMartes = "3,4", PicoPlacaMiercoles = "5,6", PicoPlacaJueves = "7,8", PicoPlacaViernes = "9,0" };
            _genericRepoParametrizacion.GetOneAsync(Arg.Any<Expression<Func<Parametrizacion, bool>>>()).Returns(Task.FromResult(ParamCarro));
        }






    }
}
