
using Parqueadero.Infrastructure.DataSource;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using Microsoft.Extensions.Configuration;

namespace Parqueadero.Api.Tests;

class ApiApp : WebApplicationFactory<Program>
{

    readonly Guid _id;

    public Guid UserId => this._id;

    public ApiApp()
    {
        _id = Guid.NewGuid();
    }

    // We should use this service collection to access repos and seed data for tests
    public IServiceProvider GetServiceCollection()
    {
        return Services;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(svc =>
        {
            svc.RemoveAll(typeof(DbContextOptions<DataContext>));
            svc.AddDbContext<DataContext>(opt =>
            {
                opt.UseInMemoryDatabase("testdb");
            });

        });

        return base.CreateHost(builder);
    }


//    private void ConfiguracionParametros()
//    {
//        var contex =

//        List < Parametrizacion > ListaParam = new List<Parametrizacion>();
//        ListaParam.AddRange(new List<Parametrizacion>()
//         {
//     new Parametrizacion() { TipoVehiculo = TipoVehiculo.Carro,ValorHora = 1000,ValorDia = 8000, Capacidad = 20,PicoPlacaLunes = "1,2",PicoPlacaMartes = "3,4", PicoPlacaMiercoles = "5,6",PicoPlacaJueves = "7,8",PicoPlacaViernes = "9,0" },
//     new Parametrizacion() { TipoVehiculo = TipoVehiculo.Moto,ValorHora = 500,Capacidad = 10, PicoPlacaLunes = "1,2,3,4",PicoPlacaMartes = "5,6,7,8", PicoPlacaMiercoles = "9,0,1,2",PicoPlacaJueves = "3,4,5,6",PicoPlacaViernes = "7,8,9,0",CilindrajeSobrecargo = 500,ValorSobrecargo = 200,HorasDia = 9 }
//});
//    }


    //private void configuracion()
    //{
    //    using System.Data.SqlClient;

    //    // Cadena de conexión a la base de datos
    //    string connectionString = "Data Source=myServer;Initial Catalog=myDatabase;User ID=myUsername;Password=myPassword";

    //    // Crear la conexión
    //    SqlConnection connection = new SqlConnection(connectionString);

    //    // Abrir la conexión
    //    connection.Open();

    //    // Realizar operaciones de prueba
    //    SqlCommand command = new SqlCommand("INSERT INTO myTable (Name) VALUES ('John')", connection);
    //    command.ExecuteNonQuery();

    //    // Cerrar la conexión
    //    connection.Close();
    //}

}

