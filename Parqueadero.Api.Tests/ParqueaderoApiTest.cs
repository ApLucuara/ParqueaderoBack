using Microsoft.Extensions.DependencyInjection;
using Parqueadero.Domain.Dtos;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Parqueadero.Domain.Ports;
using System.Text;
using System.Text.Json;


namespace Parqueadero.Api.Tests
{

    public class ParqueaderoApiTest
    {
        [Fact]
        public async Task GetbyPlateSuccess()
        {
            await using var webApp = new ApiApp();
            await CargarParametrizacion(webApp);
            var serviceCollection = webApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Domain.Entities.Parqueadero>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var voter = new Domain.Entities.Parqueadero(Domain.Enumerations.TipoVehiculo.Carro, "GTR459", null, DateTime.Now);
            var voterCreated = await repository.AddAsync(voter);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
            var client = webApp.CreateClient();
            var singleVoter = await client.GetFromJsonAsync<ParqueaderoDto>($"/api/Parqueadero/{voterCreated.Placa}");
            Assert.True(singleVoter is not null && singleVoter is ParqueaderoDto);
        }

        [Fact]
        public async Task PostExitoso()
        {
            await using var webApp = new ApiApp();
            await CargarParametrizacion(webApp);
            var carro = new Domain.Entities.Parqueadero(TipoVehiculo.Carro, "ezz33", null, new DateTime(2023, 4, 7));

            var client = webApp.CreateClient();
            var content = JsonSerializer.Serialize(carro);

            var contents = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/Parqueadero", contents);
            response.EnsureSuccessStatusCode();

            var respuestavehicle = await client.GetAsync("api/Parqueadero/" + carro.Placa);
            respuestavehicle.EnsureSuccessStatusCode();
            var responseString = await respuestavehicle.Content.ReadAsStringAsync();
            var carrox = JsonSerializer.Deserialize<Domain.Entities.Parqueadero>(responseString);
            Assert.True(carrox != null && carrox is Domain.Entities.Parqueadero);
        }

        [Fact]
        public async Task GetSuccess()
        {
            await using var webApp = new ApiApp();
            await CargarParametrizacion(webApp);
            var serviceCollection = webApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Domain.Entities.Parqueadero>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var vehiculo = new Domain.Entities.Parqueadero(Domain.Enumerations.TipoVehiculo.Carro, "GTR459", null, DateTime.Now);
            await repository.AddAsync(vehiculo);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
            var client = webApp.CreateClient();
            var respuestavehicle = await client.GetAsync("api/Parqueadero");
             respuestavehicle.EnsureSuccessStatusCode();
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var responseString = await respuestavehicle.Content.ReadAsStringAsync();
            var carrox = JsonSerializer.Deserialize<List<ParqueaderoDto>>(responseString, options);
            Assert.True(carrox != null && carrox.Count > 0);
        }

        private async Task CargarParametrizacion(ApiApp webApp)
        {
            var serviceCollection = webApp.GetServiceCollection();
            using var scope = serviceCollection.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IGenericRepository<Parametrizacion>>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var parametro1 = new Parametrizacion()
            {
                TipoVehiculo = TipoVehiculo.Carro,
                ValorHora = 1000,
                ValorDia = 8000,
                Capacidad = 20,
                PicoPlacaLunes = "1,2",
                PicoPlacaMartes = "3,4",
                PicoPlacaMiercoles = "5,6",
                PicoPlacaJueves = "7,8",
                PicoPlacaViernes = "9,0"
            };
            var parametroCreado = await repository.AddAsync(parametro1);
            await unitOfWork.SaveAsync(new CancellationTokenSource().Token);
        }




    }
}
