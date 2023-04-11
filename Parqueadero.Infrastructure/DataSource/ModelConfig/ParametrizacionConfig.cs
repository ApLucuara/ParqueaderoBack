using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parqueadero.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parqueadero.Infrastructure.DataSource.ModelConfig
{
    public class ParametrizacionConfig : IEntityTypeConfiguration<Domain.Entities.Parametrizacion>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Parametrizacion> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK_Paramerizacion");

            builder.ToTable("Parametrizacion");

            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.PicoPlacaJueves)
                .HasMaxLength(10)
                .HasColumnName("PicoPlaca_Jueves");
            builder.Property(e => e.PicoPlacaLunes)
                .HasMaxLength(10)
                .HasColumnName("PicoPlaca_Lunes");
            builder.Property(e => e.PicoPlacaMartes)
                .HasMaxLength(10)
                .HasColumnName("PicoPlaca_Martes");
            builder.Property(e => e.PicoPlacaMiercoles)
                .HasMaxLength(10)
                .HasColumnName("PicoPlaca_Miercoles");
            builder.Property(e => e.PicoPlacaViernes)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("PicoPlaca_Viernes");
            builder.Property(e => e.ValorDia).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ValorHora).HasColumnType("decimal(18, 2)");
            builder.Property(e => e.ValorSobrecargo).HasColumnType("decimal(18, 2)");
            builder.Property<TipoVehiculo>(e => e.TipoVehiculo).HasColumnType("tinyint");

        }

    }
}
