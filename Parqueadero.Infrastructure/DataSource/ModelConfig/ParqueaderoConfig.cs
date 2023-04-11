using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parqueadero.Domain.Entities;
using Parqueadero.Domain.Enumerations;
using static Dapper.SqlMapper;

namespace Parqueadero.Infrastructure.DataSource.ModelConfig
{
    public class ParqueaderoConfig : IEntityTypeConfiguration<Domain.Entities.Parqueadero>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Parqueadero> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_RegistroVehiculo");
            builder.ToTable("Parqueadero");
            builder.Property(e => e.Id).HasColumnName("ID");
            builder.Property(e => e.FechaIngreso).HasColumnType("datetime");
            builder.Property(e => e.FechaSalida).HasColumnType("datetime");
            builder.Property(e => e.Placa).HasMaxLength(6);
            builder.Property(e => e.Valor).HasColumnType("decimal(18, 2)");
            builder.Property<TipoVehiculo>(e => e.TipoVehiculo).HasColumnType("tinyint");
            builder.Property<EstadoVehiculo>(e => e.Estado).HasColumnType("tinyint");
        }

    }
}
