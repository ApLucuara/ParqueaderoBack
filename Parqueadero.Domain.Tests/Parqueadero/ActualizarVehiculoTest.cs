using NSubstitute;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Ports;
using Parqueadero.Domain.Services;
using System.Linq.Expressions;

namespace Parqueadero.Domain.Tests.Parqueadero
{
    public class ActualizarVehiculoTest
    {

        readonly IUnitOfWork _unitOfWork;
        readonly ActualizarVehiculoService _service = default!;
        readonly ObtenerVehiculoService _serviceObtenerVehiculo;

        private readonly IGenericRepository<Entities.Parqueadero> _genericRepoParqueadero;
        private readonly IGenericRepository<Parametrizacion> _genericRepoParametrizacion;

        public ActualizarVehiculoTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _genericRepoParqueadero = Substitute.For<IGenericRepository<Entities.Parqueadero>>();
            _genericRepoParametrizacion = Substitute.For<IGenericRepository<Parametrizacion>>();
            _serviceObtenerVehiculo = new ObtenerVehiculoService(_genericRepoParqueadero, _genericRepoParametrizacion);
            _service = new ActualizarVehiculoService(_unitOfWork, _serviceObtenerVehiculo, _genericRepoParqueadero);
        }

        [Fact]
        public async Task ActualizarVehiculoExitoso()
        {
            try
            {
                ConfiguracionParametros();
                ConfiguracionVehiculo();
                var resultado = await _service.ActualizarVehiculo("ABC123");
                Assert.True(resultado is true);
            }
            catch (Exception)
            {
                Assert.Fail("No deberia");
            }
        }

        private void ConfiguracionVehiculo()
        {
            var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "ABC123", null, new DateTime(2023, 04, 03, 07, 00, 00));
            _genericRepoParqueadero.GetOneAsync(Arg.Any<Expression<Func<Entities.Parqueadero, bool>>>()).Returns(Task.FromResult(vehiculo));
        }

        private void ConfiguracionParametros()
        {
            var Parametrizacion = new Parametrizacion { TipoVehiculo = TipoVehiculo.Carro, ValorHora = 1000, ValorDia = 8000, Capacidad = 20, PicoPlacaLunes = "1,2", PicoPlacaMartes = "3,4", PicoPlacaMiercoles = "5,6", PicoPlacaJueves = "7,8", PicoPlacaViernes = "9,0" };
            _genericRepoParametrizacion.GetOneAsync(Arg.Any<Expression<Func<Parametrizacion, bool>>>()).Returns(Task.FromResult(Parametrizacion));
        }

    }
}
