using Parqueadero.Domain.Enumerations;
using System.Text.Json.Serialization;

namespace Parqueadero.Domain.Dtos
{
    public class ParqueaderoDto { 
       
        public TipoVehiculo TipoVehiculo { get; set; }

        public string Placa { get; set; }

        public short? Cilindraje { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaSalida { get; set; }
        public short? HorasParqueo { get; set; }

        public decimal? Valor { get; set; }

        public EstadoVehiculo Estado { get; set; }
    }
}
