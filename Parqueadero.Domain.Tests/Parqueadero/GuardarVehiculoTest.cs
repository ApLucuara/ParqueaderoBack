using NSubstitute;
using NSubstitute.Extensions;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Exceptions;
using Parqueadero.Domain.Ports;
using Parqueadero.Domain.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Domain.Tests.Parqueadero
{
    public class GuardarVehiculoTest
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IParqueaderoRepository _repository = default!;
        private readonly IGenericRepository<Entities.Parqueadero> _genericRepoParqueadero;
        private readonly IGenericRepository<Parametrizacion> _genericRepoParametrizacion;
        readonly GuardarVehiculoService _service = default!;
        public GuardarVehiculoTest()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _repository = Substitute.For<IParqueaderoRepository>();
            _genericRepoParqueadero = Substitute.For<IGenericRepository<Entities.Parqueadero>>();
            _genericRepoParametrizacion = Substitute.For<IGenericRepository<Parametrizacion>>();
            _service = new GuardarVehiculoService(_unitOfWork, _repository, _genericRepoParqueadero, _genericRepoParametrizacion);
        }

        [Fact]
        public async Task GuardarVehiculoExitoso()
        {
            try
            {
                ConfiguracionParametros();
                var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "ABC123", null, new DateTime(2023, 04, 03, 12, 00, 00));
                _repository.SaveParqueadero(Arg.Any<Entities.Parqueadero>()).Returns(vehiculo);
                _unitOfWork.SaveAsync(Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);
                var resultado = await _service.RecordParqueaderoAsync(vehiculo);
                await _repository.Received().SaveParqueadero(Arg.Any<Entities.Parqueadero>());
                await _unitOfWork.Received().SaveAsync(Arg.Any<CancellationToken>());
                Assert.True(resultado is Entities.Parqueadero);
            }
            catch (Exception)
            {
                Assert.Fail("No deberia");
            }

        }

        [Fact]
        public async void ValidaPicoyPlaca()
        {
            try
            {
                ConfiguracionParametros();
                var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "ABC49A", null, new DateTime(2023, 03, 31, 12, 00, 00));
                await _service.RecordParqueaderoAsync(vehiculo);
                Assert.Fail("No deberia llegar");
            }
            catch (CoreBusinessException wce)
            {
                Assert.True(wce.Message.Equals($"El vehiculo tiene pico y placa, no puede ingresar"));
            }

        }

        [Fact]
        public async void ValidaParqueaderoLleno()
        {
            try
            {
                ConfiguracionParametros();
                ConfiguracionListadoVehiculos();
                var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "RUV215", null, new DateTime(2023, 03, 31, 12, 00, 00));
                _genericRepoParqueadero.AddAsync(Arg.Any<Entities.Parqueadero>()).Returns(Task.FromResult(vehiculo));
                await _service.RecordParqueaderoAsync(vehiculo);
                Assert.Fail("No deberia llegar");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"Parqueadero sin espacio disponible"));
            }
        }

        [Fact]
        public async void ValidaExistePlaca()
        {
            try
            {
                ConfiguracionParametros();
                ConfiguracionValidaExistePlaca();
                var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "XTZ761", null, new DateTime(2023, 03, 31, 12, 00, 00));
                _genericRepoParqueadero.AddAsync(Arg.Any<Entities.Parqueadero>()).Returns(Task.FromResult(vehiculo));
                await _service.RecordParqueaderoAsync(vehiculo);
                Assert.Fail("No deberia llegar");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"Ya existe un vehiculo con esta placa"));
            }

        }

        [Fact]
        public async Task PlacaInvalida()
        {
            try
            {
                ConfiguracionParametros();
                var vehiculo = new Entities.Parqueadero(Enumerations.TipoVehiculo.Carro, "INVALIDA", null, new DateTime(2023, 03, 28, 12, 00, 00));
                _genericRepoParqueadero.AddAsync(Arg.Any<Entities.Parqueadero>()).Returns(Task.FromResult(vehiculo));
                await _service.RecordParqueaderoAsync(vehiculo);
                Assert.Fail("No deberia llegar");
            }
            catch (CoreBusinessException ex)
            {
                Assert.True(ex.Message.Equals($"Placa no valida"));
            }


        }


        private void ConfiguracionParametros()
        {
            IEnumerable<Parametrizacion> Lista = new List<Parametrizacion>()
            {
                new Parametrizacion() { TipoVehiculo=TipoVehiculo.Carro,ValorHora=1000,ValorDia=8000, Capacidad = 20,PicoPlacaLunes ="1,2",PicoPlacaMartes="3,4", PicoPlacaMiercoles = "5,6",PicoPlacaJueves="7,8",PicoPlacaViernes="9,0" },
                new Parametrizacion() { TipoVehiculo=TipoVehiculo.Moto,ValorHora=500,Capacidad = 10, PicoPlacaLunes ="1,2,3,4",PicoPlacaMartes="5,6,7,8", PicoPlacaMiercoles = "9,0,1,2",PicoPlacaJueves="3,4,5,6",PicoPlacaViernes="7,8,9,0",CilindrajeSobrecargo=500,ValorSobrecargo=200,HorasDia=9 }
             };

            _genericRepoParametrizacion.GetManyAsync(Arg.Any<Expression<Func<Parametrizacion, bool>>>()).Returns(
                Task.FromResult(Lista));
        }

        private void ConfiguracionListadoVehiculos()
        {
            IEnumerable<Entities.Parqueadero> ListaVehiculos = new List<Entities.Parqueadero>();
            for (int i = 0; i < 20; i++)
            {
                Entities.Parqueadero vehiculo = new Entities.Parqueadero(
                    TipoVehiculo.Carro,
                    $"ABC12{i}",
                    0,
                    DateTime.Now.AddDays(-i)
                );

                ((List<Entities.Parqueadero>)ListaVehiculos).Add(vehiculo);
            }
            _genericRepoParqueadero.GetManyAsync(Arg.Any<Expression<Func<Entities.Parqueadero, bool>>>()).Returns(Task.FromResult(ListaVehiculos));
        }

        private void ConfiguracionValidaExistePlaca()
        {
            IEnumerable<Entities.Parqueadero> ListaVehiculos = new List<Entities.Parqueadero>();
            Entities.Parqueadero vehiculo = new Entities.Parqueadero(TipoVehiculo.Carro, $"XTZ761", 0, DateTime.Now);

            ((List<Entities.Parqueadero>)ListaVehiculos).Add(vehiculo);
            _genericRepoParqueadero.GetManyAsync(Arg.Any<Expression<Func<Entities.Parqueadero, bool>>>()).Returns(Task.FromResult(ListaVehiculos));
        }

    }
}
