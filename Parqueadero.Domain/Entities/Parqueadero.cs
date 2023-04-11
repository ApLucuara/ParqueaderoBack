
using Parqueadero.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace Parqueadero.Domain.Entities;

public class Parqueadero : DomainEntity
{
    public Parqueadero(TipoVehiculo tipoVehiculo, string placa, short? cilindraje, DateTime fechaIngreso)
    {
        TipoVehiculo = tipoVehiculo;
        Placa = placa;
        Cilindraje = cilindraje;
        FechaIngreso = fechaIngreso;
        Estado = EstadoVehiculo.Activo;

    }

    public TipoVehiculo TipoVehiculo { get; init; }

    public string Placa { get; init; }

    public short? Cilindraje { get; set; }

    public DateTime FechaIngreso { get; init; }

    public DateTime? FechaSalida { get; set; }
    public short? HorasParqueo { get; set; }

    public decimal? Valor { get; set; }

    public EstadoVehiculo Estado { get; set; }






}
