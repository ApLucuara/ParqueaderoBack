using Parqueadero.Domain.Enumerations;

namespace Parqueadero.Domain.Entities;

public class Parametrizacion : DomainEntity
{
    public TipoVehiculo TipoVehiculo { get; set; }

    public decimal ValorHora { get; set; }

    public decimal ValorDia { get; set; }

    public int Capacidad { get; set; }

    public string PicoPlacaLunes { get; set; } = default!;

    public string PicoPlacaMartes { get; set; } = default!;

    public string PicoPlacaMiercoles { get; set; } = default!;

    public string PicoPlacaJueves { get; set; } = default!;

    public string PicoPlacaViernes { get; set; } = default!;

    public short? CilindrajeSobrecargo { get; set; } 

    public decimal? ValorSobrecargo { get; set; }
    public byte HorasDia { get; set; }
}
